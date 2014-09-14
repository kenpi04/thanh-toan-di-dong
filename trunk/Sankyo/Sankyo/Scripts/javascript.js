function callfrm(dest){
	document.frmConsole.target = "_self"
	document.frmConsole.action = dest
	document.frmConsole.method = "POST"
	document.frmConsole.submit()
}	

function callfrm2(dest){
	document.frmConsole.target = "_blank"
	document.frmConsole.action = dest
	document.frmConsole.method = "POST"
	document.frmConsole.submit()
}	

function openNew(dest,popname,width,height){
	var popup=window.open(dest,popname,"left="+ (((800-width)/2)) + ",top=" + (((600-height)/2)) +",width="+width+",height="+height+",resizable=no,menubar=0,scrollbars=yes,location=no,alwaysRaised=yes");
	popup.focus();

}

function openNewPrint(dest,popname,width,height){
	popup=window.open(dest,popname,"left="+ (((800-width)/2)) + ",top=" + (((600-height)/2)) +",width="+width+",height="+height+",toolbar=no,resizable=no,menubar=1,scrollbars=yes,location=no,alwaysRaised=yes");
	popup.focus();
}

function changerow(row){
	for(i=0;i<15;i++){
		if(i<row){
			jQuery("#tr_upload"+i).show();
		}else{
			jQuery("#tr_upload"+i).hide();
		}
	} 
}

function checkboxNum(chk) {
	var total = 0;
	var max = eval('document.frmConsole.' + chk + '.length');
	for (var idx = 0; idx < max; idx++) {
		if (eval('document.frmConsole.'+ chk +'[' + idx + '].checked') == true) {
			total += 1;
		}
	}
	return total;
}

function DigitOnly() {
	if ((event.keyCode>=96)&&(event.keyCode<=105))		
		return;
	if ((event.keyCode>=48)&&(event.keyCode<=57))	
		return;
	if (event.keyCode==8 || event.keyCode==9)
		return;
	if ((event.keyCode==37)||(event.keyCode==39)||(event.keyCode==46))
		return;
	if ((event.keyCode==110)||(event.keyCode==190))
		return;
	event.returnValue=false;	
}

function DigitOnly2(e,obj) {
	var keyCode = e.keyCode || e.which;
	var keyChar = String.fromCharCode(keyCode);
	var CharShow="";
	var isThai=false;

	if (keyChar == "1" || keyChar == "2" || keyChar == "3" || keyChar == "4" || keyChar == "5" || keyChar == "6" || keyChar== "7" || keyChar == "8" 
		|| keyChar == "9" || keyChar == "0" || keyCode==8 || keyCode==9){
		return true;
	}else{
		for(i=0;i<obj.value.length;i++){
			if (obj.value.charAt(i) == "1" || obj.value.charAt(i) == "2" || obj.value.charAt(i) == "3" || obj.value.charAt(i) == "4" 
			|| obj.value.charAt(i) == "5" || obj.value.charAt(i) == "6" || obj.value.charAt(i) == "7" || obj.value.charAt(i) == "8" 
			|| obj.value.charAt(i) == "9" || obj.value.charAt(i) == "0"){		
				CharShow += obj.value.charAt(i);
			}
		}

		obj.value = CharShow;
		return false;
	}
}

function hasWhiteSpace(s) 
{
	 reWhiteSpace = new RegExp(/\s/); 
	 if (reWhiteSpace.test(s)) {
		  return false;
	 }
	return true;
}

function EngOnly(){
	if (String.fromCharCode(event.keyCode) < '~'){
		return;
	}
	event.returnValue=false;	
}

function EngOnly2(){
	var keyblock = new Array(33,64,35,36,37,94,38,42,40,41,95,43,61,92,123,125,91,93,58,59,34,39,60,62,44,46,63,47);
	var cChr;
	cChr = window.event.keyCode;
//	alert(cChr);

	for(var i=0 ; i<keyblock.length ; i++){
		if(cChr == keyblock[i]){
			event.returnValue=false;	
		}
	}

	if (String.fromCharCode(event.keyCode) < '~'){
		return;
	}
	event.returnValue=false;	
}

function EngOnly4(e,obj){
	var keyCode = e.keyCode || e.which;
	var keyChar = String.fromCharCode(keyCode);
	var CharShow="";
	var isThai=false;

	if(keyChar >= '~'){
		return false;
	}

	for(i=0;i<obj.value.length;i++){
		if(obj.value.charAt(i) >= '~') {
			isThai = true;
		}else{
			CharShow += obj.value.charAt(i);
		}
	}

	obj.value = CharShow;
	if(isThai == true){
		return false;
	}else{
		return true;	
	}
}

function CloseWindows(){
	window.close();
}

function CheckEnter(callFunction,valueFunction){
	var cChr;
	cChr = window.event.keyCode;
	if (cChr == 13)
	{
		callFunction(valueFunction);
	}
}

function OnMouseOver(obj,color){
	obj.style.background=color;
}

function noRightClick(){
	 if(event.button==2){
		 document.oncontextmenu =function(){
			 return false;
		  }
	 }
}

function Trim(TRIM_VALUE){
	if(TRIM_VALUE.length < 1){
	  return"";
	}
	TRIM_VALUE = RTrim(TRIM_VALUE);
	TRIM_VALUE = LTrim(TRIM_VALUE);
	if(TRIM_VALUE==""){
	  return "";
	}
	else{
	  return TRIM_VALUE;
	}
}

function RTrim(VALUE){
	var w_space = String.fromCharCode(32);
	var v_length = VALUE.length;
	var strTemp = "";
	if(v_length < 0){
	  return"";
	}
	var iTemp = v_length -1;

	while(iTemp > -1){
	  if(VALUE.charAt(iTemp) == w_space){
	  }
	  else{
		strTemp = VALUE.substring(0,iTemp +1);
		break;
			 }
	  iTemp = iTemp-1;

	} //End While
		   return strTemp;

} //End Function

function LTrim(VALUE){
	var w_space = String.fromCharCode(32);
	if(v_length < 1){
	return"";
	}
	var v_length = VALUE.length;
	var strTemp = "";

	var iTemp = 0;

	while(iTemp < v_length){
	  if(VALUE.charAt(iTemp) == w_space){
	  }
	  else{
		strTemp = VALUE.substring(iTemp,v_length);
		break;
	  }
	  iTemp = iTemp + 1;
	} //End While
	return strTemp;
} //End Function

function EngOnly3(e){
	var isThai=false;
	var keyCode = e.keyCode || e.which;
	var keyChar = String.fromCharCode(keyCode);
	var keyblock = new Array(33,64,35,36,37,94,38,42,40,41,95,43,61,92,123,125,91,93,58,59,34,39,60,62,44,46,63,47);
	var CharShow = "";

	for(var k=0 ; k<keyblock.length ; k++){
		if(keyCode == keyblock[k]){
			return false;
		}
	}
	if(keyChar >= '~'){
		return false;
	}

	for(i=0 ; i < document.frmConsole.d_name_new.value.length ; i++){
		if(document.frmConsole.d_name_new.value.charAt(i) >= '~') {
			isThai = true;
		}else{
			CharShow += document.frmConsole.d_name_new.value.charAt(i);
		}
	}

	document.frmConsole.d_name_new.value = CharShow;
	if(isThai == true){
		return false;
	}else{
		return true;	
	}
}

function CheckEnterNew(callFunction,valueFunction,e){
	var cChr;
	cChr = e.keyCode || e.which;
	if (cChr == 13)
	{
		callFunction(e,valueFunction);
	}
}
function noRightClick(){
	if(event.button==2){
		document.oncontextmenu =function(){
			return false;
		}
	}
}

function checkIE(){
		var ie = (navigator.appName.indexOf("Microsoft") != -1);if(ie) return true;return false;
}

function CheckDigitOnly(obj){
		var strOutput	= obj.value;
		var lengtStr	= obj.value.length;
		for(var i=0;i<lengtStr;i++){
			if(strOutput.charAt(i) >='0' && strOutput.charAt(i) <='9'){

			}else{
				obj.value="";
				return 0;
			}
		}
}

function formatNumber(num, decplaces) {
	num = parseFloat(num);
	if (!isNaN(num)) {
		var str = "" + Math.round (eval(num) * Math.pow(10,decplaces));
		if (str.indexOf("e") != -1) {
			return "Out of Range";
		}
		while (str.length <= decplaces) {
			str = "0" + str;
		}

		var decpoint = str.length - decplaces;
		var tmpNum = str.substring(0,decpoint);
		//---------------Add Commas--------------------------
		var numRet = tmpNum.toString();
		var re = /(-?\d+)(\d{3})/;
		while (re.test(numRet)) {
			numRet = numRet.replace(re, "$1,$2");
		}
		return numRet + "." + str.substring(decpoint,str.length);
	//	return numRet;
	} else {
		return "0.00";
	}
} 

function roll_over(img_name, img_src){
	document[img_name].src = img_src;
}

function mouseOverChangImage(obj , image){
	obj.src = image;
}

function mouseOverChangBackground(obj , color){
	obj.style.background = color;
}

function changeColorDialog(obj , spanId){
	var len_txt = obj.value.length;
	if(obj.value.search(/^#[a-fA-F0-9]{6}$/) != -1){
		document.getElementById(spanId).style.backgroundColor = obj.value;
	}else{
		alert("ขออภัยรูปแบบไม่ถูกต้อง");
		obj.value = "#FFFFFF";
		document.getElementById(spanId).style.backgroundColor = obj.value;
		document.getElementById(spanId).focus();
	}
}//end function changeColorDialog

function checkDigitJQuery(objectJQuery , StringExcep , StringType){
	if (StringExcep == "+")
	{
		var intRegex = /^[+]?\d+$/; // /^[-+]?\d+[,]?\d+$/;
	}else if (StringExcep == "-")
	{
		var intRegex = /^[-]?\d+$/;
	}else if (StringExcep == "+-" || StringExcep == "-+")
	{
		var intRegex = /^[+-]?\d+$/;
	}else{
		var intRegex = /^\d+$/;
	}
	
	if(StringType == "integer"){
		var str = objectJQuery.val();
		if(intRegex.test(str)){
			return true;
		}else{
			return false;
		}
	}else if(StringType == "float"){
		var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;
		var str = objectJQuery.val();
		if(intRegex.test(str) || floatRegex.test(str)){
			return true;
		}else{
			return false;
		}
	}
}//end function checkDigitOnly

/** START Equal Height Columns with jQuery **/
	function equalHeight(group) {
		tallest = 0;
		jQuery(group).each(function() {
			thisHeight = jQuery(this).height();
			if(thisHeight > tallest) {
				tallest = parseInt(thisHeight);
			}else{
				tallest = tallest;
			}
		});
		
		jQuery(group).height(tallest);
		//alert(tallest);
	}	
	/** END Equal Height Columns with jQuery **/

	 function cheackFileSize(name,maxsize){
		try { var picsize =  jQuery("#"+name)[0].files[0].size; } catch (e) { var picsize = "undified"; }
		if(picsize=="undified"){	
			return true;
		}else{
			if(picsize<=maxsize){	return true;		}
			else{							return false;		}
		}
	 }

	function checkFloat(obj){
		var strOutput	= obj.value;
		var lengtStr	= obj.value.length;
		for(var i=0;i<lengtStr;i++){
			if((strOutput.charAt(i) >='0' && strOutput.charAt(i) <='9') || (strOutput>=0)){

			}else{
				obj.value="";
				return 0;
			}
		}
	}

	function checkNumber(elem){
		var numericExpression = /^[0-9]+$/;
        if(elem.value.match(numericExpression) && ( elem.value*1 >= 0) ){
                return true;
        }else{
                elem.value=elem.value.substr(0,elem.value.length-1);
                elem.focus();
                return false;
        }
	}

function dialogImagesSilde(rid,pid,typename){
	var currenturl		= jQuery(location).attr('href');
	arrurl				= currenturl.split('/');
	
	jQuery("#dialoggalleria").dialog({
		dialogClass		: "dialog-fixed",
		draggable			: false,
		bgiframe			: false,
		height				: parseInt(660),
		width					: parseInt(807),
		modal				: true,
		closeOnEscape	: true,								//need to take care of cancel_callback()
		autoOpen			: false,
		resizable			: false,								//close: function(event, ui) { jQuery("#show_lightbox").remove(); }
		position				: ['center',100],
		open: function() {	
			jQuery(".ui-widget-header").css({"background":"none", "border":"none" ,"line-height":"2px"}); 
			jQuery(".ui-dialog-content").css({"padding":"0.5em 0.2em"});
			jQuery("#dialoggalleria").load("http://"+arrurl[2]+"/include/box_galleria/galleria.php?typeid="+typename+"&keyid="+rid+"&position="+pid);	
		}
	});
	jQuery('#dialoggalleria').html("");
	jQuery('#dialoggalleria').dialog('open');
}