using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhToanDiDong.Models.Payment
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string CustomerPhone { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }
        public string CreateDate { get; set; }
       

        public object Name { get; set; }
    }
}