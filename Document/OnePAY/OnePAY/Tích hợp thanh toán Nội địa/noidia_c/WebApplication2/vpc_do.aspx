<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vpc_do.aspx.cs" Inherits="WebApplication2._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Virtual Payment Client</title>
</head>
<body>
    <table align="center">
        <tr>
            <td>
                <table style="width: 781px; height: 59px" width="781" border="2" bgcolor="#0099FF"
                    align="center">
                    <tr>
                        <td style="width: 473px" width="473">
                            <h2 class="co">
                                &nbsp;Virtual Payment Client
                            </h2>
                        </td>
                        <td align="center" bgcolor="#0074c4">
                            <h3 class="co">
                                OnePAY</h3>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <form id="form1" runat="server" enableviewstate="False">
                <table width="80%" align="center">
                    <tr>
                        <td colspan="23" height="25">
                            <p>
                                <b>&nbsp;Basic 3-Party Transaction Fields</b></p>
                        </td>
                    </tr>
                    <tr>
                        <td width="300">
                            <asp:Label ID="Label12" runat="server" Text="Vitual Payment Client"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="virtualPaymentClientURL" runat="server" ReadOnly="True" Width="250px">http://mtf.onepay.vn/onecomm-pay/vpc.op</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Version"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_Version" runat="server" ReadOnly="True">2</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Command Type"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_Command" runat="server" ReadOnly="True">pay</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Merchant AccessCode"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_AccessCode" runat="server">D67342C2</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Merchant Transaction Reference"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_MerchTxnRef" runat="server">uneque_merchTxnRef_per_txn</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="MerchantID"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_Merchant" runat="server">ONEPAY</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Transaction OrderInfo"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_OrderInfo" runat="server">order_information</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Purchase Amount"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_Amount" runat="server">1000</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="Currency code"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_Currency" runat="server">VND</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Payment Server Display Language Locale"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_Locale" runat="server" ReadOnly="True">en</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Receipt ReturnURL"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_ReturnURL" runat="server" Width="250px">http://localhost:1410/vpc_dr.aspx</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="IP Address"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="vpc_TicketNo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="2">
                            <asp:Button ID="Button1" runat="server" Text="Check out" OnClick="Button1_Click" />
                        </td>
                    </tr>
                </table>
                </form>
            </td>
        </tr>
    </table>
</body>
</html>
