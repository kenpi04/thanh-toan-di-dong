using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sankyo.Models
{
    public class TopicModel
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}