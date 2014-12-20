var CURRENT_TXT = null, ISSHOW = false;
$("input[name=Return]").change(function () {
    if ($(this).val() == 'true') {
        $("#return-div").show()
    }
    else
        $("#return-div").hide();
})
$("#From").datepicker({
    defaultDate: "+1w",
    dateFormat: "dd/mm/yy",
    minDate: new Date(),
    changeMonth: true,
    numberOfMonths: 3,
    onClose: function (selectedDate) {
        if ($("input[name=Return]:checked").val() == 'true')
            $("#To").datepicker("option", "minDate", selectedDate);
    }
});
$("#To").datepicker({
    defaultDate: "+1w",
    dateFormat: "dd/mm/yy",
    changeMonth: true,
    numberOfMonths: 3,
    onClose: function (selectedDate) {
        $("#From").datepicker("option", "maxDate", selectedDate);
    }
});
$("#fromPlace,#toPlace").click(function () {
    openPopup();
    CURRENT_TXT = $(this).attr("id");
})
function openPopup() {

    $("#dialog-form").show();

}
function openPopup(top) {

    $("#dialog-form").css({ top: top }).show();


}
$("#frmSearch").submit(function () {
    if ($("#From").val() == "") {
        alert("Vui lòng chọn ngày đi");
        return false;
    } else
        if ($("input[name=Return]:checked").val() == 'true' && $("#To").val() == "") {
            alert("Vui lòng chọn ngày về");
            return false;
        }
        else if ($("Adult").val() == "0" || $("Child").val() == "0") {
            alert("Vui lòng chọn số lượng người đi");
            return false;
        }
    return true;
});
$(document).on('click', function (e) {
    if (!$(event.target).closest('#dialog-form').length && event.target.id != 'toPlace' && event.target.id != 'fromPlace') {
        if ($('#dialog-form').is(":visible")) {
            hidePopup();

        }
    }
})
function hidePopup() {
    $('#dialog-form').hide()

}


$(function () {
    $("a[airportcode]").click(function () {
        var name = $(this).text();
        var code = $(this).attr("airportcode");
        selectedValue(name, code)
    })
    function selectedValue(name, code) {
        if ($("#ToId").val() == $("#FromId").val()) {
            alert("Điểm đến phải khác điểm đi");
            return;
        }

        if (CURRENT_TXT == "fromPlace") {
            $("#fromPlace").html(name);
            $("#FromId").val(code);
            hidePopup();
            $("#toPlace").click();
        }
        else {

            $("#toPlace").html(name);
            $("#ToId").val(code);
            hidePopup();

        }

    }
    $('#small-searchterms').autocomplete({
        delay: 500,
        minLength: 2,
        source: '@(Url.Action("GetCity"))',
        select: function (event, ui) {
            selectedValue(ui.item.Name, ui.item.Code);
            return false;
        }
    })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                var t = item.Name;
                //html encode
                t = htmlEncode(t);
                return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a>" + t + "</a>")
            .appendTo(ul);
            };
});

