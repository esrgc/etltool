/*
Author: Tu Hoang
ESRGC 2012

Desktop browser

Controller
upload controller 
(implemented based on JQuery)

options: 
refs: object contains of references 
control: object of handlers

*/

ESRGC.Controller.Upload = ESRGC.Class({
    name: 'Upload',
    refs: {
        fileUpload: 'input#dataInput',
        fileNameSpan: 'span#fileName',
    },
    control: {
        fileUpload: {
            change: 'onFileChange'
        },
        form: {
            submit: 'onSubmit'
        }
    },
    init: function () { },
    onFileChange: function (event, object) {
        var scope = this;
        var file = $(object).val().split('\\').pop();
        log(file);
        scope.getFileNameSpan().text(file);
    }

}, ESRGC.Controller.Base);