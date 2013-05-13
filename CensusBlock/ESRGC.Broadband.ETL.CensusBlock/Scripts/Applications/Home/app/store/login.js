/*
Author: Tu Hoang
ESRGC May 2013

project ETL tool (Census block)
login.js 
handles login in request on home page
*/

ESRGC.Store.Login = ESRGC.Class({
    name: 'Login',
    url: $('.security form').attr('action')

}, ESRGC.Store.Base);