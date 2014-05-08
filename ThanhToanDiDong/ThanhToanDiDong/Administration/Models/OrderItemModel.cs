using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhToanDiDong.Admin.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string ServiceName { get; set; }
        public DateTime DateCreate { get; set; }
        public string  Status { get; set; }
        public string TotalPrice { get; set; }
    }
}