/*
Author: Tu Hoang
ESRGC May 2013

project ETL tool (Census block)
submission.js 
handles submission monitoring on dashboard homepage
*/

ESRGC.Store.Submission = ESRGC.Class({
    name: 'Submission',
    url: '',//current URL
    errorCallback: function () {
        log('An error has occured while updating status.');
    }

}, ESRGC.Store.Base);