using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Threading;
using System.Collections.Specialized;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Services
{
    public class OperationCompletedEventArgs : AsyncCompletedEventArgs
    {
        Submission _data;
        public OperationCompletedEventArgs(
            object data,
            Exception ex,
            bool cancelled,
            object userState)
            : base(ex, cancelled, userState) {
            _data = data as Submission;
        }
        //public GeocodeCompletedEventArgs():base(e
        public Submission Data {
            get {
                RaiseExceptionIfNecessary();
                return _data;
            }
        }
    }
    public class OperationProgressChangedEventArgs : ProgressChangedEventArgs
    {
        string _status;
        public OperationProgressChangedEventArgs(object status, int progressPercent, object userState)
            : base(progressPercent, userState) {
            _status = status as string;
        }
        public string Status {
            get {
                return _status;
            }
        }
    }
    public delegate void CompletedEventHandler(object sender, OperationCompletedEventArgs arg);
    public delegate void ProgressChangedEventHandler(OperationProgressChangedEventArgs e);
    public class DataCommit : Component
    {
        IUnitOfWork _workUnit = null;
        private HybridDictionary userStateToLifetime = new HybridDictionary();
        private delegate void workerEventHandler(object data, AsyncOperation asyncOp);
        private SendOrPostCallback onProgressReportCallback;
        private SendOrPostCallback onCompletedCallback;
        public event CompletedEventHandler DataCommitCompleted;
        public event ProgressChangedEventHandler DataCommitProgressChanged;

        public DataCommit(IUnitOfWork workUnit) {
            _workUnit = workUnit;
            initializeDelegates();
        }

        protected virtual void initializeDelegates() {
            onProgressReportCallback = reportProgress;
            onCompletedCallback = completeOperation;
        }

        void reportProgress(object state) {
            var e = state as OperationProgressChangedEventArgs;
            onProgressChanged(e);
        }
        void completeOperation(object state) {
            var e = state as OperationCompletedEventArgs;
            onOperationCompleted(e);
        }

        protected void onOperationCompleted(OperationCompletedEventArgs e) {
            if (this.DataCommitCompleted != null)
                DataCommitCompleted(this, e);
        }

        protected void onProgressChanged(OperationProgressChangedEventArgs e) {
            if (DataCommitProgressChanged != null)
                DataCommitProgressChanged(e);
        }
        private void completionMethod(object data, Exception ex, bool cancelled, AsyncOperation asyncOp) {
            if (!cancelled) {
                lock (userStateToLifetime) {
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
                var e = new OperationCompletedEventArgs(data, ex, cancelled, asyncOp.UserSuppliedState);
                asyncOp.PostOperationCompleted(onCompletedCallback, e);
            }
        }
        private void postProgress(string status, int progress, AsyncOperation asyncOp) {
            var progressArg = new OperationProgressChangedEventArgs(
                status + " (updated at " + DateTime.Now.ToShortTimeString() + ")",
                progress,
                asyncOp.UserSuppliedState);
            asyncOp.Post(onProgressReportCallback, progressArg);
        }
        private void workerMethod(object data, AsyncOperation asyncOp) {
            Exception exception = null;
            //get data and start committing
            var processingData = data as IEnumerable<ServiceCensusBlock>;
            Submission submission = null;
            if (!TaskCanceled(asyncOp.UserSuppliedState)) {
                try {
                    submission = new Submission() { Status = "Submitting" };
                    _workUnit.SubmissionRepository.InsertEntity(submission);
                    _workUnit.SaveChanges();
                    int count = 1, commitCount = 0;
                    foreach (var entry in processingData) {
                        System.Diagnostics.Debug.WriteLine(count);
                        entry.SubmissionID = submission.SubmissionID;
                        _workUnit.ServiceCensusRepository.InsertEntity(entry);
                        postProgress("Processing", 
                            (int)(((float)count / (float)processingData.Count()) * 100),
                            asyncOp
                        );
                        count++;
                        if(commitCount == 1000){//save every 1000 records
                            _workUnit.SaveChanges();
                            commitCount = 0;
                        }
                        commitCount++;
                    }
                    submission.Status = "Submitted";
                    _workUnit.SubmissionRepository.UpdateEntity(submission);
                    _workUnit.SaveChanges();//commit to database
                }
                catch (Exception ex) {
                    exception = ex;
                    submission.Status = "Incomplete";
                    _workUnit.SubmissionRepository.UpdateEntity(submission);
                    _workUnit.SaveChanges();//commit to database
                }
            }
            //call completion method to finish
            completionMethod(submission, exception, TaskCanceled(asyncOp.UserSuppliedState), asyncOp);
        }
        
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }
        // Utility method for determining if a 
        // task has been canceled.
        private bool TaskCanceled(object taskId) {
            return (userStateToLifetime[taskId] == null);
        }

        // This method cancels a pending asynchronous operation.
        public void CancelAsync(object taskId) {
            AsyncOperation asyncOp = userStateToLifetime[taskId] as AsyncOperation;
            if (asyncOp != null) {
                lock (userStateToLifetime.SyncRoot) {
                    userStateToLifetime.Remove(taskId);
                }
            }
        }
        public void CommitDataAsync(IEnumerable<ServiceCensusBlock> data, object taskId) {
            // Create an AsyncOperation for taskId.
            AsyncOperation asyncOp =
                AsyncOperationManager.CreateOperation(taskId);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (userStateToLifetime.SyncRoot) {
                if (userStateToLifetime.Contains(taskId)) {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                userStateToLifetime[taskId] = asyncOp;
            }
            //initialize work delegate (assign workermethod) and invoke on another thread
            workerEventHandler worker = workerMethod;
            worker.BeginInvoke(data, asyncOp, null, null);
        }
    }
}