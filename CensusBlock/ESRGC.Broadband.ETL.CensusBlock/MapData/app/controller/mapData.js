/*
Author: Tu Hoang
ESRGC April 2013

mapData.js 
Controller that handles the data mapping preview.
*/

ESRGC.Controller.MapData = ESRGC.Class({
    name: 'MapData',
    firstRowData: null,
    defaultItemText: 'Use default',
    refs: {
        firstRowDataHidFld: '#firstRowData',
        dataForm: 'form.form-horizontal',
        selectControls: 'form select',
        previewPanel: '#previewPanel',
        labels: '.control-label',
        selectedOptions: 'form option:selected',
        mapDataBtn: 'input[value="Map Data"]',
        progressModal: '#progressPanel',
        defaultInputs: 'form input'
    },
    control: {
        selectControls: {
            change: 'onSelectItemChange'
        },
        mapDataBtn: {
            click: 'onMapDataBtnClick'
        },
        defaultInputs: {
            change: 'onDefaultValueChange'
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
            if ($(obj).val() == scope.defaultItemText) {//selected item is 'use default'
                var value = $(obj).siblings().filter('input')
                    .first().removeClass('hide').val();
                scope.updateMapping(id, value);
            }
            else {
                //get value from data first row using key from the selected option
                var value = scope.firstRowData[$(obj).val()];
                scope.updateMapping(id, value);
            }
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
                if ($(form).valid()) {
                    var scope = ESRGC.getController('MapData');
                    scope.getProgressModal().modal('show');
                    form.submit();
                }
                return false; // prevent normal form posting
            }
        });
        scope.getDataForm().valid();
        //prevent hitting enter to submit the form
        $('form, input').keyup(function (event, object) {
            if (event.keyCode == 13) {
                log('enter pressed');
                event.preventDefault();
                return false;
            }
        });
    },
    onSelectItemChange: function (event, object) {
        var scope = this;
        var option = $(object).val();
        var id = $(object).attr('id');
        var label = scope.getLabels().filter('[for="' + id + '"]').text();
        //find input control that defines 'default value'
        var input = $(object).siblings().last();
        log(input.val());

        //map data only if 'Use default' isn't selected
        if (option != scope.defaultItemText) {
            input.addClass('hide');
            var value = scope.firstRowData[option];
            log(label + ': ' + value);
            scope.updateMapping(id, value);
        }
        else {
            input.removeClass('hide');
            input.focus();
            scope.updateMapping(id, input.val());
        }
    },
    onDefaultValueChange: function (event, object) {
        var scope = this;
        var selectControlId = $(object).siblings().filter('select').attr('id');
        var value = $(object).val();//get value of the default input fields being focused
        log(selectControlId + ': ' + value);
        scope.updateMapping(selectControlId, value);
    },
    onMapDataBtnClick: function (event, object) {
        log('On submit');
        return false;
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