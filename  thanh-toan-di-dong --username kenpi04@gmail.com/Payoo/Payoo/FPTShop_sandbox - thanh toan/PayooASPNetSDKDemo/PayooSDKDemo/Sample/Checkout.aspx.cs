using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Payoo.Lib;


public partial class Checkout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnPaynow_Click(object sender, ImageClickEventArgs e)
    {
        Random r = new Random();
        string OrderNo = r.Next().ToString(); //demo generate ngẫu nhiên 1 mã đơn hàng

        PayooOrder order = new PayooOrder();
        order.Session = OrderNo;
        order.BusinessUsername = ConfigurationManager.AppSettings["BusinessUsername"];
        order.OrderCashAmount = 2;
        order.OrderNo = OrderNo;
        order.ShippingDays = short.Parse(ConfigurationManager.AppSettings["ShippingDays"]);
        order.ShopBackUrl = ConfigurationManager.AppSettings["ShopBackUrl"];
        order.ShopDomain = ConfigurationManager.AppSettings["ShopDomain"];
        order.ShopID = long.Parse(ConfigurationManager.AppSettings["ShopID"]);
        order.ShopTitle = ConfigurationManager.AppSettings["ShopTitle"];
        order.StartShippingDate = DateTime.Now.ToString("dd/MM/yyyy");
        order.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];

        //You can do

        //order.OrderDescription = HttpUtility.UrlEncode("<table width='100%' border='1' cellspacing='0'><thead><tr><td width='40%' align='center'><b>Tên hàng</b></td><td width='20%' align='center'><b>Đơn giá</b></td><td width='15%' align='center'><b>Số lượng</b></td><td width='25%' align='center'><b>Thành tiền</b></td></tr></thead><tbody><tr><td align='left'>HP Pavilion DV3-3502TX</td><td align='right'>23,109,210</td><td align='center'>1</td><td align='right'>23,109,210</td></tr><tr><td align='left'>FAN Notebook (B4)</td><td align='right'>266,850</td><td align='center'>1</td><td align='right'>266,850</td></tr><tr><td align='right' colspan='3'><b>Tổng tiền:</b></td><td align='right'>23,376,060</td></tr><tr><td align='left' colspan='4'>Some notes for the order</td></tr></tbody></table>");

        //or
        long ShippingFee = 10000;
        BuidDescriptionFactory builder = new BuidDescriptionFactory();
        builder.AddItem(new PayooOrderItem("HP Pavilion DV3-3502TX", 23109210, 1));
        builder.AddItem(new PayooOrderItem("FAN Notebook (B4)", 266850, 1));
        order.OrderDescription = HttpUtility.UrlEncode(builder.GenerateDescription(ShippingFee, "Some notes for the order"));

        string XML = PaymentXMLFactory.GetPaymentXMLWithoutSign(order);
        string ChecksumKey = ConfigurationManager.AppSettings["ChecksumKey"];
        string Checksum = SHA1encode.hash(ChecksumKey + XML);

        RedirectToProvider(ConfigurationManager.AppSettings["PayooCheckout"], XML, Checksum);
    }

    private void RedirectToProvider(string ProviderUrl, string XMLCheckout, string Checksum)
    { 
        string redirect = "<html><head><title></title></head>";
        redirect += "<body><form action='" + ProviderUrl + "' method='post' style='margin-top: 50px; text-align: center;'>";
        redirect += "<noscript><input type='submit' value='Click if not redirected' /></noscript>";
        redirect += "<div id='ContinueButton' style='display: none;'><input type='submit' value='Click if not redirected' />";
        redirect += "</div><input type='hidden' name='OrdersForPayoo' value='" + XMLCheckout + "'/>";
        redirect += "</div><input type='hidden' name='CheckSum' value='" + Checksum + "'/></form>";
        redirect += "<script type='text/javascript'>window.onload = function() ";
        redirect += "{document.forms[0].submit();setTimeout(function() {document.getElementById('ContinueButton').style.display = '';}, 1000);}";
        redirect += "</script></body></html>";
        Response.Write(redirect);
    }
}
