using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Admin.Models.Messages
{
    public class MessageListModel
    {
        public MessageListModel()
        {
            Type = new List<SelectListItem>();
            ToDate = new DateTime();
            FromDate = new DateTime();
        }
        public string SearchName { get; set; }
        public int SearchType { get; set; }
        public List<SelectListItem> Type { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }       
    }
}