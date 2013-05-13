/*
Author: Tu Hoang
ESRGC May 2013

security.js
controller that handles security log in on home page. 
*/

ESRGC.Controller.Security = ESRGC.Class({
    name: 'Security',
    refs: {
        loginBtn: 'button[type="submit"]',
        loginPanel: 'div.security',
        loginForm: '.security form'
    },
    control: {
        loginForm: {
            submit: 'onFormSubmit'
        }
    },
    init: function () {
        var scope = this;
        var loginStore = ESRGC.getStore('Login');
        loginStore.on('load', scope.onLoginStoreLoad);
    },
    onFormSubmit: function (event, object) {
        event.preventDefault();
        var scope = this;
        var loginStore = ESRGC.getStore('Login');
        if (typeof loginStore != 'undefined') {
            var formData = scope.getFormData(scope.getLoginForm());
            loginStore.setParams(formData);
            loginStore.loadContent('post');
        }
        return false;
    },
    //store event handler
    onLoginStoreLoad: function (store, data) {
        var scope = ESRGC.getController('Security');
        var loginPanel = scope.getLoginPanel();
        if (data.search('html') == -1) {
            var content = $(data).find('div.security').html();
            loginPanel.html(content);
        }
        else {
            var newDoc = document.open("text/html", "replace");
            newDoc.write(data);
            newDoc.close();
        }
    }
}, ESRGC.Controller.Base);