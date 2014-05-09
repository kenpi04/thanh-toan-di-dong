<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vpc_dr.aspx.cs" Inherits="WebApplication2.vpc_dr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Response Virtual Payment Client</title>
</head>

<body>
    <form id="form1" runat="server" method="post">
    <!-- Start Branding Table -->
    <table id="Table1" cellpadding="2" width="100%" bgcolor="#0074c4" border="2">
        <tr>
            <td width="50%" bgcolor="#ced7ef">
                <h2 class="co">
                    &nbsp;Virtual Payment Client Example</h2>
            </td>
            <td align="center" bgcolor="#0074c4">
                <h3 class="co">
                    OnePay</h3>
            </td>
        </tr>
    </table>
    <!-- End Branding Table -->
    <table id="Table2" cellpadding="5" width="85%" align="center" border="0">
        <tr bgcolor="#0074c4">
            <td colspan="2" height="25">
                <p>
                    <strong>&nbsp;Basic Transaction Fields</strong></p>
            </td>
        </tr>
        <tr align="center">
            <td colspan="2" width="45%">
                <asp:Label ID="vpc_Result" runat="server" Width="208px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" width="55%">
                <strong><i>VPC API Version: </i></strong>
            </td>
            <td width="45%">
                <asp:Label ID="vpc_Version" runat="server" Width="208px"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#ced7ef">
            <td align="right">
                <strong><i>Command: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_Command" runat="server" Width="216px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <strong><i>Merchant Transaction Reference: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_MerchantRef" runat="server" Width="224px"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#ced7ef">
            <td align="right">
                <strong><i>Merchant ID: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_MerchantID" runat="server" Width="200px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <strong><i>Order Information: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_OderInfor" runat="server" Width="208px"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#ced7ef">
            <td align="right">
                <strong><i>Amount: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_Amount" runat="server" Width="208px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <font color="#0074c4">Fields above are the request values returned.<br>
                    <hr>
                    Fields below are the response fields for a Standard Transaction.<br>
                </font>
            </td>
        </tr>
        <tr bgcolor="#ced7ef">
            <td align="right">
                <strong><i>VPC Transaction Response Code: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_ResponseCode" runat="server" Width="216px"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#ced7ef">
            <td align="right">
                <strong><i>Message: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_Message" runat="server" Width="224px"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#ced7ef">
            <td align="right">
                <strong><i>Transaction Number: </i></strong>
            </td>
            <td>
                <asp:Label ID="vpc_TransactionNo" runat="server" Width="224px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <strong><i>Hash Validated Correctly: </i></strong>
            </td>
            <td>
                <asp:Label ID="hashvalidate" runat="server" Width="216px"></asp:Label>
            </td>
        </tr>
    </table>
    <p align="center">
        <a href="http://localhost:1410/vpc_do.aspx">New Transaction</a></p>
    </form>
</body>
</html>