using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhToanDiDong.Admin.Models
{
    public class FilterModel
    {
        public FilterModel()
        {
        }     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public PagingInfo PagingModel { get; set; }
    }

}