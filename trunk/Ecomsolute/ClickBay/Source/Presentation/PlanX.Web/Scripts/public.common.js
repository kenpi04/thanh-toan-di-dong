/*
** nopCommerce custom js functions
*/
$(document).ready(function (e) {
    $(".load-ajax").each(function (index, item) {
        var url = $(item).data("url");
        if (url && url.length > 0) {
            $(item).html("<div class=loading-img></div>").load(url);
        }
    });
})

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
    container.dialog({ modal: isModal });
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
        .mouseenter(function () {
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
$(document).ready(function () {


})
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}


function eraseCookie(name) {
    createCookie(name, "", -1);
}

