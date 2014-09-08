using System;
using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductOverviewModel : BaseNopEntityModel
    {
        public ProductOverviewModel()
        {
            ProductPrice = new ProductPriceModel();
            DefaultPictureModel = new PictureModel();
            SpecificationAttributeModels = new List<ProductSpecificationModel>();
            ReviewOverviewModel = new ProductReviewOverviewModel();
        }

        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string SeName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public int CateId { get; set; }
        public decimal Area { get; set; }
        public decimal AreaUse { get; set; }
        public decimal Width { get; set; }
        public decimal Dept { get; set; }
        public string NumberOfHome { get; set; }
        public string NumBedRoom { get; set; }
        public string NumBadRoom { get; set; }
        public string CoSoVatChat { get; set; }
        public string Moitruong { get; set; }
        public string Status { get; set; }
        public string TienNghi { get; set; }
        public string NumberBlock { get; set; }
        public string DictrictName { get; set; }
        public string Directors { get; set; }
        public string PhapLy { get; set; }
        //price
        public ProductPriceModel ProductPrice { get; set; }
        //picture
        public PictureModel DefaultPictureModel { get; set; }
        //specification attributes
        public IList<ProductSpecificationModel> SpecificationAttributeModels { get; set; }
        //price
        public ProductReviewOverviewModel ReviewOverviewModel { get; set; }

		#region Nested Classes

        public partial class ProductPriceModel : BaseNopModel
        {
            public string OldPrice { get; set; }
            public string Price {get;set;}

            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }

            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            public bool ForceRedirectionAfterAddingToCart { get; set; }
        }

		#endregion

        public string FullAddress { get; set; }

        public string CateName { get; set; }

        public string ProductStatusText { get; set; }

        public bool IsProject { get; set; }
        public string ChuDauTu { get; set; }
        /// <summary>
        /// using column GifcartTypeId
        /// </summary>
        public int StatusId { get; set; }
    }
}