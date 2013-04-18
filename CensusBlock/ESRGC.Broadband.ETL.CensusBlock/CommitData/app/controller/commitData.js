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
        progressBar: '.bar'
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
                //store new view
                scope.finishHtml = data;
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
            scope.pollKey = setInterval(function () {
                log('Fetching progress..');
                progressStore.loadJson('post');
            }, 5000); //refresh every 5 seconds
            //wire load event for progress store
            progressStore.on('load', function (store, data) {
                if (typeof data != 'undefined') {
                    var progress = data.progress;
                    if (progress != -1)
                        scope.getProgressBar().css('width', progress + '%');

                    var message = data.message;
                    if (typeof message != 'undefined')
                        scope.getStatusLabel().text(message);

                    if (progress == 100) {//stop polling when operation is completed
                        clearInterval(scope.pollKey);
                        setTimeout(function () {
                            scope.getStatusLabel().text('Reloading...');
                            $('body').html(scope.finishHtml);
                        }, 3000);
                    }
                }
            });
        }
        //initiate data submission
        var commitDataStore = ESRGC.getStore('CommitData');
        if (typeof commitDataStore != 'undefined') {
            commitDataStore.loadContent('post');
        }
    }

}, ESRGC.Controller.Base);