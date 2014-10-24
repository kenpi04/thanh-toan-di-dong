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
                //else
                //    alert("cập nhật thành công!");
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
function showNote(id)
{
    $.get("/Admin/Home/Note/"+id, function (d) {
        $("#dialog").html(d);
        $("#dialog").dialog();
    })
}
function formatnumber(strvalue) {
    var num;
    num = strvalue.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    num = Math.floor(num / 100).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '.' +
       num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num);
}
