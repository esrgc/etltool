/*
Tu Hoang
ESRGC April 2013
Project: Census block 
previewMapping.js controller to handle data submit (commit)
*/

ESRGC.Controller.CommitData = ESRGC.Class({
    name: 'CommitData',
    pollKey: null,
    finishHtml: "",
    refs: {
        commitBtn: '#commitBtn',
        statusPanel: '#statusContent',
        controlButtons: '.modal-footer button',
        statusLabel: '#submitStatus',
        progressBar: '.bar',
        submissionKey: '#submissionID'
    },
    control: {
        commitBtn: {
            click: 'onCommitBtnClick'
        }
    },
    init: function () {
        var scope = this;
        //assign load call back for commit data store
        var commitDataStore = ESRGC.getStore('CommitData');
        if (typeof commitDataStore != 'undefined') {
            commitDataStore.on('load', function (store, data) {
                scope.getProgressBar().css('width', '100%');
                //store new view
                scope.getStatusLabel().text('Reloading...');
                setTimeout(function () {
                    $('body').html(data);
                }, 2000);
            });
        }
    },
    onCommitBtnClick: function (event, object) {
        log('Submitting data...');
        var scope = this;
        //show status panel
        scope.getStatusPanel().removeClass('hide');
        //disable submit button and cancel button
        scope.getControlButtons().attr('disabled', 'disabled');

        var progressStore = ESRGC.getStore('CommitProgress');
        if (typeof progressStore != 'undefined') {
            //setting interval for status refresh        
            //            scope.pollKey = setInterval(function () {
            //                log('Fetching progress..');
            //                progressStore.loadJson('post');
            //            }, 10000); //refresh every 10 seconds
            //start tracking status
            setTimeout(function () {
                log('Fetching progress..');
                progressStore.setParams({
                    submissionID: scope.getSubmissionKey().val()
                });
                progressStore.loadJson('post');
            }, 5000);
            //wire load event for progress store
            progressStore.on('load', function (store, data) {
                if (typeof data != 'undefined') {
                    var progress = data.progress;
                    if (typeof progress != -1) {
                        scope.getProgressBar().css('width', progress + '%');
                        if (typeof data.message != 'undefined') {
                            var message = data.message.trim();

                            if (typeof message != 'undefined')
                                scope.getStatusLabel().text(message);
                        }
                        //still in progress..fetch new status after 10seconds
                        if (progress < 100) {
                            setTimeout(function () {
                                log('Fetching progress..');
                                progressStore.loadJson('post');
                            }, 10000);
                        }
                    }
                }
            });
        }
        //initiate data submission
        var commitDataStore = ESRGC.getStore('CommitData');
        commitDataStore.setParams({
            submissionID: scope.getSubmissionKey().val()
        });
        if (typeof commitDataStore != 'undefined') {
            commitDataStore.loadContent('post');
        }

    }

}, ESRGC.Controller.Base);