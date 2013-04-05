/*
Author: Tu Hoang
ESRGC 2013

mapData.js 
Controller that handles the data mapping preview.
*/

ESRGC.Controller.MapData = ESRGC.Class({
    name: 'MapData',
    refs: {
        dataForm: 'form.form-horizontal'
    },
    control: {},
    init: function () {
        var scope = this;
        scope.getDataForm().validate({
            debug: true,
            highlight: function (label) {
                $(label).closest('.control-group').addClass('error');
                $(label).closest('.control-group').removeClass('success');
            },
            success: function (label) {
                $(label).text('OK!').addClass('valid')
                    .closest('.control-group').addClass('success');
            }//,
//            messages: {
//                'MappingObject.PPROVNAMEColumn': 'Provider name is required.'
//            }
        });
    }
}, ESRGC.Controller.Base);