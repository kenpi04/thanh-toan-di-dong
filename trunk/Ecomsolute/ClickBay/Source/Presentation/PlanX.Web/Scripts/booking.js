$("a[data-ng-click=click]").click(function (event) {        
    var divName = $(this).attr("data-name");
    var divParent = $(this).parents("article");
    var div = divParent.find("div[class^=" + divName + "]");
    if (div.is(":hidden")) {
        div.css("display","table");
        divParent.find("#label-" + divName).find("i").attr("class", "fa fa-minus-square-o fa-lg");
    } else {
        div.hide();
        divParent.find("#label-" + divName).find("i").attr("class", "fa fa-plus-square-o fa-lg");
    }
});