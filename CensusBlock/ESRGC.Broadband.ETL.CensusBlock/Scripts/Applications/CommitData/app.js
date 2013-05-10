/*
ESRGC 2013
Project: Census Block ETL tool 

app.js app definition for Preview mapping
*/

ESRGC.App = ESRGC.Class({
    name: 'DataCommit',
    controllers: ['CommitData'],
    stores: ['CommitData', 'CommitProgress'],
    views: [],
    initialize: function () {
        ESRGC.Application.prototype.initialize.apply(this, arguments);
    }
}, ESRGC.Application);