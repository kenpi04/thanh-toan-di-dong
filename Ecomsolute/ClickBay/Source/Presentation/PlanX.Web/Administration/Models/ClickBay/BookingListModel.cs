﻿using PlanX.Web.Framework;
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

        [NopResourceDisplayName("Admin.ClickBay.List.BookingStatusId")]
        public int BookingStatusId { get; set; }

        [NopResourceDisplayName("Admin.ClickBay.List.CustomerId")]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.ClickBay.List.BookingId")]
        public int BookingId { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.List.PRNCode")]
        public string PRNCode { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.List.TicketId")]
        public string TicketId { get; set; }

        public IList<SelectListItem> AvailableBookingStatuses { get; set; }
    }
}