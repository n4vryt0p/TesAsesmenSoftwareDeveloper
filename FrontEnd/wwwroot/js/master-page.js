"use strict"
const { jsPDF } = window.jspdf;

//window.jsPDF = window.jspdf.jsPDF;
$(document).ready(function () {
    $('#masterGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `/product/Read`,
            insertUrl: `/product/Create`,
            updateUrl: `/product/Edit`,
            deleteUrl: `/product/Delete`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        remoteOperations: true,
        allowFilterEditor: false,
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
                dataField: 'nama',
                caption: 'Nama/Merek',
                validationRules: [{
                    type: 'required',
                    message: 'Nama is required.',
                }],
            },
            {
                dataField: 'deskripsi',
                caption: 'Deskripsi'
            },
            {
                dataField: 'tanggalKadaluarsa',
                caption: 'Kadaluarsa',
                dataType: 'date',
                format: 'dd MMM yyyy'
            },
            {
                dataField: 'hargaSatuan',
                caption: 'Harga Satuan',
                dataType: 'number'
            },
            {
                dataField: 'jumlah',
                caption: 'Stok',
                dataType: 'number'
            },
            {
                dataField: 'isActive',
                dataType: 'boolean'
            },
        ],
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'Master Info',
                showTitle: true,
                width: 700,
                height: 525,
            },
            form: {
                items: [{
                    itemType: 'group',
                    colCount: 1,
                    colSpan: 2,
                    items: ['nama', 'deskripsi', 'tanggalKadaluarsa', 'hargaSatuan', 'jumlah', 'isActive'],
                }],
            },
        },
        export: {
            enabled: true,
            formats: ['pdf'],
            allowExportSelectedData: true,
        },
        onExporting(e) {
            const doc = new jsPDF();

            DevExpress.pdfExporter.exportDataGrid({
                jsPDFDocument: doc,
                component: e.component,
                indent: 5,
            }).then(() => {
                doc.save('MaterProduct.pdf');
            });
        },
    });

    //const fileUploader = $('#file-uploader').dxFileUploader({
    //    multiple: false,
    //    accept: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    //    value: [],
    //    uploadMode: 'instantly',
    //    uploadUrl: '/master/upload',
    //    labelText: "Import from Excel",
    //    onBeforeSend: function (e) {
    //        $('html').block();
    //    },
    //    onFilesUploaded: function (e) {
    //        $('html').unblock();
    //    }
    //}).dxFileUploader('instance');
});
