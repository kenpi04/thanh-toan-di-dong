using System;
using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Customer
{
    public partial class CustomerOrderListModel : BaseNopModel
    {
        public CustomerOrderListModel()
        {
            Orders = new List<OrderDetailsModel>();
            RecurringOrders = new List<RecurringOrderModel>();
            CancelRecurringPaymentErrors = new List<string>();
            Products = new List<ProductProfileModel>();
        }

        public IList<OrderDetailsModel> Orders { get; set; }
        public IList<RecurringOrderModel> RecurringOrders { get; set; }
        public IList<string> CancelRecurringPaymentErrors { get; set; }

        public CustomerNavigationModel NavigationModel { get; set; }

        public IList<ProductProfileModel> Products { get; set; }

        #region Nested classes
        public partial class OrderDetailsModel : BaseNopEntityModel
        {
            public string OrderTotal { get; set; }
            public bool IsReturnRequestAllowed { get; set; }
            public string OrderStatus { get; set; }
            public DateTime CreatedOn { get; set; }
        }
        public partial class RecurringOrderModel : BaseNopEntityModel
        {
            public string StartDate { get; set; }
            public string CycleInfo { get; set; }
            public string NextPayment { get; set; }
            public int TotalCycles { get; set; }
            public int CyclesRemaining { get; set; }
            public int InitialOrderId { get; set; }
            public bool CanCancel { get; set; }
        }
        public partial class ProductProfileModel : BaseNopEntityModel
        {
            public ProductProfileModel()
            {
                DefaultPictureModel = new Media.PictureModel();
            }

            public string Sku { get; set; }
            public string Name { get; set; }
            public string Sename { get; set; }
            public string Area { get; set; }
            public string Price { get; set; }
            public string BathRoom { get; set; }
            public string BedRoom { get; set; }
            public string TinhTrang { get; set; }
            public string ViewNumber { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime UpdatedOn { get; set; }
            public DateTime? ExpiredOn { get; set; }
            public string TrangThaiDuyet { get; set; }
            public Media.PictureModel DefaultPictureModel { get; set; }
        }

        #endregion
    }
}