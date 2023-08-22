"use strict"
$(document).ready(function () {
    $('#roleGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'roleId',
            loadUrl: `/AdminConsole/RoleManage/Read`,
            insertUrl: `/AdminConsole/RoleManage/Create`,
            updateUrl: `/AdminConsole/RoleManage/Edit`,
            deleteUrl: `/AdminConsole/RoleManage/Delete`,
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
            dataField: 'roleName',
            caption: 'Nama Role',
            validationRules: [{
                type: 'required',
                message: 'Nama role harus diisi.',
            }],
            },
        ],
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'Role Info',
                showTitle: true,
                width: 700,
                height: 525,
            },
            form: {
                items: [{
                    itemType: 'group',
                    colSpan: 2,
                    items: ['roleName'],
                }],
            },
        },
    });
});