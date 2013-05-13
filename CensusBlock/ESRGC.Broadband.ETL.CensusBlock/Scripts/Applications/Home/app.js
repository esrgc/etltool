/*
Author: Tu Hoang
ESRGC 2013

app.js Home

This app handles the user log in on home page

*/

ESRGC.App = ESRGC.Class({
    name: 'Security',
    controllers: ['Security'],
    views: [],
    stores: ['Login'],
    initialize: function () {
        ESRGC.Application.prototype.initialize.apply(this, arguments);
    }

}, ESRGC.Application);