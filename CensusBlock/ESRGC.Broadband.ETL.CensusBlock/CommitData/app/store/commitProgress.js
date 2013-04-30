/*
Tu Hoang
ESRGC April 2013

store: commitProgress.js -- handles updating data committing progress.
Updates status of data commitment
*/

ESRGC.Store.CommitProgress = ESRGC.Class({
    name: 'CommitProgress',
    url: '../Monitor/UpdateStatus',
    errorCallback: function () {
        var controller = ESRGC.getController('DataCommit');
        if (typeof controller != 'undefined') {
            var intervalKey = controller.pollKey;
            if (intervalKey)
                clearInterval(intervalKey);
        }
        alert('An error has occured while processing your data. Please try again later.');
    }

}, ESRGC.Store.Base);