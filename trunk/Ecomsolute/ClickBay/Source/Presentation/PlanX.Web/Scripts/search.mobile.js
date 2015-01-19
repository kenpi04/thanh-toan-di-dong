
$(document).ready(function () {
	$("select[name=Adult_m]").val('1');
	$("select[id=FromId_m]").val('SGN');
	$("select[id=ToId_m]").val('HAN');

	$("input[id=DepartDate_m]").each(function () {
		var pr = $(this).parents("#frmSearch-mobile");

		$(this).datepicker({
			dateFormat: "dd/mm/yy",
			minDate: new Date(),
			changeMonth: false,
			numberOfMonths: 1,
			onClose: function (selectedDate) {
				if (pr.find($("input[name=Return_m]:checked")).val() == 'true')
					pr.find($("#ReturnDate_m")).datepicker("option", "minDate", selectedDate);
			}
		});
	});
	$("input[id=ReturnDate_m]").each(function () {
		$(this).datepicker({
			dateFormat: "dd/mm/yy",
			changeMonth: false,
			numberOfMonths: 1,
		});
	});
})

$(function () {

	$("input[name=Return_m]").change(function () {
		if ($(this).val() == 'true') {
			$("#frmSearch-mobile").find("#return-div-m").show('slow');
		}
		else
			$("#frmSearch-mobile").find("#return-div-m").hide('slow');
	})

	$("#btnSearchMobile").click(function (e) {
		var error = true;
		var frmSearch = $(this).parents($("#frmSearch-mobile"));

		var roundtrip = frmSearch.find($("input[name=Return_m]:checked")).val();
		var fromIdMessage = frmSearch.find($("#FromId_m_message"));
		var toIdMessage = frmSearch.find($("#ToId_m_message"));
		var fromdateMessage = frmSearch.find($("#DepartDate_m_message"));
		var todateMessage = frmSearch.find($("#ReturnDate_m_message"));
		var passengerMessage = frmSearch.find($("#passenger_m_message"));

		var fromId = frmSearch.find("#FromId_m").val();
		if (fromId == '') {
			fromIdMessage.contents.remove();
			fromIdMessage.append("Vui lòng chọn điểm đi.");
			error = false;
		}
		var toId = frmSearch.find("#ToId_m").val();
		if (fromId == '') {
			toIdMessage.contents.remove();
			toIdMessage.append("Vui lòng chọn điểm đến.");
			error = false;
		}
		var fromDate = frmSearch.find("#DepartDate_m").val();
		if (fromDate == '') {
			fromdateMessage.contents().remove();
			fromdateMessage.append("Vui lòng chọn ngày đi.");
			error = false;
		}
		var toDate = frmSearch.find("#ReturnDate_m").val();
		if (roundtrip == 'true' & toDate == '') {
			todateMessage.contents().remove();
			todateMessage.append("Vui lòng chọn ngày về.");
			error = false;
		}
		var adult = frmSearch.find("#Adult_m").val();
		var child = frmSearch.find("#Child_m").val();
		var ifant = frmSearch.find("#Flant_m").val();
		if (adult == 0 & child == 0 & ifant == 0) {
			passengerMessage.contents().remove();
			passengerMessage.append("Vui lòng chọn ít nhất 1 hành khách.");
			error = false;
		}
		if (error) {
			window.location = 'http://clickbay.com.vn/tim-ve?' + 'Return=' + roundtrip + '&FromId=' + fromId + '&ToId=' + toId + '&DepartDate=' + fromDate + '&ReturnDate=' + toDate + '&Adult=' + adult + '&Child=' + child + '&Flant=' + ifant;
		}
	});
});