function showPop(mes, redirect, url) {
    $("#spanInfo").html(mes);
    $("#divPopupInfo").slideDown(3000).slideUp(1000);
    $("#spanInfo").html("");
    if (redirect) {
        if (url && url != "")
            window.location.href = url;
        else
            window.location.reload();

    }

}
$(function () {
    $(".datepicker").datepicker();
    $('form').submit(function () {
        var form = $(this);
        $.ajax({
            type: form.attr("method"),
            url: form.attr("action"),
            data: form.serialize(),
            success: function (d) {
                if (form.attr("update"))
                    $("#" + form.attr("update")).html(d);
                else
                    showMes("cập nhật thành công!", true, null);
            }
        })
        return false;
    })
    $(document).on("click", ".pagination li a[href]", function () {
        var page = parseInt($(this).html());
        $("#PagingModel_CurrentPage").val(page);
        $('form').submit();
        return false;
    })
});