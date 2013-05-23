/*
Author: Tu Hoang
ESRGC 2013

Project ETL - Censusblock

Dashboard controller -- handle dashboard operations

*/

ESRGC.Controller.Dashboard = ESRGC.Class({
    name: 'Dashboard',
    refs: {
        overviewPanel: '#overview'
    },
    control: {
    },
    init: function () {
        var scope = this;
        var store = ESRGC.getStore('Submission');
        if (typeof store != 'undefined') {
            store.on('load', scope.onSubmissionLoad);

            setInterval(function () {
                store.loadContent();
            }, 30000); //load every 30seconds
        }
    },
    onSubmissionLoad: function (store, data) {
        var scope = ESRGC.getController('Dashboard');
        if (typeof data != 'undefined') {
            var panel = scope.getOverviewPanel();
            panel.replaceWith($(scope.getRef('overviewPanel'), data));
        }
    }
}, ESRGC.Controller.Base);