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

        /// <summary>
        /// ten
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ma tin
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// mo ta ngan
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// mo ta day du
        /// </summary>
        public string FullDescription { get; set; }
        /// <summary>
        /// sename
        /// </summary>
        public string SeName { get; set; }
        /// <summary>
        /// ten lien he
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// dien thoai lien he
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// email lien he
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// id category
        /// </summary>
        public int CateId { get; set; }
        /// <summary>
        /// dien tich
        /// </summary>
        public decimal Area { get; set; }
        /// <summary>
        /// dien tich su dung
        /// </summary>
        public decimal AreaUse { get; set; }
        /// <summary>
        /// chieu rong
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// chieu dai
        /// </summary>
        public decimal Dept { get; set; }
        /// <summary>
        /// so nha
        /// </summary>
        public string NumberOfHome { get; set; }
        /// <summary>
        /// so phong ngu
        /// </summary>
        public string NumBedRoom { get; set; }
        /// <summary>
        /// so phong tam(bathroom thi dung hon?)
        /// </summary>
        public string NumBadRoom { get; set; }
        /// <summary>
        /// co so vat chat
        /// </summary>
        public string CoSoVatChat { get; set; }
        /// <summary>
        /// moi truong xung quanh
        /// </summary>
        public string Moitruong { get; set; }
        /// <summary>
        /// trang thai duyet tin
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// tien nghi
        /// </summary>
        public string TienNghi { get; set; }
        /// <summary>
        /// so tang
        /// </summary>
        public string NumberBlock { get; set; }
        /// <summary>
        /// ten Quan/huyen
        /// </summary>
        public string DictrictName { get; set; }
        /// <summary>
        /// huong: dong, tay, nam, bac
        /// </summary>
        public string Directors { get; set; }
        /// <summary>
        /// phap ly: so do, so hong,...
        /// </summary>
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

        /// <summary>
        /// trang thai tin: dang ban(sale) - da dat coc (ordered) - da ban(sole)
        /// </summary>
        public string ProductStatusText { get; set; }

        /// <summary>
        /// La du an
        /// </summary>
        public bool IsProject { get; set; }
        public string ChuDauTu { get; set; }
        /// <summary>
        /// using column GifcartTypeId
        /// </summary>
        public int StatusId { get; set; }
    }
}