function doHTTPRequest(url, parameters, callback) {
  var http_request = false;
  if (window.XMLHttpRequest) { // Mozilla, Safari,...
    http_request = new XMLHttpRequest();
    if(http_request.overrideMimeType) {
      http_request.overrideMimeType('text/xml');
    }
  } else if (window.ActiveXObject) { // IE
    try {
      http_request = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
          http_request = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) {}
    }
  }
  if (!http_request) {
    alert('Cannot create XMLHTTP instance');
    return false;
  }

  //http_request.onreadystatechange = callback;
  http_request.onreadystatechange = function () {
		if (http_request.readyState == 4) 
		if (http_request.status == 200) {
                        callback();
        } else {
            alert("There was a problem retrieving the XML data:\n" + req.statusText);
        }
  }

  http_request.open('POST', url, true);
  http_request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
  http_request.setRequestHeader("Content-length", parameters.length);
  http_request.setRequestHeader("Connection", "close");
  http_request.send(parameters);
  
  return http_request;
}

function parseSubscriber() {
	if (SubRequest.readyState == 4) {
		document.getElementById('serverResponse').innerHTML = SubRequest.responseText;
	}
}