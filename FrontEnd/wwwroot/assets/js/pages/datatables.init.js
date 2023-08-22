/*
Template Name: Velzon - Admin & Dashboard Template
Author: Themesbrand
Website: https://Themesbrand.com/
Contact: Themesbrand@gmail.com
File: datatables init js
*/

document.addEventListener('DOMContentLoaded', function () {
    let t = new DataTable('#example', {
        ajax: '../master/data'
    });

    $('#create-btn').on('click', function () {
        //$('html').unblock();
       
        //t.row.add([
        //    counter + '.1',
        //    counter + '.2',
        //    counter + '.3',
        //    counter + '.4',
        //    counter + '.5',
        //    counter + '.6',
        //    counter + '.7',
        //    counter + '.8',
        //    counter + '.9',
        //    counter + '.10',
        //    counter + '.11',
        //    counter + '.12'
        //]).draw(false);
    });

    if (document.getElementById("showModal")) {
        document.getElementById("showModal").addEventListener("show.bs.modal", function (e) {
            if (e.relatedTarget.classList.contains("edit-item-btn")) {
                document.getElementById("exampleModalLabel").innerHTML = "Edit Master";
                document.getElementById("showModal").querySelector(".modal-footer").style.display = "block";
                document.getElementById("add-btn").innerHTML = "Update";
            } else if (e.relatedTarget.classList.contains("add-btn")) {
                document.getElementById("exampleModalLabel").innerHTML = "Add Master";
                document.getElementById("showModal").querySelector(".modal-footer").style.display = "block";
                document.getElementById("add-btn").innerHTML = "Add master";
            } else {
                document.getElementById("exampleModalLabel").innerHTML = "List Master";
                document.getElementById("showModal").querySelector(".modal-footer").style.display = "none";
            }
        });
        //ischeckboxcheck();

        document.getElementById("showModal").addEventListener("hidden.bs.modal", function () {
            //clearFields();
        });
    }
});

Array.prototype.slice.call(forms).forEach(function (form) {
    form.addEventListener('submit', function (event) {
        if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        } else {
            event.preventDefault();

            //document.getElementById("close-modal").click();
            //Swal.fire({
            //    position: 'center',
            //    icon: 'success',
            //    title: 'Customer inserted successfully!',
            //    showConfirmButton: false,
            //    timer: 2000,
            //    showCloseButton: true
            //});
        }
    }, false)
})


//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#scroll-vertical', {
//      "scrollY":        "210px",
//      "scrollCollapse": true,
//      "paging":         false
//    });
    
//});

//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#scroll-horizontal', {
//      "scrollX": true
//    });
//});

//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#alternative-pagination', {
//      "pagingType": "full_numbers"
//    });
//});

//$(document).ready(function() {
//    var t = $('#add-rows').DataTable();
//    var counter = 1;
 
//    $('#addRow').on( 'click', function () {
//        t.row.add( [
//            counter +'.1',
//            counter +'.2',
//            counter +'.3',
//            counter +'.4',
//            counter +'.5',
//            counter +'.6',
//            counter +'.7',
//            counter +'.8',
//            counter +'.9',
//            counter +'.10',
//            counter +'.11',
//            counter +'.12'
//        ] ).draw( false );
 
//        counter++;
//    } );
 
//    // Automatically add a first row of data
//    $('#addRow').click();
//});


//$(document).ready(function() {
//    $('#example').DataTable();
//});

////fixed header
//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#fixed-header', {
//      "fixedHeader": true
//    });
    
//}); 

////modal data datables
//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#model-datatables', {
//      responsive: {
//            details: {
//                display: $.fn.dataTable.Responsive.display.modal( {
//                    header: function ( row ) {
//                        var data = row.data();
//                        return 'Details for '+data[0]+' '+data[1];
//                    }
//                } ),
//                renderer: $.fn.dataTable.Responsive.renderer.tableAll( {
//                    tableClass: 'table'
//                } )
//            }
//        }
//    });
    
//}); 

////buttons exmples
//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#buttons-datatables', {
//        dom: 'Bfrtip',
//        buttons: [
//            'copy', 'csv', 'excel', 'print', 'pdf'
//        ]
//    });
//}); 

////buttons exmples
//document.addEventListener('DOMContentLoaded', function () {
//  let table = new DataTable('#ajax-datatables', {
//        "ajax": 'assets/json/datatable.json'
//    });
//}); 