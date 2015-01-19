$(document).ready(function () { 
	$("select[name=Adult_menu]").val('1');
	$("select[id=FromId_menu]").val('SGN');
	$("select[id=ToId_menu]").val('HAN');
})

$(function () {

	$("#btnSearchMenu").click(function (e) {
		var error = true;
		var frmSearch = $(this).parents($("#formSearch-menu"));
		var regexp = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
		var roundtrip = false;//frmSearch.find($("input[name=Return_menu]:checked")).val();
		var fromIdMessage = frmSearch.find($("#FromId_menu_message"));
		var toIdMessage = frmSearch.find($("#ToId_menu_message"));
		var fromdateMessage = frmSearch.find($("#DepartDate_menu_message"));
		var todateMessage = frmSearch.find($("#ReturnDate_menu_message"));
		var passengerMessage = frmSearch.find($("#passenger_menu_message"));

		var fromId = frmSearch.find("#FromId_menu").val();
		if (fromId == '') {
			fromIdMessage.contents.remove();
			fromIdMessage.append("Vui lòng chọn điểm đi.");
			error = false;
		}
		var toId = frmSearch.find("#ToId_menu").val();
		if (fromId == '') {
			toIdMessage.contents.remove();
			toIdMessage.append("Vui lòng chọn điểm đến.");
			error = false;
		}
		var fromDate = frmSearch.find("#DepartDate_menu").val();
		if (fromDate == '') {
			fromdateMessage.contents().remove();
			fromdateMessage.append("Vui lòng nhập ngày đi.");
			error = false;
		} else {
			if (!regexp.test(fromDate)) {
				fromdateMessage.contents().remove();
				fromdateMessage.append("Ngày đi không đúng định dạng.");
				error = false;
			}
		}

		var toDate = frmSearch.find("#ReturnDate_menu").val();
		if (toDate != '') {
			roundtrip = true;
			if (!regexp.test(toDate)) {
				todateMessage.contents().remove();
				todateMessage.append("Ngày về không đúng định dạng.");
				error = false;
			}
		}
		else { roundtrip = false; }

		var adult = frmSearch.find("#Adult_menu").val();
		var child = frmSearch.find("#Child_menu").val();
		var ifant = frmSearch.find("#Flant_menu").val();
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