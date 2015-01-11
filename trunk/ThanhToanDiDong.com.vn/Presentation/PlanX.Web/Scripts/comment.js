PAGESIZE = 5;
$(function () {
    $(document).on("click", "#page-comment input.default", function () {
        var page;
        if (!$(this).attr("page"))
            page = parseInt($(this).val());
        else
            page = parseInt($(this).attr("page"));
        loadComment(page,PAGESIZE)
       
       
    })
    loadComment(1, PAGESIZE);
        $(document).on("submit","form[name=frmComment]",function () {
            addComment($(this));
            return false;
        })
        $(document).on("keydown.autocomplete","input[name=Place]", function() {
          
            var el=$(this);
            $(this).autocomplete({
                delay: 500,
                minLength: 1,
                source: CITIES,
                select: function( event, ui ) {
                    el.val(ui.item.label);
                  
                    return false;
                }
            })
                    .data("ui-autocomplete")._renderItem = function( ul, item ) {
                        var t = item.label;
                        //html encode
                        t = htmlEncode(t);
                        return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + t + "</a>")
                    .appendTo(ul);
                    };
        });
    });
function addComment(frm)
{
    if(frm.valid())
        
    {
        $.ajax({
            type: "POST",
            url: "/News/NewsCommentAdd",
            data: frm.serialize(),
            success:function(d)
            {
                if(d=="UNENABLE")
                    window.location.href="/";
                else
                {
                    alert(d);
                    frm.find("input[type=text],textarea").val("");
                    loadComment(1,PAGESIZE);
                }
            },
            error: function (e)
            {
                alert("Lỗi ");
            }
        })
    }
    return false;
}
function viewMore(id)
{
    var span = $("#comment-d-" + id).find(".more");
    var a=$("#comment-d-"+id).find(".comment-cmd");
    if (span.is(":hidden"))
    {
            
        span.show();
        a.html("Thu lại")
    }
    else
    {
        a.html("Xem thêm...")
        span.hide();
    }
}
function ShowCommentReply(id) {
    var divParent=$("#comment-d-" + id);
    var showDiv = divParent.find($(".comment-form"));
    var div = $("div[id=comment-detail]");
    var form = div.find($(".comment-form"));
    if (form.length != 0) {
        form.each(function () {
            $(this).remove();
        });
    }
    if (showDiv.length == 0) {
        var frmComment=$("#frmAddComment").clone();
        frmComment.find("input[type=text],textarea").val("");
        if(divParent.attr("parent"))
        {
            frmComment.append("<input type='hidden' value='"+divParent.attr("parent")+"' name='ParentId'/>");
        }
        $("div[id=comment-d-" + id + "]").append(frmComment);
    }
};

function CommentLike(id) {
    var lt=$("#lt-"+id);
    $.ajax({
        type:"POST",
        url:"/News/CommentLike",
        data:{id:id},
        success:function(d){
            if(d!="NOT OK")
            {
                lt.hide()
                .html("[Thích "+d+"]")
                .show("slow");
            }
        }
    })
    return false;
};

    

function loadComment(pageindex,pageSize)
{
    $.get("/News/CommentList", {pageIndex:pageindex,pageSize:pageSize,newsId:17},function(d){
        $("#comment-detail").replaceWith(d);
    }).error(function(){
        alert("lỗi !")
    });
}
