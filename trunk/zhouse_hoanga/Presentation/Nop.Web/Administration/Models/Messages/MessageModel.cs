using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Admin.Models.Messages
{
    public class MessageModel : BaseNopEntityModel
    {
        public MessageModel()
        {
        }
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int SiteId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public DateTime BookDate { get; set; }
    }
}