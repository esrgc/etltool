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
using System.Diagnostics;
using System.Configuration;

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
            //debug code
            Debug.AutoFlush = true;
            Exception exception = null;
            Submission submission = null;
            //get data and start committing
            var processingData = data as IEnumerable<ServiceCensusBlock>;
            float total = (float)processingData.Count();
            //calculation variables
            int maxInterval = (int)(total/6);
            int count = 0, interval = 0;
            
            //start time
            var startTime = DateTime.Now;
            //checks if task is canceled
            if (!TaskCanceled(asyncOp.UserSuppliedState)) {
                //create a new database context (work unit)
                IUnitOfWork _workUnit = new DomainWorkUnit(new DomainDataContext());
                try {
                    submission = _workUnit.SubmissionRepository.GetEntityByID(_commitID);
                    if (submission == null)
                        throw new InvalidOperationException("Submission was not initiated.");
                    //timers
                    DateTime timer = DateTime.Now;
                    DateTime startTimer = DateTime.Now;
                    //start processing
                    foreach (var entry in processingData) {
                        entry.SubmissionID = submission.SubmissionID;
                        _workUnit.ServiceCensusRepository.InsertEntity(entry);

                        //tracking time
                        if (interval == 0)
                            timer = DateTime.Now;

                        //keeps count
                        count++;
                        interval++;
                        
                        //store every 1000 records
                        if (interval == maxInterval) {
                            _workUnit.SaveChanges();//Commit pending data
                            
                            //calculating time
                            var intervalTime = DateTime.Now - timer;
                            var timeElapsed = DateTime.Now - startTimer;
                            //calculate speed
                            int speedPerSec = (int)(interval / intervalTime.TotalSeconds);
                            //report progress
                            var status = "Processing";
                            var progressPercent = (int)(((float)count / total) * 100);

                            //store submission status
                            submission.Status = status;
                            submission.SubmissionTimeStarted = startTime;
                            submission.LastStatusUpdate = DateTime.Now;
                            submission.RecordsStored = count;
                            submission.ProgressPercentage = progressPercent;
                            submission.SpeedPerSecond = speedPerSec;
                            //update submission
                            _workUnit.SubmissionRepository.UpdateEntity(submission);
                            //commit changes
                            _workUnit.SaveChanges();
                            
                            //debug code
                            Debug.WriteLine("Processed " + count + " " +
                                DateTime.Now.ToLongTimeString() + " " +
                                progressPercent + "%. Time elapsed: " + 
                                timeElapsed.TotalSeconds + " seconds. Interval time: " + 
                                intervalTime.TotalSeconds + " seconds. Speed: " + 
                                speedPerSec + " rec/second.");
                            
                            //reset interval
                            interval = 0;
                            //renew context 
                            //_workUnit.RenewContext();
                        }
                    }
                    //update submission status when finished
                    submission.Status = "Submitted";
                    submission.RecordsStored = count;
                    submission.ProgressPercentage = 100;
                    submission.LastStatusUpdate = DateTime.Now;
                    submission.SubmissionTimeCompleted = DateTime.Now;
                    _workUnit.SubmissionRepository.UpdateEntity(submission);
                    _workUnit.SaveChanges();//commit to database
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