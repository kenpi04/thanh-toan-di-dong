using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ThanhToanDiDong.Admin.Models
{
    public class OrderListModel
    {
        public OrderListModel()
        {
           
        }
     
        public int CateId { get; set; }
        
        public DateTime ?StartDate { get; set; }
        public DateTime ?EndDate { get; set; }
        public int Status { get; set; }
        public PagingInfo PagingModel { get; set; }
        public IList<SelectListItem> CateMobiles { get; set; }
        
        
    }
}