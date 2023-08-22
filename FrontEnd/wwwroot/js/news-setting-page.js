"use strict"
const typeX = [];
$(document).ready(function () {
    var dataGrid = $('#prodGrig').dxDataGrid({
        dataSource: typeX,
        keyExpr: 'id',
        remoteOperations: false,
        scrolling: {
            rowRenderingMode: 'virtual',
        },
        paging: {
            pageSize: 5,
        },
        pager: {
            visible: true,
            showPageSizeSelector: false,
            showInfo: true,
            showNavigationButtons: true,
        },
        editing: {
            mode: 'batch',
            allowUpdating: true,
            allowAdding: true,
            allowDeleting: true,
            selectTextOnEditStart: true,
            startEditAction: 'click',
        },
        repaintChangesOnly: true,
        columns: [
            {
                dataField: 'id',
                visible: false
            },
            {
                dataField: 'productId',
                caption: 'Nama Produk',
                lookup: {
                    dataSource: window.$prod,
                    displayExpr: 'text',
                    valueExpr: 'id',
                },
                editCellTemplate: dropDownBoxEditorTemplate,
            },
            {
                dataField: 'kuantitas',
                caption: 'Kuantitas',
                dataType: 'number',
            }
        ],
        
    }).dxDataGrid("instance");

    var dataGrid = $('#newsGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `Transaction/Read`,
            //insertUrl: `Transaction/Create`,
            //updateUrl: `Transaction/Edit`,
            //deleteUrl: `Transaction/Delete`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        keyExpr: 'id',
        remoteOperations: true,
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
        repaintChangesOnly: true,
        columns: [
            {
                dataField: 'alamatTujuan',
                caption: 'Alamat Tujuan'
            },
            {
                dataField: 'totalHarga',
                caption: 'Total Harga',
                dataType: 'number'
            },
            {
                dataField: 'tanggal',
                caption: 'Tanggal',
                dataType: 'date',
                format: 'dd MMM yyyy'
            },
        ],
        masterDetail: {
            enabled: true,
            template(container, options) {
                const currentEmployeeData = options.data;

                $('<div>')
                    .addClass('master-detail-caption')
                    .text(`Transaksi ID: ${currentEmployeeData.id}`)
                    .appendTo(container);

                $('<div>')
                    .dxDataGrid({
                        dataSource: DevExpress.data.AspNet.createStore({
                            key: ["productId", "transactionId"],
                            loadUrl: '/Transaction/ReadData?id=' + currentEmployeeData.transactionId,
                            onBeforeSend(method, ajaxOptions) {
                                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                                if (antiForgeryToken) {
                                    ajaxOptions.headers = {
                                        "RequestVerificationToken": antiForgeryToken
                                    };
                                };
                            },
                        }),
                        columnAutoWidth: true,
                        showBorders: true,
                        columns: [
                            {
                                dataField: 'namaProduk',
                                caption: 'Nama Produk',
                            },
                            {
                                dataField: 'kuantitas',
                                caption: 'Kuantitas',
                                dataType: 'number',
                            },
                            {
                                dataField: 'hargaSatuan',
                                caption: 'Harga Satuan',
                                dataType: 'number'
                            }],
                    }).appendTo(container);
            },
        },
        //editing: {
        //    mode: 'popup',
        //    allowAdding: true,
        //    allowUpdating: true,
        //    allowDeleting: true,
        //    popup: {
        //        title: 'Master Info',
        //        showTitle: true,
        //        width: 700,
        //        height: 525,
        //    },
        //    form: {
        //        items: [{
        //            itemType: 'group',
        //            colCount: 1,
        //            colSpan: 2,
        //            items: ['nama', 'deskripsi', 'tanggalKadaluarsa', 'hargaSatuan', 'jumlah', 'isActive'],
        //        }],
        //    },
        //},
    }).dxDataGrid("instance");
});

function dropDownBoxEditorTemplate(cellElement, cellInfo) {
    return $('<div>').dxDropDownBox({
        dropDownOptions: { width: 500 },
        dataSource: window.$prod,
        value: cellInfo.value,
        valueExpr: 'id',
        displayExpr: 'text',
        inputAttr: { 'aria-label': 'Product' },
        contentTemplate(e) {
            return $('<div>').dxDataGrid({
                dataSource: window.$prod,
                remoteOperations: true,
                columns: ['text', 'zoomId'],
                hoverStateEnabled: true,
                scrolling: { mode: 'virtual' },
                height: 250,
                selection: { mode: 'single' },
                selectedRowKeys: [cellInfo.value],
                focusedRowEnabled: true,
                focusedRowKey: cellInfo.value,
                onSelectionChanged(selectionChangedArgs) {
                    e.component.option('value', selectionChangedArgs.selectedRowKeys[0]);
                    cellInfo.setValue(selectionChangedArgs.selectedRowKeys[0]);
                    if (selectionChangedArgs.selectedRowKeys.length > 0) {
                        e.component.close();
                    }
                },
            });
        },
    });
}