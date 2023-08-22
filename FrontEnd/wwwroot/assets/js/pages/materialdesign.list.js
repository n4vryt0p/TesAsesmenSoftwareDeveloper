/*
Template Name: Velzon - Admin & Dashboard Template
Author: Themesbrand
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Material design Init Js File
*/

// icons
function isNew(icon) {
    return icon.version === '6.5.95';
}
function isDeprecated(icon) {
    return typeof icon.deprecated == 'undefined'
        ? false
        : icon.deprecated;
}
function getIconItem(icon, isNewIcon) {
    var div = document.createElement('div'),
        i = document.createElement('i');
        div.className = "col-xl-3 col-lg-4 col-sm-6";
        i.className = 'mdi mdi-' + icon.name,
        span = document.createElement('span');
    div.appendChild(i);
    span.appendChild(document.createTextNode('mdi-' + icon.name));
    div.appendChild(span);
    return div;
}