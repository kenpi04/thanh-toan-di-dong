using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookingListModel:BaseNopModel
    {
        public BookingListModel()
        {
            this.AvailableBookingStatuses = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.ClickBay.List.CreateDate")]
        [UIHint("DateNullable")]
        public DateTime? CreateDate { get; set; }

        [NopResourceDisplayName("Admin.ClickBay.List.CustomerNameOrPhone")]
        public string CustomerNameOrPhone { get; set; }

        [NopResourceDisplayName("Admin.ClickBay.List.OrderStatus")]
        public int BookingStatusId { get; set; }

        [NopResourceDisplayName("Admin.ClickBay.List.PaymentStatus")]
        public int CustomerId { get; set; }
        
        [NopResourceDisplayName("Admin.ClickBay.List.GoDirectlyToNumber")]
        public int BookingId { get; set; }

        public IList<SelectListItem> AvailableBookingStatuses { get; set; }
    }
}