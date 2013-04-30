using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Threading;
using System.Collections.Specialized;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Concrete;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL;

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
        private HybridDictionary userStateToLifetime = new HybridDictionary();
        private delegate void workerEventHandler(object data, AsyncOperation asyncOp);

        private SendOrPostCallback onProgressReportCallback;
        private SendOrPostCallback onCompletedCallback;

        public event CompletedEventHandler DataCommitCompleted;
        public event ProgressChangedEventHandler DataCommitProgressChanged;

        int _commitID;
        public DataCommit(int commitID) {
            _commitID = commitID;
            initializeDelegates();
        }

        protected virtual void initializeDelegates() {
            onProgressReportCallback = reportProgress;
            onCompletedCallback = completeOperation;
        }

        private void reportProgress(object state) {
            var e = state as OperationProgressChangedEventArgs;
            onProgressChanged(e);
        }
        private void completeOperation(object state) {
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
                status,
                progress,
                asyncOp.UserSuppliedState);
            asyncOp.Post(onProgressReportCallback, progressArg);
        }
        private void workerMethod(object data, AsyncOperation asyncOp) {
            Exception exception = null;
            //get data and start committing
            var processingData = data as IEnumerable<ServiceCensusBlock>;
            Submission submission = null;
            var startTime = DateTime.Now;
            if (!TaskCanceled(asyncOp.UserSuppliedState)) {
                //create a new database context (work unit)
                IUnitOfWork _workUnit = new DomainWorkUnit(new DomainDataContext());
                try {
                    submission = _workUnit.SubmissionRepository.GetEntityByID(_commitID);
                    if (submission == null)
                        throw new InvalidOperationException("Submission was not initiated.");

                    int count = 0;
                    float total = (float)processingData.Count();
                    foreach (var entry in processingData) {
                        entry.SubmissionID = submission.SubmissionID;
                        _workUnit.ServiceCensusRepository.InsertEntity(entry);
                        count++;//keeps count

                        //report progress
                        var status = "Processing";
                        var progressPercent = (int)(((float)count / total) * 100);
                        //store submission status
                        submission.Status = status;
                        submission.SubmissionTimeStarted = startTime;
                        submission.LastStatusUpdate = DateTime.Now;
                        submission.RecordsStored = count;
                        submission.ProgressPercentage = progressPercent;
                        //update submission
                        _workUnit.SubmissionRepository.UpdateEntity(submission);
                        //postProgress(status,
                        //    progressPercent,
                        //    asyncOp
                        //);
                        //reset interval

                        //commit changes
                        _workUnit.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Processed " + count);
                    }
                    //update submission status when finished
                    submission.Status = "Submitted";
                    submission.SubmissionTimeCompleted = DateTime.Now;
                    _workUnit.SubmissionRepository.UpdateEntity(submission);
                    _workUnit.SaveChanges();//commit to database
                    System.Diagnostics.Debug.WriteLine("Saved " + count);
                }
                catch (InvalidOperationException ex) {
                    exception = ex;
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