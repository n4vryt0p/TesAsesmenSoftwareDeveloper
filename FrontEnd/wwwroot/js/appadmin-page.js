"use strict"
$(document).ready(function () {
    $('#appGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'appId',
            loadUrl: `/AdminConsole/Application/Read`,
            //insertUrl: `/AdminConsole/Application/Create`,
            //updateUrl: `/AdminConsole/Application/Edit`,
            //deleteUrl: `/AdminConsole/Application/Delete`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        //remoteOperations: true,
        columns: [{
            dataField: 'appId',
            caption: 'Application Id',
        }, {
            dataField: 'appName',
            caption: 'Nama Aplikasi',
            validationRules: [{
                type: 'required',
                message: 'Nama Aplikasi harus diisi.',
            }],
        },
        ],
    })
});