using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlTest.Models
{
    public class GetDataModel
    {
        public string DauSo { get; set; }
        public string SoDT { get; set; }
        public string StatusId { get; set; }
        public string NumberType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}