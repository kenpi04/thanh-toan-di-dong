//set language vietnam
$.datepicker.regional["vi-VN"] =
	{
	    closeText: "Đóng",
	    prevText: "Trước",
	    nextText: "Sau",
	    currentText: "Hôm nay",
	    monthNames: ["Tháng một", "Tháng hai", "Tháng ba", "Tháng tư", "Tháng năm", "Tháng sáu", "Tháng bảy", "Tháng tám", "Tháng chín", "Tháng mười", "Tháng mười một", "Tháng mười hai"],
	    monthNamesShort: ["Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười", "Mười một", "Mười hai"],
	    dayNames: ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"],
	    dayNamesShort: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy"],
	    dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
	    weekHeader: "Tuần",
	    dateFormat: "dd/mm/yy",
	    firstDay: 1,
	    isRTL: false,
	    showMonthAfterYear: false,
	    yearSuffix: ""
	};
$.datepicker.setDefaults($.datepicker.regional["vi-VN"]);


var CURRENT_TXT = null, ISSHOW = false;
$("input[name=Return]").change(function () {
    if ($(this).val() == 'true') {
        $("#return-div").show('slow')
    }
    else
        $("#return-div").hide('slow');
})
$("#From").datepicker({
    defaultDate: "+1w",
    dateFormat: "dd/mm/yy",
    minDate: new Date(),
    changeMonth: false,
    numberOfMonths: 1,
    onClose: function (selectedDate) {
        if ($("input[name=Return]:checked").val() == 'true')
            $("#To").datepicker("option", "minDate", selectedDate);
    }
});
$("#To").datepicker({
    defaultDate: "+1w",
    dateFormat: "dd/mm/yy",
    changeMonth: false,
    numberOfMonths: 1,
    onClose: function (selectedDate) {
        $("#From").datepicker("option", "maxDate", selectedDate);
    }
});

function openPopup(name) {
    $("#title-text-id").contents().remove();
    if (name != 'toPlace')
    {        
        $("#title-text-id").append("Chọn điểm đi");
      
    }
    else {
        $("#title-text-id").append("Chọn điểm đến");
      
    }
    $("#dialog-form").show('slow');

}
//function openPopup(top) {

//    $("#dialog-form").css({ top: top }).show();


//}
$("#frmSearch").submit(function () {
    if ($("#From").val() == "") {
        alert("Vui lòng chọn ngày đi.");
        return false;
    } else
        if ($("input[name=Return]:checked").val() == 'true' && $("#To").val() == "") {
            alert("Vui lòng chọn ngày về.");
            return false;
        }
        else if ($("#Adult").val() == "0" && $("#Child").val() == "0") {
            alert("Vui lòng chọn số lượng người đi.");
            return false;
        }
        else if ($("#FromId").val() == $("#ToId").val()) {
            alert("Nơi đi và nơi đến phải khác nhau.");
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
    $('#dialog-form').hide('slow');
}


$(function () {
    $("a[airportcode]").click(function () {
        var name = $(this).text();
        var code = $(this).attr("airportcode");
        selectedValue(name, code)
    })
    function selectedValue(name, code) {
        //if ($("#ToId").val() == $("#FromId").val()) {
        //    alert("Điểm đến phải khác điểm đi");
        //    return;
        //}

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

