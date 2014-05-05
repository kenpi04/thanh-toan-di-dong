<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Checkout Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Click on the PayNow button to complete the payment on Payoo.<br />
        <br />
        <table width='700px' border='1' cellspacing='0'><thead><tr><td width='40%' align='center'>Tên hàng</td><td width='20%' align='center'>Đơn giá</td><td 
width='15%' align='center'>Số lượng</td><td width='25%' align='center'>Thành tiền (VNĐ)</td></tr></thead><tbody><tr><td align='left'>HP Pavilion DV3-3502TX</td><td 
align='right'>23,109,210</td><td align='center'>1</td><td align='right'>23,109,210</td></tr><tr><td align='left'>FAN Notebook (B4)</td><td align='right'>266,850</td><td 
align='center'>1</td><td align='right'>266,850</td></tr><tr><td align='right' colspan='3'>Phí vận chuyển</td><td align='right'>10,000</td></tr><tr><td align='right' colspan='3'>Tổng tiền</td><td align='right'>23,386,060</td></tr><tr><td 
align='left' colspan='4'>Some notes for the order</td></tr></tbody></table>
        <br />
        
        <asp:ImageButton ID="btnPaynow" runat="server" Height="32px" ImageUrl="https://www.payoo.com.vn/img/button/PayNow.jpg"
            OnClick="btnPaynow_Click" Width="90px" />&nbsp;<br />
        <br />
        <a href='index.htm'>Home</a>
    </div>
    </form>
</body>
</html>
