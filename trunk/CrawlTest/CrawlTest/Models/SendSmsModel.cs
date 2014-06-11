using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlTest.Models
{
    public class SendSmsModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ToNumber { get; set; }
        
        public string Content { get; set; }
        public string Captcha { get; set; }
        public string CaptchaImageUrl { get; set; }

    }
}