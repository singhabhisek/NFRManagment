
/* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
var dropdown = document.getElementsByClassName("dropdown-btn");
var i;

for (i = 0; i < dropdown.length; i++) {
    dropdown[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var dropdownContent = this.nextElementSibling;
        if (dropdownContent.style.display === "block") {
            dropdownContent.style.display = "none";
        } else {
            dropdownContent.style.display = "block";
        }
    });
}




/*Confirmation Delete*/
$('#dialog-delete').dialog({
    resizable: false,
    height: 200,
    autoOpen: false,
    modal: true,
    buttons: {
        "Delete": function () {
            __doPostBack("ctl00$ContentPlaceHolder1$GridView1", "");
            $(this).dialog("close");
        },
        Cancel: function () {
            $(this).dialog("close");
        }
    }
});


