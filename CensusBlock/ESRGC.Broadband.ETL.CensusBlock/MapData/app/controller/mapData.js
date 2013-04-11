/*
Author: Tu Hoang
ESRGC 2013

mapData.js 
Controller that handles the data mapping preview.
*/

ESRGC.Controller.MapData = ESRGC.Class({
    name: 'MapData',
    firstRowData: null,
    refs: {
        firstRowDataHidFld: '#firstRowData',
        dataForm: 'form.form-horizontal',
        selectControls: 'form select',
        previewPanel: '#previewPanel',
        labels: '.control-label',
        selectedOptions: 'form option:selected'
    },
    control: {
        selectControls: {
            change: 'onSelectItemChange'
        }
    },
    init: function () {
        var scope = this;
        //parse first row data
        scope.firstRowData = $.parseJSON(scope.getFirstRowDataHidFld().val());
        //log(scope.firstRowData);

        //update initial mapping
        $.each(scope.getSelectControls(), function (index, obj) {
            var id = $(obj).attr('id');
            //get value from data first row using key from the selected option
            var value = scope.firstRowData[$(obj).val()];
            scope.updateMapping(id, value);
        });
        //validate form
        scope.getDataForm().validate({
            debug: true,
            highlight: function (label) {
                $(label).closest('.control-group').addClass('error');
                $(label).closest('.control-group').removeClass('success');
            },
            success: function (label) {
                $(label).text('OK!').addClass('valid')
                    .closest('.control-group').addClass('success');
            },
            submitHandler: function (form) {
                if ($(form).valid())
                    form.submit();
                return false; // prevent normal form posting
            }
            //,
            //            messages: {
            //                'MappingObject.PROVNAMEColumn': 'Provider name is required.'
            //            }
        });
        scope.getDataForm().valid();

    },
    onSelectItemChange: function (event, object) {
        var scope = this;
        var id = $(object).attr('id');
        var value = scope.firstRowData[$(object).val()];
        var label = scope.getLabels().filter('[for="' + id + '"]').text();
        log(label + ': ' + value);
        scope.updateMapping(id, value);

    },
    /*---Privates---*/
    updateMapping: function (id, value) {
        var html;
        if (typeof value != 'undefined') {
            value = value == "" ? "Not available" : value;
            if (value == 'Not available')
                html = '<span class="label label-warning">' + value + '</span>';
            else
                html = '<span class="label label-success">' + value + '</span>';
        }
        else
            html = '<span class="label">Not mapped</span>';

        var previewElement = $('label[for="' + id + '"]')
            .filter(':not(.control-label)')
            .closest('td').siblings()
            .first();
        previewElement.html(html);
    }

}, ESRGC.Controller.Base);