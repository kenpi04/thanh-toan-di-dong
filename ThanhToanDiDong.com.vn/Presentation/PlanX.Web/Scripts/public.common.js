/*
** nopCommerce custom js functions
*/
$(document).ready(function () {
    // browser window scroll (in pixels) after which the "back to top" link is shown
    var offset = 300,
		//browser window scroll (in pixels) after which the "back to top" link opacity is reduced
		offset_opacity = 1200,
		//duration of the top scrolling animation (in ms)
		scroll_top_duration = 700,
		//grab the "back to top" link
		$back_to_top = $('.cd-top');

    //hide or show the "back to top" link
    $(window).scroll(function () {
        ($(this).scrollTop() > offset) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible cd-fade-out');
        if ($(this).scrollTop() > offset_opacity) {
            $back_to_top.addClass('cd-fade-out');
        }
    });

    //smooth scroll to top
    $back_to_top.on('click', function (event) {
        event.preventDefault();
        $('body,html').animate({
            scrollTop: 0,
        }, scroll_top_duration
		);
    });

    $('#newsletter-subscribe-button').click(function () {
        var email = $("#newsletter-email").val();
        var $this = $(this);
        $this.val("Gửi...");
        $.ajax({
            cache: false,
            type: "POST",
            url: "/subscribenewsletter",
            data: { "email": email },
        success: function (data) {
            $this.val("Gửi");                        
            $("#newsletter-result-block").html(data.Result);
            if (data.Success) {
                $('#newsletter-subscribe-block').hide();
                $('#newsletter-result-block').show();
                $("#newsletter-email").val("");
            }
            else {
                $('#newsletter-result-block').fadeIn("slow").delay(2000).fadeOut("slow");
            }
        },
        error:function (xhr, ajaxOptions, thrownError){
            alert('Failed to subscribe.');
            $this.val("Gửi");
        }
    });
    return false;
    });

    $(".load-ajax").each(function (index, item) {
        var url = $(item).data("url");
        if (url && url.length > 0) {
            $(item).html("<div class=loading-img></div>").load(url).fadeIn(600);
            $(item).removeAttr("data-url");
        }
    })
})
//show sub breadcumb
function ShowSub(id) {
    var sub = $("li[id=" + id + "]").find("ul");
    if (sub.css("display") == "none") {
        sub.fadeIn(400);
    }
    else { sub.fadeOut(400); }
};

function imageResize(id) {
    var margin = '';
    var marginValue;
    var contW, contH, img, imgW, imgH, ratio, ratioDiv;
    var path = $("#" + id);
    // variables: get width & height of div view
    contW = path.width();
    contH = path.height();
    ratioDiv = (contW / contH).toFixed(2);
    //get width & height real Image
    img = path.find("img");
    imgW = img.width();
    imgH = img.height();
    ratio = (imgW / imgH).toFixed(2);
    //
    if (ratio == ratioDiv)//w = h
    {
        imgH = contH; imgW = contW;
    }

    if (ratioDiv > ratio) {
        imgW = contW;
        imgH = (imgW / ratio).toFixed(2);
        margin = 'margin-top';
        marginValue = ((contH - imgH) / 2).toFixed(2);
    }
    else if (ratioDiv < ratio) {
        imgH = contH;
        imgW = (contH * ratio).toFixed(2);
        margin = 'margin-left';
        marginValue = ((contW - imgW) / 2).toFixed(2);
    }
    if (imgW != 0 || imgH != 0) {
        img.css("width", imgW + "px ");
        img.css("height", imgH + "px ");
        img.css(margin, marginValue + "px ");
    }
}

function OpenWindow(query, w, h, scroll) {
    var l = (screen.width - w) / 2;
    var t = (screen.height - h) / 2;

    winprops = 'resizable=0, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
    if (scroll) winprops += ',scrollbars=1';
    var f = window.open(query, "_blank", winprops);
}

function setLocation(url) {
    window.location.href = url;
}

function displayAjaxLoading(display) {
    if (display) {
        $('.ajax-loading-block-window').show();
    }
    else {
        $('.ajax-loading-block-window').hide('slow');
    }
}

function displayPopupNotification(message, messagetype, modal) {
    //types: success, error
    var container;
    if (messagetype == 'success') {
        //success
        container = $('#dialog-notifications-success');
    }
    else if (messagetype == 'error') {
        //error
        container = $('#dialog-notifications-error');
    }
    else {
        //other
        container = $('#dialog-notifications-success');
    }

    //we do not encode displayed message
    var htmlcode = '';
    if ((typeof message) == 'string') {
        htmlcode = '<p>' + message + '</p>';
    } else {
        for (var i = 0; i < message.length; i++) {
            htmlcode = htmlcode + '<p>' + message[i] + '</p>';
        }
    }

    container.html(htmlcode);

    var isModal = (modal ? true : false);
    container.dialog({modal:isModal});
}


var barNotificationTimeout;
function displayBarNotification(message, messagetype, timeout) {
    clearTimeout(barNotificationTimeout);

    //types: success, error
    var cssclass = 'success';
    if (messagetype == 'success') {
        cssclass = 'success';
    }
    else if (messagetype == 'error') {
        cssclass = 'error';
    }
    //remove previous CSS classes and notifications
    $('#bar-notification')
        .removeClass('success')
        .removeClass('error');
    $('#bar-notification .content').remove();

    //we do not encode displayed message

    //add new notifications
    var htmlcode = '';
    if ((typeof message) == 'string') {
        htmlcode = '<p class="content">' + message + '</p>';
    } else {
        for (var i = 0; i < message.length; i++) {
            htmlcode = htmlcode + '<p class="content">' + message[i] + '</p>';
        }
    }
    $('#bar-notification').append(htmlcode)
        .addClass(cssclass)
        .fadeIn('slow')
        .mouseenter(function ()
            {
                clearTimeout(barNotificationTimeout);
            });

    $('#bar-notification .close').unbind('click').click(function () {
        $('#bar-notification').fadeOut('slow');
    });

    //timeout (if set)
    if (timeout > 0) {
        barNotificationTimeout = setTimeout(function () {
            $('#bar-notification').fadeOut('slow');
        }, timeout);
    }
}

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}