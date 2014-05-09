<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vpc_queryDR.aspx.cs" Inherits="WebApplication2.vpc_queryDR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>

<body>
    <table width='100%' border='2' cellpadding='2' bgcolor='#0074C4'>
        <tr>
            <td bgcolor='#CED7EF' width='90%'>
                <h2 class='co'>
                    &nbsp;Virtual Payment Client - Version 1</h2>
            </td>
            <td bgcolor='#0074C4' align='center'>
                <h3 class='co'>
                    ONEPAY</h3>
            </td>
        </tr>
    </table>
    <center>
        <h2>
            <br />
            ASP.net QueryDR Example</h2>
        <form id="form1" runat="server">
        <table>
            <tr bgcolor="#CED7EF">
                <td width="1%">
                    &nbsp;
                </td>
                <td width="40%">
                    <b><i>Virtual Payment Client URL:&nbsp;</i></b>
                </td>
                <td width="55%">
                    <asp:TextBox ID="virtualPaymentClientURL" runat="server" Width="400px" Text="https://migs.mastercard.com.au/vpcdps" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;<hr width="75%">
                    &nbsp;
                </td>
            </tr>
            <tr bgcolor="#0074C4">
                <td colspan="3" height="25">
                    <p>
                        <b>&nbsp;Basic QueryDR Transaction Fields</b></p>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>VPC Version: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_Version" runat="server" Width="200px">1</asp:TextBox>
                </td>
            </tr>
            <tr bgcolor="#CED7EF">
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>Command Type: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_Command" runat="server" Width="200px">queryDR</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>Merchant AccessCode: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_AccessCode" runat="server" Width="200px" Text="614240F4"></asp:TextBox>
                </td>
            </tr>
            <tr bgcolor="#CED7EF">
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>MerchantID: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_Merchant" runat="server" Width="200px" Text="TESTONEPAYUSD"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>Search Merchant Transaction Reference: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_MerchTxnRef" runat="server" Width="200px" Text=""></asp:TextBox>
                </td>
            </tr>
            <tr bgcolor="#CED7EF">
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>User: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_User" runat="server" Width="200px" Text="op01"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <b><i>Password: </i></b>
                </td>
                <td>
                    <asp:TextBox ID="vpc_Password" runat="server" Width="200px" Text="op123456"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="SubButL" runat="server" Text="Search" OnClick="SubButL_Click" />
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>