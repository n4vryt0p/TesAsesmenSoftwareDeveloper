"use strict"
$(document).ready(function () {
    $('#privGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: ["roleId", "menuId"],
            loadUrl: `/AdminConsole/PrivilegeManage/Read`,
            insertUrl: `/AdminConsole/PrivilegeManage/Create`,
            updateUrl: `/AdminConsole/PrivilegeManage/Edit`,
            deleteUrl: `/AdminConsole/PrivilegeManage/Delete`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        grouping: {
            autoExpandAll: false,
            texts: {
                groupByThisColumn: "Group by This Column",
                groupContinuedMessage: "⬅ Back",
                groupContinuesMessage: "Next ➡",
                ungroup: "Ungroup",
                ungroupAll: "Ungroup All"
            },
        },
        groupPanel: {
            visible: true,
        },
        columns: [
            {
                dataField: 'roleId',
                caption: 'Role Name',
                validationRules: [{
                    type: 'required',
                    message: 'Nama role harus diisi.',
                }],
                allowSorting: false,
                lookup: {
                    dataSource: window.$roles,
                    valueExpr: 'id',
                    displayExpr: 'text',
                },
                groupIndex: 0,
            },
            {
                dataField: 'menuId',
                caption: 'Menu Name',
                allowSorting: false,
                lookup: {
                    dataSource: window.$menus,
                    valueExpr: 'id',
                    displayExpr: 'text',
                },
            },
            {
                dataField: 'isRead',
                caption: 'Read',
                dataType: "boolean",
            },
            {
                dataField: 'isAdd',
                caption: 'Add',
                dataType: "boolean",
            },
            {
                dataField: 'isEdit',
                caption: 'Edit',
                dataType: "boolean",
            },
            {
                dataField: 'isDelete',
                caption: 'Delete',
                dataType: "boolean",
            },
        ],
        showBorders: true,
        onRowUpdating: function (options) {
            $.extend(options.newData, $.extend({}, options.oldData, options.newData));
        },
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'Privileges',
                showTitle: true
            },
            form: {
                items: [
                    {
                        itemType: 'group',
                        colCount: 1,
                        colSpan: 2,
                        items: ['roleId', 'menuId'],
                    },
                    {
                        itemType: 'group',
                        colCount: 4,
                        colSpan: 2,
                        items: ['isRead', 'isAdd', 'isEdit', 'isDelete'],
                    }
                ],
            },
        },
    }).dxDataGrid('instance');
});