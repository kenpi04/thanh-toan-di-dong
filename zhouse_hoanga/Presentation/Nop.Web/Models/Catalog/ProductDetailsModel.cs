﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductDetailsModel : BaseNopEntityModel
    {
        public ProductDetailsModel()
        {
            DefaultPictureModel = new PictureModel();
            PictureModels = new List<PictureModel>();
            GiftCard = new GiftCardModel();
            ProductPrice = new ProductPriceModel();
            AddToCart = new AddToCartModel();
            ProductVariantAttributes = new List<ProductVariantAttributeModel>();
            AssociatedProducts = new List<ProductDetailsModel>();
            VendorModel = new VendorBriefInfoModel();
            Facilities = new List<SelectListItem>();
            Environments = new List<SelectListItem>();
            PictureModels = new List<PictureModel>();
            TienIch = new List<string>();
            ThichHop = new List<string>();
           
        }

        //picture(s)
        public bool DefaultPictureZoomEnabled { get; set; }
        public PictureModel DefaultPictureModel { get; set; }
        public IList<PictureModel> PictureModels { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ProductTemplateViewPath { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public string CateName { get; set; }
        public string FullAddress { get; set; }

        public bool ShowSku { get; set; }
        public string Sku { get; set; }

        public bool ShowManufacturerPartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }

        public bool ShowGtin { get; set; }
        public string Gtin { get; set; }

        public bool ShowVendor { get; set; }
        public VendorBriefInfoModel VendorModel { get; set; }

        public bool HasSampleDownload { get; set; }

        public GiftCardModel GiftCard { get; set; }

        public bool IsShipEnabled { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool FreeShippingNitificationEnabled { get; set; }
        public string DeliveryDate { get; set; }
        /// <summary>
        /// la du an
        /// </summary>
        public bool IsProject { get; set; }

        public string StockAvailability { get; set; }

        public bool IsCurrentCustomerRegistered { get; set; }
        public bool DisplayBackInStockSubscription { get; set; }
        public bool BackInStockAlreadySubscribed { get; set; }

        public ProductPriceModel ProductPrice { get; set; }

        public AddToCartModel AddToCart { get; set; }

        public IList<ProductVariantAttributeModel> ProductVariantAttributes { get; set; }
    
        //a list of associated products. For example, "Grouped" products could have several child "simple" products
        public IList<ProductDetailsModel> AssociatedProducts { get; set; }
        
        /// <summary>
        /// trang thai duyet
        /// </summary>
        public string Status { get; set; }

        public string DistrictName { get; set; }

        public string HinhThuc { get; set; }

        /// <summary>
        /// dien tich
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// so tang
        /// </summary>
        public string NumberFloors { get; set; }
        /// <summary>
        /// so phong tam
        /// </summary>
        public string BathRooms { get; set; }
        /// <summary>
        /// so phong tam
        /// </summary>
        public string BedRooms { get; set; }
        public List<string> TienIch { get; set; }
        public List<string> ThichHop { get; set; }
        /// <summary>
        /// Huong: dong,tay,nam,bac
        /// </summary>
        public string Directors { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        /// <summary>
        /// ngay thi cong
        /// </summary>
        public string StartConstructionDate { get; set; }
        /// <summary>
        /// Ngay hoan cong
        /// </summary>
        public string FinishConstructionDate { get; set; }

        /// <summary>
        /// Manufactor partnumber
        /// </summary>
        public string ChuDauTu { get; set; }
        /// <summary>
        /// Gtin
        /// </summary>
        public string Contructors { get; set; }
        /// <summary>
        /// co so vat chat
        /// </summary>
        public IList<SelectListItem> Facilities { get; set; }
        /// <summary>
        /// moi truong xung quanh
        /// </summary>
        public IList<SelectListItem> Environments { get; set; }
        public int CustomerId { get; set; }

		#region Nested Classes

        public partial class ProductBreadcrumbModel : BaseNopModel
        {
            public ProductBreadcrumbModel()
            {
                CategoryBreadcrumb = new List<CategoryModel>();
            }

            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductSeName { get; set; }
            public IList<CategoryModel> CategoryBreadcrumb { get; set; }
        }

        public partial class AddToCartModel : BaseNopModel
        {
            public AddToCartModel()
            {
                this.AllowedQuantities = new List<SelectListItem>();
            }
            public int ProductId { get; set; }

            [NopResourceDisplayName("Products.Qty")]
            public int EnteredQuantity { get; set; }

            [NopResourceDisplayName("Products.EnterProductPrice")]
            public bool CustomerEntersPrice { get; set; }
            [NopResourceDisplayName("Products.EnterProductPrice")]
            public decimal CustomerEnteredPrice { get; set; }
            public String CustomerEnteredPriceRange { get; set; }

            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }
            public List<SelectListItem> AllowedQuantities { get; set; }

            //pre-order
            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            //updating existing shopping cart item?
            public int UpdatedShoppingCartItemId { get; set; }
        }

        public partial class ProductPriceModel : BaseNopModel
        {
            /// <summary>
            /// The currency (in 3-letter ISO 4217 format) of the offer price 
            /// </summary>
            public string CurrencyCode { get; set; }

            public string OldPrice { get; set; }

            public string Price { get; set; }
            public string PriceWithDiscount { get; set; }

            public decimal PriceValue { get; set; }
            public decimal PriceWithDiscountValue { get; set; }

            public bool CustomerEntersPrice { get; set; }

            public bool CallForPrice { get; set; }

            public int ProductId { get; set; }

            public bool HidePrices { get; set; }

            public bool DynamicPriceUpdate { get; set; }
        }

        public partial class GiftCardModel : BaseNopModel
        {
            public bool IsGiftCard { get; set; }

            [NopResourceDisplayName("Products.GiftCard.RecipientName")]
            [AllowHtml]
            public string RecipientName { get; set; }
            [NopResourceDisplayName("Products.GiftCard.RecipientEmail")]
            [AllowHtml]
            public string RecipientEmail { get; set; }
            [NopResourceDisplayName("Products.GiftCard.SenderName")]
            [AllowHtml]
            public string SenderName { get; set; }
            [NopResourceDisplayName("Products.GiftCard.SenderEmail")]
            [AllowHtml]
            public string SenderEmail { get; set; }
            [NopResourceDisplayName("Products.GiftCard.Message")]
            [AllowHtml]
            public string Message { get; set; }

            public GiftCardType GiftCardType { get; set; }
        }

        public partial class TierPriceModel : BaseNopModel
        {
            public string Price { get; set; }

            public int Quantity { get; set; }
        }

        public partial class ProductVariantAttributeModel : BaseNopEntityModel
        {
            public ProductVariantAttributeModel()
            {
                AllowedFileExtensions = new List<string>();
                Values = new List<ProductVariantAttributeValueModel>();
            }

            public int ProductId { get; set; }

            public int ProductAttributeId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Selected value for textboxes
            /// </summary>
            public string TextValue { get; set; }
            /// <summary>
            /// Selected day value for datepicker
            /// </summary>
            public int? SelectedDay { get; set; }
            /// <summary>
            /// Selected month value for datepicker
            /// </summary>
            public int? SelectedMonth { get; set; }
            /// <summary>
            /// Selected year value for datepicker
            /// </summary>
            public int? SelectedYear { get; set; }
            /// <summary>
            /// Allowed file extensions for customer uploaded files
            /// </summary>
            public IList<string> AllowedFileExtensions { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<ProductVariantAttributeValueModel> Values { get; set; }

        }

        public partial class ProductVariantAttributeValueModel : BaseNopEntityModel
        {
            public string Name { get; set; }

            public string ColorSquaresRgb { get; set; }

            public string PriceAdjustment { get; set; }

            public decimal PriceAdjustmentValue { get; set; }

            public bool IsPreSelected { get; set; }

            public int PictureId { get; set; }
            public string PictureUrl { get; set; }
            public string FullSizePictureUrl { get; set; }
        }

		#endregion


        /// <summary>
        /// trang thai tin: dang ban, da ban, dang xay
        /// </summary>
        public string ProductStatusText { get; set; }
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

        public string PhapLy { get; set; }

        public int StatusId { get; set; }
        /// <summary>
        /// UserAgreementText
        /// </summary>
        public string DacDiemNoiBat { get; set; }
        /// <summary>
        /// Khuyen mai
        /// </summary>
        public string Promotion { get; set; }

        public string UserAgreementText { get; set; }
    }
}