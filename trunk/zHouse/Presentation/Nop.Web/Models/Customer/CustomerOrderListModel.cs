using System;
using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.UI.Paging;

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
            PagingFilteringContext = new PagingFilteringModel();
        }

        public IList<OrderDetailsModel> Orders { get; set; }
        public IList<RecurringOrderModel> RecurringOrders { get; set; }
        public IList<string> CancelRecurringPaymentErrors { get; set; }

        public CustomerNavigationModel NavigationModel { get; set; }

        public IList<ProductProfileModel> Products { get; set; }

        public PagingFilteringModel PagingFilteringContext { get; set; }

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
            public decimal Area { get; set; }
            public string Price { get; set; }
            //public string BathRoom { get; set; }
            //public string BedRoom { get; set; }
            public string TinhTrang { get; set; }
            public string ViewNumber { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime UpdatedOn { get; set; }
            public DateTime? AvailableStartDateTimeUtc { get; set; }
            public DateTime? AvailableEndDateTimeUtc { get; set; }
            public short TrangThaiDuyetId { get; set; }
            public string TrangThaiDuyet { get; set; }
            public Nop.Core.Domain.Catalog.ProductType ProductType { get; set; }
            public Media.PictureModel DefaultPictureModel { get; set; }
        }

        #endregion
    }

    public class PagingFilteringModel:BasePageableModel
    {
        public PagingFilteringModel()
        {
            this.categoryId = 0;
            this.statusEndDate = 0;
            this.status = 0;
            this.productId = 0;
            this.customerId = 0;
            this.districtId = 0;
        }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int categoryId { get; set; }
        public int statusEndDate {get;set;}
        public int status {get;set;}
        public int productId { get; set; }
        public int customerId { get; set; }
        public int districtId { get; set; }
    }
}