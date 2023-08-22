"use strict"
$(document).ready(function () {
    $('#grupGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'groupId',
            loadUrl: `/AdminConsole/Group/Read`,
            //insertUrl: `/AdminConsole/Group/Create`,
            //updateUrl: `/AdminConsole/Group/Edit`,
            //deleteUrl: `/AdminConsole/Group/Delete`,
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
        columns: [
            {
            dataField: 'group',
            caption: 'Grup',
            validationRules: [{
                type: 'required',
                message: 'Grup harus diisi.',
            }],
            }, {
                dataField: 'groupName',
                caption: 'Nama Grup',
                validationRules: [{
                    type: 'required',
                    message: 'Nama Grup harus diisi.',
                }],
            },
        ],
        editing: {
            allowAdding: false,
            allowUpdating: false,
            allowDeleting: false
        },
    });
});