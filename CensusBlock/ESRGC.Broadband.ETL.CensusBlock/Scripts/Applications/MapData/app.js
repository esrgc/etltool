/*
Author: Tu Hoang
ESRGC 2013

app.js MapData 

This app handles the preview of data on the client side. 
Helps user visualize their mapping.
*/

ESRGC.App = ESRGC.Class({
    name: 'MapData',
    controllers: ['MapData'],
    views: ['MapData'],
    stores: [],
    initialize: function () {
        ESRGC.Application.prototype.initialize.apply(this, arguments);
    }

}, ESRGC.Application);