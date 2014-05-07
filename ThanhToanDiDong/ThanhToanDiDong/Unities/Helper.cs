using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhToanDiDong.Unities
{
    public class Helper
    {
        public static string GetIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (ip != null)
                return ip;
            return "";
        }
    }
}