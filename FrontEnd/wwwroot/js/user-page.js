"use strict"
$(document).ready(function () {
    var antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
    $('#userGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `/AdminConsole/usermanage/Read`,
            insertUrl: `/AdminConsole/usermanage/Create`,
            updateUrl: `/AdminConsole/usermanage/Edit`,
            deleteUrl: `/AdminConsole/usermanage/Delete`,
            onBeforeSend(method, ajaxOptions) {
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        remoteOperations: true,
        allowFilterEditor:false,
        //filterRow: {
        //    visible: true,
        //    applyFilter: "auto"
        //},
        searchPanel: {
            visible: false
        },
        scrolling: {
            rowRenderingMode: 'virtual',
        },
        paging: {
            pageSize: 10,
        },
        pager: {
            visible: true,
            allowedPageSizes: [5, 10, 'all'],
            showPageSizeSelector: true,
            showInfo: true,
            showNavigationButtons: true,
        },
        columns: [
            {
                dataField: 'userName',
                caption: 'User Name',
                selectedFilterOperation: "contains",
                filterOperations: [],
                validationRules: [{
                    type: 'required',
                    message: 'User Name is required.',
                }],
            },
            {
                dataField: 'pass',
                caption: 'Password',
                allowSorting: false,
                allowFiltering: false,
            },
            {
                dataField: 'fullName',
                caption: 'Full Name',
                selectedFilterOperation: "contains",
                filterOperations: [],
            },
            {
                dataField: 'email',
                caption: 'Email',
                selectedFilterOperation: "contains",
                filterOperations: [],
                validationRules: [{
                    type: 'required',
                    message: 'Email is required.',
                }],
            },
            {
                dataField: 'roles',
                caption: 'Roles',
                allowSorting: false,
                allowFiltering: false,
                editCellTemplate: tagBoxEditorTemplate,
                lookup: {
                    dataSource: window.$roles,
                    valueExpr: 'text',
                    displayExpr: 'text',
                },
                cellTemplate(container, options) {
                    const noBreakSpace = '\u00A0';
                    const text = (options.value || []).map((element) => options.column.lookup.calculateCellValue(element)).join(', ');
                    container.text(text || noBreakSpace).attr('title', text);
                },
            },
            {
                dataField: 'isActive',
                caption: 'Active',
                dataType: "boolean",
                //allowFiltering: false,
            },
        ],
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'User Info',
                showTitle: true,
                width: 700,
                height: 525,
            },
            form: {
                items: [{
                    itemType: 'group',
                    colSpan: 2,
                    items: ['userName','pass', 'fullName', 'email', 'roles', 'isActive'],
                }],
            },
        },
        onEditorPreparing: (e) => {
            if (e.dataField == "pass" && e.value == '******') e.editorOptions.disabled = true
        }
    });

    function tagBoxEditorTemplate(cellElement, cellInfo) {
        return $('<div>').dxTagBox({
            dataSource: window.$roles,
            value: cellInfo.value,
            valueExpr: 'text',
            displayExpr: 'text',
            showSelectionControls: true,
            maxDisplayedTags: 3,
            showMultiTagOnly: false,
            applyValueMode: 'useButtons',
            searchEnabled: true,
            onValueChanged(e) {
                cellInfo.setValue(e.value);
            },
            onSelectionChanged() {
                cellInfo.component.updateDimensions();
            },
        });
    }
});