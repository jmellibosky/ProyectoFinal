/*!
* Start Bootstrap - Simple Sidebar v6.0.5 (https://startbootstrap.com/template/simple-sidebar)
* Copyright 2013-2022 Start Bootstrap
* Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-simple-sidebar/blob/master/LICENSE)
*/
// 
// Scripts
// 

var myDivs = document.querySelectorAll('.sidebarToggle'); // select all elements with class "sidebarToggle"
for (var i = 0; i < myDivs.length; i++) {
    var div = myDivs[i];
    if (div.style.display !== "none") {
        divx = div; // set divx as current div that is displaying
    }
}

//const button = document.getElementById("sidebarToggle");
//const divx = document.getElementById("sidebarAdmin");

/*button.addEventListener("click", function () {
    if (div.classList.contains("hiddenSidebar")) {
        div.classList.remove("hiddenSidebar");
        return false;
    } else {
        div.classList.add("hiddenSidebar");
        return false;
    }
});*/

function hideSidebar() {
    if (divx.classList.contains("hiddenSidebar")) {
        divx.classList.remove("hiddenSidebar");
        return false;
    } else {
        divx.classList.add("hiddenSidebar");
        return false;
    }
}