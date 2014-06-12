using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// IEBrowser derived from ApplicationContext supports message loop
/// </summary>
public class IEBrowser : System.Windows.Forms.ApplicationContext 
{
    AutoResetEvent resultEvent;

    string userName, password;
    string htmlResult;
    string htmlCookieTable;
    string htmlInputTable;
    string htmlScriptTable;
    int status = 0;

    int loginCount;
    int navigationCounter;
    ScriptCallback scriptCallback;

    WebBrowser ieBrowser;
    Form form;
    Thread thrd;
    public int Status {
        get { return status; }
    }
    public string HtmlResult
    {
        get { return htmlResult; }
    }

    public int NavigationCounter
    {
        get { return navigationCounter; }
    }

    public string HtmlCookieTable
    {
        get { return "Cookies:" + htmlCookieTable; }
    }

    public string HtmlInputTable
    {
        get { return "Input elements:" + htmlInputTable; }
    }

    public string HtmlScriptTable
    {
        get { return "Script variables:" + htmlScriptTable; }
    }

    /// <summary>
    /// class constructor 
    /// </summary>
    /// <param name="visible">whether or not the form and the WebBrowser control are visiable</param>
    /// <param name="userName">client user name</param>
    /// <param name="password">client password</param>
    /// <param name="resultEvent">functionality to keep the main thread waiting</param>
    public IEBrowser(string userName, string password, AutoResetEvent resultEvent)
    {
        this.userName = userName;
        this.password = password;
        this.resultEvent = resultEvent;
        htmlResult = null;


        thrd = new Thread(new ThreadStart(
            delegate {
                Init();
                System.Windows.Forms.Application.Run(this); 
            }));
        // set thread to STA state before starting
        thrd.SetApartmentState(ApartmentState.STA);
        thrd.Start();
    }

    // initialize the WebBrowser and the form
    private void Init()
    {
        scriptCallback = new ScriptCallback(this);

        // create a WebBrowser control
        ieBrowser = new WebBrowser();
        // set the location of script callback functions
        ieBrowser.ObjectForScripting = scriptCallback;
        // set WebBrowser event handls
        ieBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(IEBrowser_DocumentCompleted);
        ieBrowser.Navigating += new WebBrowserNavigatingEventHandler(IEBrowser_Navigating);
       

        loginCount = 0;
        // initialise the navigation counter
        navigationCounter = 0;
        ieBrowser.Navigate("https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi");
    }

    // dipose the WebBrowser control and the form and its controls
    protected override void Dispose(bool disposing)
    {
        if (thrd != null)
        {
            thrd.Abort();
            thrd = null;
            return;
        }

        System.Runtime.InteropServices.Marshal.Release(ieBrowser.Handle);
        ieBrowser.Dispose();
        if (form != null) form.Dispose();
        base.Dispose(disposing);
    }

    // Navigating event handle
    void IEBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    {
       
    }

    // DocumentCompleted event handle
     void IEBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {

        HtmlDocument doc = ((WebBrowser)sender).Document;
         // set email address and password, and try to login three times
        if (this.status == 0)
        {

            doc.GetElementById("username").SetAttribute("value", userName);
            doc.GetElementById("password").SetAttribute("value", password);
            doc.GetElementById("fm1").InvokeMember("submit");           
            this.status = 1;
        }
        else if (status == 1)
        {
            htmlResult = ieBrowser.DocumentText;
        }
       
            
           
     
       
    }


    /// <summary>
    /// class to hold the functions called by script codes in the WebBrowser control
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ScriptCallback
    {
        IEBrowser owner;
        static string scriptPattern;

        public ScriptCallback(IEBrowser owner)
        {
            this.owner = owner;

            // read JScript.js only once
           // if (scriptPattern != null) return;
            //StreamReader rd = File.OpenText(System.AppDomain.CurrentDomain.BaseDirectory + "JScript.js");
           // scriptPattern = rd.ReadToEnd();
           // rd.Close();
        }

        // callback function to get the content of page in the WebBrowser control 
        public void getHtmlResult(int count)
        {
            // unequal means the content is not stable
            if (owner.navigationCounter != count) return;

            // get HTML content
            owner.htmlResult = owner.ieBrowser.DocumentText;

            HtmlDocument doc = owner.ieBrowser.Document;
            if (doc.Cookie != null)
            {
                // get cookies
                owner.htmlCookieTable = "<table border=1 cellspacing=0 cellpadding=2><tr><th>Name</th><th>Value</th><tr>";
                foreach (string cookie in Regex.Split(doc.Cookie, @";\s*"))
                {
                    string[] arr = cookie.Split(new char[] { '=' }, 2);
                    owner.htmlCookieTable += string.Format("<td>{0}</td><td>{1}</td></tr>", arr[0], (arr.Length == 2) ? arr[1] : "&nbsp;");
                }
                owner.htmlCookieTable += "</table><p />";
            }

            HtmlElementCollection inputs = doc.GetElementsByTagName("INPUT");
            //if (inputs.Count != 0)
            //{
            //    // get ids, names, values and types of input elements
            //    owner.htmlInputTable = "<table border=1 cellspacing=0 cellpadding=2><tr><th>Id</th><th>Name</th><th>Value</th><th>Type</th><tr>";
            //    foreach (var i in inputs)
            //    {
            //        var input = (HtmlElement)i;
            //        owner.htmlInputTable += string.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", input.GetAttribute("Id"), input.GetAttribute("Name"), input.GetAttribute("Value"), input.GetAttribute("Type"));
            //    }
            //    owner.htmlInputTable += "</table><p />";
            //    owner.htmlInputTable = owner.htmlInputTable.Replace("<td></td>", "<td>&nbsp;</td>");
            //}

            HtmlElementCollection scripts = doc.GetElementsByTagName("SCRIPT");
            if (scripts.Count != 0)
            {
                string vars = string.Empty;
                foreach (HtmlElement script in scripts)
                {
                    if (script.InnerHtml == null) continue;

                    foreach (string name in getVariableNames(script.InnerHtml).Split(new char[] { ';' }))
                    {
                        if (name.Trim().Length == 0) continue;
                        if (vars.Contains("\"" + name + "\"")) continue;

                        // one row of the script variable table - <tr>getValue([script variable name]</tr> - getValue() is a script function in JScript.js
                        vars += string.Format("+\"<tr>\"+getValue(\"{0}\")+\"</tr>\"", name);
                    }
                }

                // request script to send back the names, values and types of script variables
                doc.InvokeScript("setTimeout", new object[] { scriptPattern.Replace("{0}", vars.Substring(1)), 10 });
            }
            else
            {
                // set resultEvent to let main thread continue
                owner.resultEvent.Set();
            }
        }

        // get script variable names from the InnerHtml of script tag
        string getVariableNames(string InnerHtml)
        {
            // remove fuction definitions
            int n;
            Regex r = new Regex(@"\{|\}");
            while (true)
            {
                Match m1 = Regex.Match(InnerHtml, @"function\s+[^\(]+\(|new\s+function\s*\(|function\s*\(");
                if (!m1.Success) break;

                int nestCount = 0;
                n = m1.Groups[0].Index;
                do
                {
                    Match m2 = r.Match(InnerHtml, n + 1);
                    n = m2.Groups[0].Index;
                    nestCount += (m2.Groups[0].Value[0] == '{') ? 1 : -1;
                } while (nestCount != 0);

                InnerHtml = InnerHtml.Remove(m1.Groups[0].Index, n - m1.Groups[0].Index + 1);
            }

            // remove "if (...)"
            r = new Regex(@"\(|\)");
            while (true)
            {
                Match m1 = Regex.Match(InnerHtml, @"if\s*\(");
                if (!m1.Success) break;

                int nestCount = 0;
                n = m1.Groups[0].Index;
                do
                {
                    Match m2 = r.Match(InnerHtml, n + 1);
                    n = m2.Groups[0].Index;
                    nestCount += (m2.Groups[0].Value[0] == '(') ? 1 : -1;
                } while (nestCount != 0);

                InnerHtml = InnerHtml.Remove(m1.Groups[0].Index, n - m1.Groups[0].Index + 1);
            }

            InnerHtml = Regex.Replace(InnerHtml, @"\W+try\s*\{|\}\s*catch\([^\)]+\)\s*\{|\Welse\W", ";");
            InnerHtml = Regex.Replace(InnerHtml, @"<[^>]+>[^<]*<[^>]+>|<!--[^>]+>", "");
            InnerHtml = Regex.Replace(InnerHtml, @"\+*=[^;\}]+[;\}]|\{|\}", ";");

            string variables = string.Empty;
            //Match m = Regex.Match(InnerHtml, @"(var\s|;+|^|\{+)\s*(\S+?)\s*(=[\s\S]+?[;$]|;|\}|$)");
            r = new Regex(@"(var\s|;+|^)\s*(\S+?)\s*(;+|$)");
            n = 0;
            while (true)
            {
                Match m = r.Match(InnerHtml, n);
                if (!m.Success) break;

                variables += ";" + m.Groups[2].Value;
                n = m.Index + m.Length - 1;
            }

            // remove function calling
            variables = Regex.Replace(variables, @";[^;]+\([^\(;]+", "", RegexOptions.RightToLeft);

            variables = Regex.Replace(variables, @"^\W*;+\s*|\s*;+\W*$", "");
            variables = Regex.Replace(variables, @";{2,}", ";");
            variables = variables.Replace('"', '\'');

            return variables;
        }

        // callback function to set the names, values and types of script variables
        // parameter vars conains the names, values and types of script variables
        public void getScriptResult(string vars) {

            // set script table
            owner.htmlScriptTable = "<table border=1 cellspacing=0 cellpadding=2><tr><th>Name</th><th>Value</th><th>Type</th><tr>" + vars + "</table><p />";
            owner.htmlScriptTable = owner.htmlScriptTable.Replace("<td></td>", "<td>&nbsp;</td>");

            // set resultEvent to let main thread continue
            owner.resultEvent.Set();
        }
    }
}
