/*
ESRGC 2013
Project: Census Block ETL tool 

app.js app definition for Admin
*/

ESRGC.App = ESRGC.Class({
    name: 'Dashboard',
    controllers: ['Dashboard'],
    stores: ['Submission'],
    views: [],
    initialize: function () {
        ESRGC.Application.prototype.initialize.apply(this, arguments);
    }
}, ESRGC.Application);