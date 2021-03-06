using System;
using System.Configuration;
using Payoo.Lib;
using System.Web;
using System.Text;

public partial class NotifyListener : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.RequestType == "POST")
        {
            string NotifyMessage = Request.Form.Get("NotifyData");
            //NotifyMessage = "<?xml version='1.0'?><PayooConnectionPackage xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Data>PFBheW1lbnROb3RpZmljYXRpb24+PHNob3BzPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxzaG9wPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8c2Vzc2lvbj4xNjg2MjQ0OTIzPC9zZXNzaW9uPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dXNlcm5hbWU+cG5obG9uZzwvdXNlcm5hbWU+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxzaG9wX2lkPjM4MDwvc2hvcF9pZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHNob3BfdGl0bGU+VmlldGtpdGU8L3Nob3BfdGl0bGU+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxzaG9wX2RvbWFpbj5odHRwOi8vdmlldGtpdGUuY29tLzwvc2hvcF9kb21haW4+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxzaG9wX2JhY2tfdXJsPmh0dHA6Ly92aWV0a2l0ZS5jb20vcGF5b28vdGhhbmt5b3UuYXNweDwvc2hvcF9iYWNrX3VybD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPG9yZGVyX25vPjE2ODYyNDQ5MjM8L29yZGVyX25vPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8b3JkZXJfY2FzaF9hbW91bnQ+Mjwvb3JkZXJfY2FzaF9hbW91bnQ+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxvcmRlcl9zaGlwX2RhdGU+MzAvMDEvMjAxMzwvb3JkZXJfc2hpcF9kYXRlPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8b3JkZXJfc2hpcF9kYXlzPjE8L29yZGVyX3NoaXBfZGF5cz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPG9yZGVyX2Rlc2NyaXB0aW9uPkNoaSt0aSVlMSViYSViZnQraCVjMyViM2ErJWM0JTkxJWM2JWExbit0aGFuaCt0byVjMyVhMW4rbmh1K3NhdSUzYUhQK1BhdmlsaW9uK0RWMy0zNTAyVFgrR2klYzMlYTElM2ErMjMxMDkyMTAuRkFOK05vdGVib29rKyhCNCkrR2klYzMlYTElM2ErMjY2ODUwLihTb21lK25vdGVzK2Zvcit0aGUrb3JkZXIpPC9vcmRlcl9kZXNjcmlwdGlvbj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPG5vdGlmeV91cmw+aHR0cDovL3ZpZXRraXRlLmNvbS9wYXlvby9Ob3RpZnlMaXN0ZW5lci5hc3B4PC9ub3RpZnlfdXJsPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvc2hvcD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvc2hvcHM+PFN0YXRlPlBBWU1FTlRfUkVDRUlWRUQ8L1N0YXRlPjwvUGF5bWVudE5vdGlmaWNhdGlvbj4=</Data><Signature>MIIBbQYJKoZIhvcNAQcCoIIBXjCCAVoCAQExCzAJBgUrDgMCGgUAMAsGCSqGSIb3DQEHATGCATkwggE1AgEBMIGSMIGEMQswCQYDVQQGEwJWVTEMMAoGA1UECBMDSENNMQwwCgYDVQQHEwNIQ00xEjAQBgNVBAoTCVZpZXRVbmlvbjEOMAwGA1UECxMFUGF5b28xDjAMBgNVBAMTBVBheW9vMSUwIwYJKoZIhvcNAQkBFhZwYXlvb0B2aWV0dW5pb24uY29tLnZuAgkA673T+Q8894cwCQYFKw4DAhoFADANBgkqhkiG9w0BAQEFAASBgE5KARgcs45SfKa9+FYzwrjkPPpspY++aEnHaN48yK/Sd9I1yH4m9bFNWSH9W9Yg1SV7UnjeT+9HfjGE3L6Eo9ElRyymdQTpLXY8WH9YmuKEe53YqbH6mxmoxAwVXS69OlkaTivEOKffxP7QytwqMG31Ti40dTIYdGMIkJ0qJReT</Signature><PayooSessionID>DI0jRfI1Am1xbc9q+oFU3eJ9zCRCKNUSTTcjsL3psZltUb/bhIUObx8TjZ572KpHXkOKt9SwrBEVdh4qiVXo0Q==</PayooSessionID></PayooConnectionPackage>";

            if (NotifyMessage == null || "".Equals(NotifyMessage))
            {
                return;
            }

            PayooNotify listener = new PayooNotify(NotifyMessage);

            PaymentNotification invoice = listener.GetPaymentNotify();

            //Xác thực chữ ký của payoo trong gói notify
            PayooSignature py = new PayooSignature(Server.MapPath(@"App_Data\Certificates\payoo_public_cert.pem"));
            if (py.Verify(listener.NotifyData, listener.Signature))
            {
                //Neu trang thai don hang cua payoo là PAYMENT_RECEIVED -> tien hanh xu ly databse cap nhat don hang
                if (invoice.State == "PAYMENT_RECEIVED")
                {
                    string LogPath = Server.MapPath(@"App_Data\invoice.txt");
                    LogWriter.WriteLog(LogPath, "Date: " + DateTime.Now.ToString());
                    LogWriter.WriteLog(LogPath, "OrderNo: " + invoice.OrderNo);
                    LogWriter.WriteLog(LogPath, "OrderCashAmount: " + invoice.OrderCashAmount);
                }
            }
            else
            {
                //ConfirmToPayoo fail. Log for manual investigation.
                string LogPath = Server.MapPath(@"App_Data\log.txt");
                LogWriter.WriteLog(LogPath, "ConfirmToPayoo fail. ");
            }
        }
    }
}
