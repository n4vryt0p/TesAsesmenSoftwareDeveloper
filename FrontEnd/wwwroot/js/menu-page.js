"use strict"

$(document).ready(function () {
    $('#menuGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'menuId',
            loadUrl: `/AdminConsole/MenuManage/Read`,
            insertUrl: `/AdminConsole/MenuManage/Create`,
            updateUrl: `/AdminConsole/MenuManage/Edit`,
            deleteUrl: `/AdminConsole/MenuManage/Delete`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        columns: [
            {
                dataField: 'menuId',
                visible: false,
            },
            {
                dataField: 'parentMenuId',
                caption: 'Parent Menu',
                editorOptions: {
                    showClearButton: true
                },
                lookup: {
                    dataSource(options) {
                        return {
                            store: window.$grups,
                            filter: options.data ? ['zoomId', '=', options.data.appId] : null,
                        };
                    },
                    displayExpr: 'text',
                    valueExpr: 'id'
                },
                groupIndex: 1,
            },
            {
                dataField: 'menuName',
                caption: 'Menu Name',
                validationRules: [{
                    type: 'required',
                    message: 'Menu Name is required.',
                }],
            },
            {
                dataField: 'uniqueName',
                caption: 'Unique Name',
                validationRules: [{
                    type: 'required',
                    message: 'Unique Name is required.',
                }],
            },
            {
                dataField: 'menuLink',
                caption: 'Menu Link',
                validationRules: [{
                    type: 'required',
                    message: 'Menu Link is required.',
                }],
            },
            {
                dataField: 'icon',
                caption: 'Icon',
                cellTemplate: function (element, info) {
                    if (info.text != null) {
                        element.append("<span class='material-icons sidebar-menu-icon sidebar-menu-icon--left'>" + info.text + "</span>");
                    }
                },
            },
            {
                dataField: 'menuIndex',
                caption: 'Menu Index',
                dataType: "number",
                validationRules: [{
                    type: 'required',
                    message: 'Menu Index is required.',
                },
                {
                    type: 'range',
                    min: 1,
                    message: 'Menu Index > 0.',
                }
                ],
            },
            {
                dataField: 'isActive',
                caption: 'Active?',
                dataType: "boolean",
            },
        ],
        onEditorPreparing(e) {
            if (e.parentType === 'dataRow' && e.dataField === 'parentMenuId') {
                e.editorOptions.disabled = (typeof e.row.data.appId !== 'string');
            }
        },
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'Menu Info',
                showTitle: true,
                width: 700,
                height: 600,
            },
            form: {
                items: [{
                    itemType: 'group',
                    colSpan: 2,
                    items: ['parentMenuId', 'menuName', 'uniqueName', 'menuLink', 'icon', 'menuIndex', 'isActive'],
                }],
            },
        },
        //onRowUpdated(e) {
        //    const datas = e.data;
        //    //window.$grups = window.$grups.filter(dat => dat.id != datas.id);
        //},
        onRowRemoved(e) {
            let datas = e.data;
            window.$grups = window.$grups.filter(dat => dat.id != datas.menuId);
        },
        onRowInserted(e) {
            let datas = e.data;
            let lgth = window.$grups.length + 1;
            let dt = { "id": datas.menuId, "text": `"${datas.menuName}: ${datas.menuLink}"`, "zoomId": `"${datas.appId}"` }
            window.$grups.push(dt);
        },
    })
});