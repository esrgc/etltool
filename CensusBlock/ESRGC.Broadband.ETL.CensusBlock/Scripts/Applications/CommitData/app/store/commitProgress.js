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
        log('An error has occured while updating status.');
    }

}, ESRGC.Store.Base);