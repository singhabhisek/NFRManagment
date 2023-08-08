
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


/*Close Modal Pop Up on outside*/
function pageLoad() {
    var modalPopup = $find('mpe');
    modalPopup.add_shown(function () {
        modalPopup._backgroundElement.addEventListener("click", function () {
            modalPopup.hide();
        });
    });
};


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

/*loader*/
function ShowProgress() {
    setTimeout(function () {
        var modal = $('<div />');
        modal.addClass("modal");
        $('body').append(modal);
        var loading = $(".loading");
        loading.show();
        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
        loading.css({ top: top, left: left });
    }, 200);
}
