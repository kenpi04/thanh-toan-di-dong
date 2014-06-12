using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlTest
{
    public static class UrlContant
    {
        public static String HOME_PAGE = "https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi";
        public static String LOGIN_URL = "https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi";
        public static String LOGIN_REFERER_URL = "https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi";
        public static String SMS_URL = "http://vinaphone.com.vn/messaging/sms/sms.do";
        public static String SEND_SMS_RESULT_URL = "http://vinaphone.com.vn/messaging/sms/sendSms.do";
        public static String SMS_REFERER_URL = "http://vinaphone.com.vn/messaging/sms/sms.do?bSubs={receiveNumber}";
        public static String CAPTCHA_SMS_URL = "http://vinaphone.com.vn/Kaptcha2.jpg?";
    }
}