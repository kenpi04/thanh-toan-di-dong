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
        public string CardSerie { get; set; }
        public string CardId { get; set; }
        public string OrderType { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ExpiredDate { get; set; }
        public int OrderStatusId { get; set; }
    }
}