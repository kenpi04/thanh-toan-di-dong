<%@ Page language="c#" Inherits="WebApplication1.vpc_dr" CodeFile="vpc_dr.aspx.cs" ValidateRequest="false"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title> Response Virtual Payment Client</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<!-- Start Branding Table -->
			<table id="Table1" cellpadding="2" width="100%" bgcolor="#0074c4" border="2">
				<tr>
					<td width="50%" bgcolor="#ced7ef">
						<h2 class="co">&nbsp;Virtual Payment Client Example</h2>
					</td>
					<td align="center" bgcolor="#0074c4">
						<h3 class="co">OnePay</h3>
					</td>
				</tr>
			</table>
			<!-- End Branding Table -->			
				
			<table id="Table2" cellPadding="5" width="85%" align="center" border="0">
				<tr bgColor="#0074c4">
					<td colSpan="2" height="25">
						<p><strong>&nbsp;Basic Transaction Fields</strong></p>
					</td>
				</tr>
				<tr align="center" >
					<td  colSpan="2" width="45%">
						<asp:Label id="vpc_Result" runat="server" Width="208px"></asp:Label></td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right" width="55%"><strong><i>VPC API Version: </i></strong>
					</td>
					<td width="45%">
						<asp:Label id="vpc_Version" runat="server" Width="208px"></asp:Label></td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Command: </i></strong>
					</td>
					<td>
						<asp:Label id="vpc_Command" runat="server" Width="216px"></asp:Label></td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Merchant Transaction Reference: </i></strong>
					</td>
					<td>
						<asp:Label id="vpc_MerchantRef" runat="server" Width="224px"></asp:Label></td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Merchant ID: </i></strong>
					</td>
					<td>
						<asp:Label id="vpc_MerchantID" runat="server" Width="200px"></asp:Label></td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Order Information: </i></strong>
					</td>
					<td>
						<asp:Label id="vpc_OderInfor" runat="server" Width="208px"></asp:Label></td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Amount: </i></strong>
					</td>
					<td>
						<asp:Label id="vpc_Amount" runat="server" Width="208px"></asp:Label></td>
				</tr>
				<tr>
					<td align="center" colSpan="2"><font color="#0074c4">Fields above are the request 
							values returned.<br>
							<hr>
							Fields below are the response fields for a Standard Transaction.<br>
						</font>
					</td>
				</tr>
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>VPC Transaction Response Code: </i></strong>
					</td>
					<td>
						<asp:Label id="vpc_ResponseCode" runat="server" Width="216px"></asp:Label></td>
				</tr>
				
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Message: </i></strong>
					</td>
					<td><asp:Label id="vpc_Message" runat="server" Width="224px"></asp:Label></td>
				</tr>			
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Transaction Number: </i></strong>
					</td>
					<td><asp:Label id="vpc_TransactionNo" runat="server" Width="224px"></asp:Label></td>
				</tr>
			
							
				<tr bgColor="#ced7ef">
					<td align="right"><strong><i>Hash Validated Correctly: </i></strong>
					</td>
					<td><asp:Label id="hashvalidate" runat="server" Width="216px"></asp:Label></td>
				</tr>
			</table>
			
				<p align="center" ><a href="http://localhost:1040/vpc_do.aspx">New Transaction</a></p>
			
		</form>
	
	</body>
</HTML>
