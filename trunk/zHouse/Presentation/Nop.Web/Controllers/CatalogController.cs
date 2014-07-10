﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.News;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.UI.Captcha;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Media;
using Nop.Core.Domain.Messages;
using Nop.Services.Topics;

namespace Nop.Web.Controllers
{
    public partial class CatalogController : BaseNopController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly ICategoryTemplateService _categoryTemplateService;
        private readonly IManufacturerTemplateService _manufacturerTemplateService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IWebHelper _webHelper;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
        private readonly ICompareProductsService _compareProductsService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IProductTagService _productTagService;
        private readonly IOrderReportService _orderReportService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISearchTermService _searchTermService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IShippingService _shippingService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ICategoryNewsService _catenewsService;
        private readonly IMessagesService _messagesService;

        private readonly ITopicService _topicService;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly VendorSettings _vendorSettings;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly BlogSettings _blogSettings;
        private readonly ForumSettings _forumSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly ICacheManager _cacheManager;
        private readonly CaptchaSettings _captchaSettings;
        private readonly ICustomerService _customerService;

        private readonly IStateProvinceService _stateProvinceService;

        #endregion

        #region Constructors

        public CatalogController(ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService,
            IVendorService vendorService,
            IProductTemplateService productTemplateService,
            ICategoryTemplateService categoryTemplateService,
            IManufacturerTemplateService manufacturerTemplateService,
            IProductAttributeService productAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ITaxService taxService,
            ICurrencyService currencyService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IWebHelper webHelper,
            ISpecificationAttributeService specificationAttributeService,
            IDateTimeHelper dateTimeHelper,
            IRecentlyViewedProductsService recentlyViewedProductsService,
            ICompareProductsService compareProductsService,
            IWorkflowMessageService workflowMessageService,
            IProductTagService productTagService,
            IOrderReportService orderReportService,
            IGenericAttributeService genericAttributeService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            IEventPublisher eventPublisher,
            ISearchTermService searchTermService,
            IProductAttributeParser productAttributeParser,
            IShippingService shippingService,
            MediaSettings mediaSettings,
            CatalogSettings catalogSettings,
            VendorSettings vendorSettings,
            ShoppingCartSettings shoppingCartSettings,
            BlogSettings blogSettings,
            ForumSettings forumSettings,
            LocalizationSettings localizationSettings,
            CustomerSettings customerSettings,
            CaptchaSettings captchaSettings,
            ICacheManager cacheManager,
            IStateProvinceService stateProvinceService,
            IUrlRecordService urlRecordService,
            ICategoryNewsService catenewsService,
            ICustomerService customerService,
            IMessagesService messagesService,
            ITopicService topicService
            )
        {
            this._topicService = topicService;
            this._messagesService = messagesService;
            this._catenewsService = catenewsService;
            this._urlRecordService = urlRecordService;
            this._stateProvinceService = stateProvinceService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._productService = productService;
            this._vendorService = vendorService;
            this._productTemplateService = productTemplateService;
            this._categoryTemplateService = categoryTemplateService;
            this._manufacturerTemplateService = manufacturerTemplateService;
            this._productAttributeService = productAttributeService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._webHelper = webHelper;
            this._specificationAttributeService = specificationAttributeService;
            this._dateTimeHelper = dateTimeHelper;
            this._recentlyViewedProductsService = recentlyViewedProductsService;
            this._compareProductsService = compareProductsService;
            this._workflowMessageService = workflowMessageService;
            this._productTagService = productTagService;
            this._orderReportService = orderReportService;
            this._genericAttributeService = genericAttributeService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._permissionService = permissionService;
            this._customerActivityService = customerActivityService;
            this._eventPublisher = eventPublisher;
            this._searchTermService = searchTermService;
            this._productAttributeParser = productAttributeParser;
            this._shippingService = shippingService;


            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._vendorSettings = vendorSettings;
            this._shoppingCartSettings = shoppingCartSettings;
            this._blogSettings = blogSettings;
            this._forumSettings = forumSettings;
            this._localizationSettings = localizationSettings;
            this._customerSettings = customerSettings;
            this._captchaSettings = captchaSettings;

            this._cacheManager = cacheManager;
            this._customerService = customerService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected List<int> GetChildCategoryIds(int parentCategoryId)
        {
            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_CHILD_IDENTIFIERS_MODEL_KEY, parentCategoryId, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            return _cacheManager.Get(cacheKey, () =>
            {
                var categoriesIds = new List<int>();
                var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId);
                foreach (var category in categories)
                {
                    categoriesIds.Add(category.Id);
                    categoriesIds.AddRange(GetChildCategoryIds(category.Id));
                }
                return categoriesIds;
            });
        }

        [NonAction]
        protected int GetCategoryProductNumber(int categoryId)
        {
            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_NUMBER_OF_PRODUCTS_MODEL_KEY,
                string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id, categoryId);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var categoryIds = new List<int>();
                categoryIds.Add(categoryId);
                //include subcategories
                if (_catalogSettings.ShowCategoryProductNumberIncludingSubcategories)
                    categoryIds.AddRange(GetChildCategoryIds(categoryId));
                var numberOfProducts = _productService
                    .SearchProducts(categoryIds: categoryIds,
                    storeId: _storeContext.CurrentStore.Id,
                    visibleIndividuallyOnly: true,
                    pageSize: 1)
                    .TotalCount;
                return numberOfProducts;
            });
            return cachedModel;
        }

        /// <summary>
        /// Prepare category (simple) models
        /// </summary>
        /// <param name="rootCategoryId">Root category identifier</param>
        /// <param name="loadSubCategoriesForIds">Load subcategories only for the specified category IDs; pass null to load subcategories for all categories</param>
        /// <param name="level">Current level</param>
        /// <param name="levelsToLoad">A value indicating how many levels to load (max)</param>
        /// <param name="validateIncludeInTopMenu">A value indicating whether we should validate "include in top menu" property</param>
        /// <returns>Category models</returns>
        [NonAction]
        protected IList<CategorySimpleModel> PrepareCategorySimpleModels(int rootCategoryId,
            IList<int> loadSubCategoriesForIds, int level, int levelsToLoad, bool validateIncludeInTopMenu)
        {
            var result = new List<CategorySimpleModel>();
            foreach (var category in _categoryService.GetAllCategoriesByParentCategoryId(rootCategoryId))
            {
                if (validateIncludeInTopMenu && !category.IncludeInTopMenu)
                {
                    continue;
                }

                var categoryModel = new CategorySimpleModel()
                {
                    Id = category.Id,
                    Name = category.GetLocalized(x => x.Name),
                    SeName = category.GetSeName()
                };

                //product number for each category
                if (_catalogSettings.ShowCategoryProductNumber)
                {
                    categoryModel.NumberOfProducts = GetCategoryProductNumber(category.Id);
                }

                //load subcategories?
                bool loadSubCategories = false;
                if (loadSubCategoriesForIds == null)
                {
                    //load all subcategories
                    loadSubCategories = true;
                }
                else
                {
                    //we load subcategories only for certain categories
                    for (int i = 0; i <= loadSubCategoriesForIds.Count - 1; i++)
                    {
                        if (loadSubCategoriesForIds[i] == category.Id)
                        {
                            loadSubCategories = true;
                            break;
                        }
                    }
                }
                if (levelsToLoad <= level)
                {
                    loadSubCategories = false;
                }
                if (loadSubCategories)
                {
                    var subCategories = PrepareCategorySimpleModels(category.Id, loadSubCategoriesForIds, level + 1, levelsToLoad, validateIncludeInTopMenu);
                    categoryModel.SubCategories.AddRange(subCategories);
                }
                result.Add(categoryModel);
            }

            return result;
        }

        [NonAction]
        protected IEnumerable<ProductOverviewModel> PrepareProductOverviewModels(IEnumerable<Product> products,
            bool preparePriceModel = true, bool preparePictureModel = true,
            int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
            bool forceRedirectionAfterAddingToCart = false)
        {
            if (products == null)
                throw new ArgumentNullException("products");

            var models = new List<ProductOverviewModel>();
            foreach (var product in products)
            {
                var model = new ProductOverviewModel()
                {
                    Id = product.Id,
                    Name = product.GetLocalized(x => x.Name),
                    ShortDescription = product.GetLocalized(x => x.ShortDescription),
                    // FullDescription = product.GetLocalized(x => x.FullDescription),
                    SeName = product.GetSeName(),
                    ContactName = product.ContactName,
                    ContactPhone = product.ContactPhone,
                    Email = product.ContactEmail,

                    NumBedRoom = GetOptionName(product, ProductAttributeEnum.NumberOfBedRoom),
                    NumBadRoom = GetOptionName(product, ProductAttributeEnum.NumberOfBadRoom),
                    Status = GetOptionName(product, ProductAttributeEnum.Status),
                    NumberBlock = GetOptionName(product, ProductAttributeEnum.NumberBlock),
                    Directors=GetOptionName(product,ProductAttributeEnum.Director),
                    Area = product.Area,
                    FullAddress = product.FullAddress,
                    DictrictName = product.District.Name,
                 


                };
                if (product.CallForPrice)
                {
                    model.ProductPrice.Price = "Thỏa thuận";
                }
                else
                {
                    model.ProductPrice.Price = ReturnPriceString(product.Price, "đ");
                }
                //price
                //if (preparePriceModel)
                //{
                //    #region Prepare product price

                //    var priceModel = new ProductOverviewModel.ProductPriceModel()
                //    {
                //        ForceRedirectionAfterAddingToCart = forceRedirectionAfterAddingToCart
                //    };

                //    switch (product.ProductType)
                //    {
                //        case ProductType.GroupedProduct:
                //            {
                //                #region Grouped product

                //                var associatedProducts = _productService.SearchProducts(
                //                    storeId: _storeContext.CurrentStore.Id,
                //                    visibleIndividuallyOnly: false,
                //                    parentGroupedProductId: product.Id);

                //                switch (associatedProducts.Count)
                //                {
                //                    case 0:
                //                        {
                //                            //no associated products
                //                            priceModel.OldPrice = null;
                //                            priceModel.Price = null;
                //                            priceModel.DisableBuyButton = true;
                //                            priceModel.DisableWishlistButton = true;
                //                            priceModel.AvailableForPreOrder = false;
                //                        }
                //                        break;
                //                    default:
                //                        {
                //                            //we have at least one associated product
                //                            priceModel.DisableBuyButton = true;
                //                            priceModel.DisableWishlistButton = true;
                //                            priceModel.AvailableForPreOrder = false;

                //                            if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                //                            {
                //                                //find a minimum possible price
                //                                decimal? minPossiblePrice = null;
                //                                Product minPriceProduct = null;
                //                                foreach (var associatedProduct in associatedProducts)
                //                                {
                //                                    //calculate for the maximum quantity (in case if we have tier prices)
                //                                    var tmpPrice = _priceCalculationService.GetFinalPrice(associatedProduct,
                //                                        _workContext.CurrentCustomer, decimal.Zero, true, int.MaxValue);
                //                                    if (!minPossiblePrice.HasValue || tmpPrice < minPossiblePrice.Value)
                //                                    {
                //                                        minPriceProduct = associatedProduct;
                //                                        minPossiblePrice = tmpPrice;
                //                                    }
                //                                }
                //                                if (minPriceProduct != null && !minPriceProduct.CustomerEntersPrice)
                //                                {
                //                                    if (minPriceProduct.CallForPrice)
                //                                    {
                //                                        priceModel.OldPrice = null;
                //                                        priceModel.Price = _localizationService.GetResource("Products.CallForPrice");
                //                                    }
                //                                    else if (minPossiblePrice.HasValue)
                //                                    {
                //                                        //calculate prices
                //                                        decimal taxRate = decimal.Zero;
                //                                        decimal finalPriceBase = _taxService.GetProductPrice(minPriceProduct, minPossiblePrice.Value, out taxRate);
                //                                        decimal finalPrice = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceBase, _workContext.WorkingCurrency);

                //                                        priceModel.OldPrice = null;
                //                                        priceModel.Price = String.Format(_localizationService.GetResource("Products.PriceRangeFrom"), _priceFormatter.FormatPrice(finalPrice));

                //                                    }
                //                                    else
                //                                    {
                //                                        //Actually it's not possible (we presume that minimalPrice always has a value)
                //                                        //We never should get here
                //                                        Debug.WriteLine(string.Format("Cannot calculate minPrice for product #{0}", product.Id));
                //                                    }
                //                                }
                //                            }
                //                            else
                //                            {
                //                                //hide prices
                //                                priceModel.OldPrice = null;
                //                                priceModel.Price = null;
                //                            }
                //                        }
                //                        break;
                //                }

                //                #endregion
                //            }
                //            break;
                //        case ProductType.SimpleProduct:
                //        default:
                //            {
                //                #region Simple product

                //                //add to cart button
                //                priceModel.DisableBuyButton = product.DisableBuyButton || 
                //                    !_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart) ||
                //                    !_permissionService.Authorize(StandardPermissionProvider.DisplayPrices);

                //                //add to wishlist button
                //                priceModel.DisableWishlistButton = product.DisableWishlistButton || 
                //                    !_permissionService.Authorize(StandardPermissionProvider.EnableWishlist) ||
                //                    !_permissionService.Authorize(StandardPermissionProvider.DisplayPrices);
                //                //pre-order
                //                if (product.AvailableForPreOrder)
                //                {
                //                    priceModel.AvailableForPreOrder = !product.PreOrderAvailabilityStartDateTimeUtc.HasValue ||
                //                        product.PreOrderAvailabilityStartDateTimeUtc.Value >= DateTime.UtcNow;
                //                    priceModel.PreOrderAvailabilityStartDateTimeUtc = product.PreOrderAvailabilityStartDateTimeUtc;
                //                }

                //                //prices
                //                if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                //                {
                //                    //calculate for the maximum quantity (in case if we have tier prices)
                //                    decimal minPossiblePrice = _priceCalculationService.GetFinalPrice(product,
                //                        _workContext.CurrentCustomer, decimal.Zero, true, int.MaxValue);
                //                    if (!product.CustomerEntersPrice)
                //                    {
                //                        if (product.CallForPrice)
                //                        {
                //                            //call for price
                //                            priceModel.OldPrice = null;
                //                            priceModel.Price = _localizationService.GetResource("Products.CallForPrice");
                //                        }
                //                        else
                //                        {
                //                            //calculate prices
                //                            decimal taxRate = decimal.Zero;
                //                            decimal oldPriceBase = _taxService.GetProductPrice(product, product.OldPrice, out taxRate);
                //                            decimal finalPriceBase = _taxService.GetProductPrice(product, minPossiblePrice, out taxRate);

                //                            decimal oldPrice = _currencyService.ConvertFromPrimaryStoreCurrency(oldPriceBase, _workContext.WorkingCurrency);
                //                            decimal finalPrice = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceBase, _workContext.WorkingCurrency);

                //                            //do we have tier prices configured?
                //                            var tierPrices = new List<TierPrice>();
                //                            if (product.HasTierPrices)
                //                            {
                //                                tierPrices.AddRange(product.TierPrices
                //                                    .OrderBy(tp => tp.Quantity)
                //                                    .ToList()
                //                                    .FilterByStore(_storeContext.CurrentStore.Id)
                //                                    .FilterForCustomer(_workContext.CurrentCustomer)
                //                                    .RemoveDuplicatedQuantities());
                //                            }
                //                            //When there is just one tier (with  qty 1), 
                //                            //there are no actual savings in the list.
                //                            bool displayFromMessage = tierPrices.Count > 0 && 
                //                                !(tierPrices.Count == 1 && tierPrices[0].Quantity <= 1);
                //                            if (displayFromMessage)
                //                            {
                //                                priceModel.OldPrice = null;
                //                                priceModel.Price = String.Format(_localizationService.GetResource("Products.PriceRangeFrom"), _priceFormatter.FormatPrice(finalPrice));
                //                            }
                //                            else
                //                            {
                //                                if (finalPriceBase != oldPriceBase && oldPriceBase != decimal.Zero)
                //                                {
                //                                    priceModel.OldPrice = _priceFormatter.FormatPrice(oldPrice);
                //                                    priceModel.Price = _priceFormatter.FormatPrice(finalPrice);
                //                                }
                //                                else
                //                                {
                //                                    priceModel.OldPrice = null;
                //                                    priceModel.Price = _priceFormatter.FormatPrice(finalPrice);
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    //hide prices
                //                    priceModel.OldPrice = null;
                //                    priceModel.Price = null;
                //                }

                //                #endregion
                //            }
                //            break;
                //    }

                //    model.ProductPrice = priceModel;

                //    #endregion
                //}

                //picture
                if (preparePictureModel)
                {
                    #region Prepare product picture

                    //If a size has been set in the view, we use it in priority
                    int pictureSize = productThumbPictureSize.HasValue ? productThumbPictureSize.Value : _mediaSettings.ProductThumbPictureSize;
                    //prepare picture model
                    var defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DEFAULTPICTURE_MODEL_KEY, product.Id, pictureSize, true, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
                    model.DefaultPictureModel = _cacheManager.Get(defaultProductPictureCacheKey, () =>
                    {
                        var picture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                        var pictureModel = new PictureModel()
                        {
                            ImageUrl = _pictureService.GetPictureUrl(picture, pictureSize),
                            FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                            Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), model.Name),
                            AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat"), model.Name)
                        };
                        return pictureModel;
                    });

                    #endregion
                }

                //specs
                if (prepareSpecificationAttributes)
                {
                    model.SpecificationAttributeModels = PrepareProductSpecificationModel(product);
                }
                if (product.ProductCategories.Count > 0)
                {
                    model.CateName = product.ProductCategories.FirstOrDefault().Category.Name;
                }

                //reviews
                // model.ReviewOverviewModel = PrepareProductReviewOverviewModel(product);

                models.Add(model);
            }
            return models;
        }

        [NonAction]
        protected IList<ProductSpecificationModel> PrepareProductSpecificationModel(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            string cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_SPECS_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id);
            return _cacheManager.Get(cacheKey, () =>
            {
                var model = _specificationAttributeService.GetProductSpecificationAttributesByProductId(product.Id, null, true)
                   .Select(psa =>
                   {
                       return new ProductSpecificationModel()
                       {
                           SpecificationAttributeId = psa.SpecificationAttributeOption.SpecificationAttributeId,
                           SpecificationAttributeName = psa.SpecificationAttributeOption.SpecificationAttribute.GetLocalized(x => x.Name),
                           SpecificationAttributeOption = !String.IsNullOrEmpty(psa.CustomValue) ? psa.CustomValue : psa.SpecificationAttributeOption.GetLocalized(x => x.Name),
                       };
                   }).ToList();
                return model;
            });
        }

        [NonAction]
        protected ProductDetailsModel PrepareProductDetailsPageModel(Product product,
            ShoppingCartItem updatecartitem = null, bool isAssociatedProduct = false)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            #region Standard properties

            var model = new ProductDetailsModel()
            {
                Id = product.Id,
                Name = product.GetLocalized(x => x.Name),
                ShortDescription = product.GetLocalized(x => x.ShortDescription),
                FullDescription = product.GetLocalized(x => x.FullDescription),
                MetaKeywords = product.GetLocalized(x => x.MetaKeywords),
                MetaDescription = product.GetLocalized(x => x.MetaDescription),
                MetaTitle = product.GetLocalized(x => x.MetaTitle),
                SeName = product.GetSeName(),
                ShowSku = _catalogSettings.ShowProductSku,
                Sku = product.Sku,
                ShowManufacturerPartNumber = _catalogSettings.ShowManufacturerPartNumber,
                FreeShippingNitificationEnabled = _catalogSettings.ShowFreeShippingNotification,
                ManufacturerPartNumber = product.ManufacturerPartNumber,
                ShowGtin = _catalogSettings.ShowGtin,
                Gtin = product.Gtin,
                StockAvailability = product.FormatStockMessage(_localizationService),
                HasSampleDownload = product.IsDownload && product.HasSampleDownload,
                IsCurrentCustomerRegistered = _workContext.CurrentCustomer.IsRegistered(),
                Area = product.Area.ToString("#"),
                ContactEmail = product.ContactEmail,
                ContactName = product.ContactName,
                ContactPhone = product.ContactPhone,
                FullAddress = product.FullAddress,
                CustomerId = product.CustomerId,
                ProductStatusText = _localizationService.GetResource("Product.StatusText." + product.ProductStatusText),
                AreaUse=product.AreaUse,
               Width=product.Width,
               Dept=product.Height



            };
            if (product.ProductCategories.Count > 0)
            {
                model.CateName = product.ProductCategories.FirstOrDefault().Category.Name;
            }
            if (product.District != null)
                model.DistrictName = product.District.Name;
            #endregion
            //vendor
            if (_vendorSettings.ShowVendorOnProductDetailsPage)
            {
                var vendor = _vendorService.GetVendorById(product.VendorId);
                if (vendor != null && vendor.Active)
                {
                    model.ShowVendor = true;
                    model.VendorModel.Id = vendor.Id;
                    model.VendorModel.Name = vendor.Name;
                    model.VendorModel.SeName = SeoExtensions.GetSeName(vendor.Name);
                }
            }
            #region SPA
            model.Facilities = product.ProductSpecificationAttributes.Where(x => x.SpecificationAttributeOption.SpecificationAttributeId == (int)ProductAttributeEnum.CoSoVatChat)
                .Select(x => x.SpecificationAttributeOption)
                .ToSelectList(x => x.Name, x => x.Id.ToString());
            model.Environments = product.ProductSpecificationAttributes.Where(x => x.SpecificationAttributeOption.SpecificationAttributeId == (int)ProductAttributeEnum.Enviroment)
               .Select(x => x.SpecificationAttributeOption)
               .ToSelectList(x => x.Name, x => x.Id.ToString());
            model.BedRooms = GetOptionName(product, ProductAttributeEnum.NumberOfBedRoom);
            model.BadRooms = GetOptionName(product, ProductAttributeEnum.NumberOfBadRoom);
            model.NumberFloors = GetOptionName(product, ProductAttributeEnum.NumberOfFloor);
            model.Status = GetOptionName(product, ProductAttributeEnum.Status);
            model.Directors = GetOptionName(product, ProductAttributeEnum.Director);
            model.PhapLy = GetOptionName(product, ProductAttributeEnum.PhapLy);
            

            #endregion

            #region Templates

            var templateCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_TEMPLATE_MODEL_KEY, product.ProductTemplateId);
            model.ProductTemplateViewPath = _cacheManager.Get(templateCacheKey, () =>
            {
                var template = _productTemplateService.GetProductTemplateById(product.ProductTemplateId);
                if (template == null)
                    template = _productTemplateService.GetAllProductTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                return template.ViewPath;
            });

            #endregion

            #region Pictures

            model.DefaultPictureZoomEnabled = _mediaSettings.DefaultPictureZoomEnabled;
            //default picture
            var defaultPictureSize =
                _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage;
            var defaultPictureFullSize = _mediaSettings.ProductDetailsPictureSize;
            //prepare picture models
            var productPicturesCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DETAILS_PICTURES_MODEL_KEY, product.Id, defaultPictureSize, defaultPictureFullSize, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
            var cachedPictures = _cacheManager.Get(productPicturesCacheKey, () =>
            {
                var pictures = _pictureService.GetPicturesByProductId(product.Id);

                var defaultPictureModel = new PictureModel()
                {
                    ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(), defaultPictureSize, !isAssociatedProduct),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(),defaultPictureFullSize, !isAssociatedProduct),
                    Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat.Details"), model.Name),
                    AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat.Details"), model.Name),
                };
                //all pictures
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    pictureModels.Add(new PictureModel()
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, defaultPictureSize),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture, defaultPictureFullSize),
                        Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat.Details"), model.Name),
                        AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat.Details"), model.Name),
                    });
                }

                return new { DefaultPictureModel = defaultPictureModel, PictureModels = pictureModels };
            });
            model.DefaultPictureModel = cachedPictures.DefaultPictureModel;
            model.PictureModels = cachedPictures.PictureModels;

            #endregion

            #region Product price

            model.ProductPrice.ProductId = product.Id;
            if (product.CallForPrice)
            {
                model.ProductPrice.Price = "Thỏa thuận";

            }
            else
            {
                model.ProductPrice.Price = ReturnPriceString(product.Price, "vnđ");
            }
            //model.ProductPrice.DynamicPriceUpdate = _catalogSettings.EnableDynamicPriceUpdate;
            //if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            //{
            //    model.ProductPrice.HidePrices = false;
            //    if (product.CustomerEntersPrice)
            //    {
            //        model.ProductPrice.CustomerEntersPrice = true;
            //    }
            //    else
            //    {
            //        if (product.CallForPrice)
            //        {
            //            model.ProductPrice.CallForPrice = true;
            //        }
            //        else
            //        {
            //            decimal taxRate = decimal.Zero;
            //            decimal oldPriceBase = _taxService.GetProductPrice(product, product.OldPrice, out taxRate);
            //            decimal finalPriceWithoutDiscountBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, false), out taxRate);
            //            decimal finalPriceWithDiscountBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, true), out taxRate);

            //            decimal oldPrice = _currencyService.ConvertFromPrimaryStoreCurrency(oldPriceBase, _workContext.WorkingCurrency);
            //            decimal finalPriceWithoutDiscount = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceWithoutDiscountBase, _workContext.WorkingCurrency);
            //            decimal finalPriceWithDiscount = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceWithDiscountBase, _workContext.WorkingCurrency);

            //            if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
            //                model.ProductPrice.OldPrice = _priceFormatter.FormatPrice(oldPrice);

            //            model.ProductPrice.Price = _priceFormatter.FormatPrice(finalPriceWithoutDiscount);

            //            if (finalPriceWithoutDiscountBase != finalPriceWithDiscountBase)
            //                model.ProductPrice.PriceWithDiscount = _priceFormatter.FormatPrice(finalPriceWithDiscount);

            //            model.ProductPrice.PriceValue = finalPriceWithoutDiscount;
            //            model.ProductPrice.PriceWithDiscountValue = finalPriceWithDiscount;

            //            //currency code
            //            model.ProductPrice.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
            //        }
            //    }
            //}
            //else
            //{
            //    model.ProductPrice.HidePrices = true;
            //    model.ProductPrice.OldPrice = null;
            //    model.ProductPrice.Price = null;
            //}
            #endregion



            return model;
        }
        private string GetOptionName(Product p, ProductAttributeEnum att)
        {
            if (p.ProductSpecificationAttributes.Count == 0)
                return "";
            var deffaulAttr = p.ProductSpecificationAttributes.Where(x => x.SpecificationAttributeOption.SpecificationAttributeId == (int)att)
               .Select(x => x.SpecificationAttributeOption)
               .FirstOrDefault();
            if (deffaulAttr == null)
                return "";
            return deffaulAttr.Name;
        }
        [NonAction]
        protected ProductReviewOverviewModel PrepareProductReviewOverviewModel(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var model = new ProductReviewOverviewModel()
            {
                ProductId = product.Id,
                RatingSum = product.ApprovedRatingSum,
                TotalReviews = product.ApprovedTotalReviews,
                AllowCustomerReviews = product.AllowCustomerReviews
            };
            return model;
        }

        [NonAction]
        protected void PrepareProductReviewsModel(ProductReviewsModel model, Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            if (model == null)
                throw new ArgumentNullException("model");

            model.ProductId = product.Id;
            model.ProductName = product.GetLocalized(x => x.Name);
            model.ProductSeName = product.GetSeName();

            var productReviews = product.ProductReviews.Where(pr => pr.IsApproved).OrderBy(pr => pr.CreatedOnUtc);
            foreach (var pr in productReviews)
            {
                var customer = pr.Customer;
                model.Items.Add(new ProductReviewModel()
                {
                    Id = pr.Id,
                    CustomerId = pr.CustomerId,
                    CustomerName = customer.FormatUserName(),
                    AllowViewingProfiles = _customerSettings.AllowViewingProfiles && customer != null && !customer.IsGuest(),
                    Title = pr.Title,
                    ReviewText = pr.ReviewText,
                    Rating = pr.Rating,
                    Helpfulness = new ProductReviewHelpfulnessModel()
                    {
                        ProductReviewId = pr.Id,
                        HelpfulYesTotal = pr.HelpfulYesTotal,
                        HelpfulNoTotal = pr.HelpfulNoTotal,
                    },
                    WrittenOnStr = _dateTimeHelper.ConvertToUserTime(pr.CreatedOnUtc, DateTimeKind.Utc).ToString("g"),
                });
            }

            model.AddProductReview.CanCurrentCustomerLeaveReview = _catalogSettings.AllowAnonymousUsersToReviewProduct || !_workContext.CurrentCustomer.IsGuest();
            model.AddProductReview.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnProductReviewPage;
        }

        #endregion

        #region Categories

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Category(int categoryId, SearchModel searchModel, CatalogPagingFilteringModel command, int streetId = 0, int wardId = 0, int districtId = 0, int stateProvinceId = 0)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            if (category == null || category.Deleted)
                return InvokeHttp404();

            //Check whether the current user has a "Manage catalog" permission
            //It allows him to preview a category before publishing
            if (!category.Published && !_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return InvokeHttp404();

            //ACL (access control list)
            if (!_aclService.Authorize(category))
                return InvokeHttp404();

            //Store mapping
            if (!_storeMappingService.Authorize(category))
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.LastContinueShoppingPage,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            if (command.PageNumber <= 0) command.PageNumber = 1;

            var model = category.ToModel();




            //sorting
            model.PagingFilteringContext.AllowProductSorting = _catalogSettings.AllowProductSorting;
            if (model.PagingFilteringContext.AllowProductSorting)
            {
                foreach (ProductSortingEnum enumValue in Enum.GetValues(typeof(ProductSortingEnum)))
                {
                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "orderby=" + ((int)enumValue).ToString(), null);

                    var sortValue = enumValue.GetLocalizedEnum(_localizationService, _workContext);
                    model.PagingFilteringContext.AvailableSortOptions.Add(new SelectListItem()
                    {
                        Text = sortValue,
                        Value = sortUrl,
                        Selected = enumValue == (ProductSortingEnum)command.OrderBy
                    });
                }
            }



            //view mode
            model.PagingFilteringContext.AllowProductViewModeChanging = _catalogSettings.AllowProductViewModeChanging;
            var viewMode = !string.IsNullOrEmpty(command.ViewMode)
                ? command.ViewMode
                : _catalogSettings.DefaultViewMode;
            if (model.PagingFilteringContext.AllowProductViewModeChanging)
            {
                var currentPageUrl = _webHelper.GetThisPageUrl(true);
                //grid
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Categories.ViewMode.Grid"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=grid", null),
                    Selected = viewMode == "grid"
                });
                //list
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Categories.ViewMode.List"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=list", null),
                    Selected = viewMode == "list"
                });
            }

            //page size
            model.PagingFilteringContext.AllowCustomersToSelectPageSize = false;
            if (category.AllowCustomersToSelectPageSize && category.PageSizeOptions != null)
            {
                var pageSizes = category.PageSizeOptions.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (pageSizes.Any())
                {
                    // get the first page size entry to use as the default (category page load) or if customer enters invalid value via query string
                    if (command.PageSize <= 0 || !pageSizes.Contains(command.PageSize.ToString()))
                    {
                        int temp = 0;

                        if (int.TryParse(pageSizes.FirstOrDefault(), out temp))
                        {
                            if (temp > 0)
                            {
                                command.PageSize = temp;
                            }
                        }
                    }

                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "pagesize={0}", null);
                    sortUrl = _webHelper.RemoveQueryString(sortUrl, "pagenumber");

                    foreach (var pageSize in pageSizes)
                    {
                        int temp = 0;
                        if (!int.TryParse(pageSize, out temp))
                        {
                            continue;
                        }
                        if (temp <= 0)
                        {
                            continue;
                        }

                        model.PagingFilteringContext.PageSizeOptions.Add(new SelectListItem()
                        {
                            Text = pageSize,
                            Value = String.Format(sortUrl, pageSize),
                            Selected = pageSize.Equals(command.PageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        });
                    }

                    if (model.PagingFilteringContext.PageSizeOptions.Any())
                    {
                        model.PagingFilteringContext.PageSizeOptions = model.PagingFilteringContext.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();
                        model.PagingFilteringContext.AllowCustomersToSelectPageSize = true;

                        if (command.PageSize <= 0)
                        {
                            command.PageSize = int.Parse(model.PagingFilteringContext.PageSizeOptions.FirstOrDefault().Text);
                        }
                    }
                }
            }
            else
            {
                //customer is not allowed to select a page size
                command.PageSize = category.PageSize;
            }

            if (command.PageSize <= 0) command.PageSize = category.PageSize;


            //price ranges
            model.PagingFilteringContext.PriceRangeFilter.LoadPriceRangeFilters(category.PriceRanges, _webHelper, _priceFormatter);
            var selectedPriceRange = model.PagingFilteringContext.PriceRangeFilter.GetSelectedPriceRange(_webHelper, category.PriceRanges);
            decimal? minPriceConverted = null;
            decimal? maxPriceConverted = null;
            if (selectedPriceRange != null)
            {
                if (selectedPriceRange.From.HasValue)
                    minPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(selectedPriceRange.From.Value, _workContext.WorkingCurrency);

                if (selectedPriceRange.To.HasValue)
                    maxPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(selectedPriceRange.To.Value, _workContext.WorkingCurrency);
            }





            //category breadcrumb
            model.DisplayCategoryBreadcrumb = _catalogSettings.CategoryBreadcrumbEnabled;
            if (model.DisplayCategoryBreadcrumb)
            {
                foreach (var catBr in category.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService))
                {
                    model.CategoryBreadcrumb.Add(new CategoryModel()
                    {
                        Id = catBr.Id,
                        Name = catBr.GetLocalized(x => x.Name),
                        SeName = catBr.GetSeName()
                    });
                }
            }



            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();



            //subcategories
            //We cache whether we have subcategories
            IList<Category> subcategories = null;
            string hasSubcategoriesCacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_HAS_SUBCATEGORIES_KEY, categoryId,
                string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var hasSubcategoriesCache = _cacheManager.Get<bool?>(hasSubcategoriesCacheKey);
            if (!hasSubcategoriesCache.HasValue)
            {
                subcategories = _categoryService.GetAllCategoriesByParentCategoryId(categoryId);
                hasSubcategoriesCache = subcategories.Count > 0;
                _cacheManager.Set(hasSubcategoriesCacheKey, hasSubcategoriesCache, 60);
            }
            if (hasSubcategoriesCache.Value && subcategories == null)
            {
                subcategories = _categoryService.GetAllCategoriesByParentCategoryId(categoryId);
            }
            if (subcategories != null)
            {
                model.SubCategories = subcategories
                .Select(x =>
                {
                    var subCatName = x.GetLocalized(y => y.Name);
                    var subCatModel = new CategoryModel.SubCategoryModel()
                    {
                        Id = x.Id,
                        Name = subCatName,
                        SeName = x.GetSeName(),
                    };

                    //prepare picture model
                    int pictureSize = _mediaSettings.CategoryThumbPictureSize;
                    var categoryPictureCacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_PICTURE_MODEL_KEY, x.Id, pictureSize, true, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
                    subCatModel.PictureModel = _cacheManager.Get(categoryPictureCacheKey, () =>
                    {
                        var pictureModel = new PictureModel()
                        {
                            FullSizeImageUrl = _pictureService.GetPictureUrl(x.PictureId),
                            ImageUrl = _pictureService.GetPictureUrl(x.PictureId, pictureSize),
                            Title = string.Format(_localizationService.GetResource("Media.Category.ImageLinkTitleFormat"), subCatName),
                            AlternateText = string.Format(_localizationService.GetResource("Media.Category.ImageAlternateTextFormat"), subCatName)
                        };
                        return pictureModel;
                    });

                    return subCatModel;
                })
                .ToList();
            }



            //featured products

            if (!_catalogSettings.IgnoreFeaturedProducts)
            {
                //We cache whether we have featured products
                IPagedList<Product> featuredProducts = null;
                string cacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_HAS_FEATURED_PRODUCTS_KEY, categoryId,
                    string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
                var hasFeaturedProductsCache = _cacheManager.Get<bool?>(cacheKey);
                if (!hasFeaturedProductsCache.HasValue)
                {
                    featuredProducts = _productService.SearchProducts(
                       categoryIds: new List<int>() { category.Id },
                       storeId: _storeContext.CurrentStore.Id,
                       visibleIndividuallyOnly: true,
                       featuredProducts: true);
                    hasFeaturedProductsCache = featuredProducts.TotalCount > 0;
                    _cacheManager.Set(cacheKey, hasFeaturedProductsCache, 60);
                }
                if (hasFeaturedProductsCache.Value && featuredProducts == null)
                {
                    featuredProducts = _productService.SearchProducts(
                       categoryIds: new List<int>() { category.Id },
                       storeId: _storeContext.CurrentStore.Id,
                       visibleIndividuallyOnly: true,
                       featuredProducts: true);
                }
                if (featuredProducts != null)
                {
                    model.FeaturedProducts = PrepareProductOverviewModels(featuredProducts).ToList();
                }
            }


            var categoryIds = new List<int>();
            categoryIds.Add(category.Id);            
            if (_catalogSettings.ShowProductsFromSubcategories)
            {
                //include subcategories
                categoryIds.AddRange(GetChildCategoryIds(category.Id));
            }
            //products
            IList<int> alreadyFilteredSpecOptionIds = model.PagingFilteringContext.SpecificationFilter.GetAlreadyFilteredSpecOptionIds(_webHelper);
            
            IList<int> filterableSpecificationAttributeOptionIds = null;
            var products = _productService.SearchProducts(out filterableSpecificationAttributeOptionIds, true,
                categoryIds: categoryIds,
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                featuredProducts: _catalogSettings.IncludeFeaturedProductsInNormalLists ? null : (bool?)false,
                priceMin: minPriceConverted, priceMax: maxPriceConverted,
                filteredSpecs: alreadyFilteredSpecOptionIds,
                dictrictIds:new List<int>{districtId},
                wardId:wardId,
                stateProvinceId:stateProvinceId,
                streetId:streetId,
                orderBy: (ProductSortingEnum)command.OrderBy,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);
            model.Products = PrepareProductOverviewModels(products).ToList();

            model.PagingFilteringContext.LoadPagedList(products);
            model.PagingFilteringContext.ViewMode = viewMode;

            //specs
            model.PagingFilteringContext.SpecificationFilter.PrepareSpecsFilters(alreadyFilteredSpecOptionIds,
                filterableSpecificationAttributeOptionIds,
                _specificationAttributeService, _webHelper, _workContext);


            //template
            var templateCacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_TEMPLATE_MODEL_KEY, category.CategoryTemplateId);
            var templateViewPath = _cacheManager.Get(templateCacheKey, () =>
            {
                var template = _categoryTemplateService.GetCategoryTemplateById(category.CategoryTemplateId);
                if (template == null)
                    template = _categoryTemplateService.GetAllCategoryTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                return template.ViewPath;
            });

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewCategory", _localizationService.GetResource("ActivityLog.PublicStore.ViewCategory"), category.Name);

            return View(templateViewPath, model);
        }

        [ChildActionOnly]
        public ActionResult CategoryNavigation(int currentCategoryId, int currentProductId)
        {
            //get active category
            var activeCategory = _categoryService.GetCategoryById(currentCategoryId);
            if (activeCategory == null && currentProductId > 0)
            {
                var productCategories = _categoryService.GetProductCategoriesByProductId(currentProductId);
                if (productCategories.Count > 0)
                    activeCategory = productCategories[0].Category;
            }
            var activeCategoryId = activeCategory != null ? activeCategory.Id : 0;

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_NAVIGATION_MODEL_KEY, _workContext.WorkingLanguage.Id,
                string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id, activeCategoryId);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var breadCrumb = activeCategory != null ?
                    activeCategory.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService)
                    .Select(x => x.Id).ToList()
                    : new List<int>();
                return PrepareCategorySimpleModels(0, breadCrumb, 0, int.MaxValue, false).ToList();
            });

            var model = new CategoryNavigationModel()
            {
                CurrentCategoryId = activeCategoryId,
                Categories = cachedModel
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_MENU_MODEL_KEY, _workContext.WorkingLanguage.Id,
                string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                return PrepareCategorySimpleModels(0, null, 0, _catalogSettings.TopCategoryMenuSubcategoryLevelsToDisplay, true).ToList();
            });

            var model = new TopMenuModel()
            {
                Categories = cachedModel,
                RecentlyAddedProductsEnabled = _catalogSettings.RecentlyAddedProductsEnabled,
                BlogEnabled = _blogSettings.Enabled,
                ForumEnabled = _forumSettings.ForumsEnabled,
                Districts = _stateProvinceService.GetDistHCM().OrderBy(x => x.DisplayOrder).ToSelectList(x => x.Name, x => x.GetSeName())
            };
            model.CategoriesNews = _catenewsService.GetAllCategories().OrderBy(x => x.DisplayOrder).ToSelectList(x => x.Name, x => x.GetSeName());
            model.Topics = _topicService.GetAllTopics(0, 1).Select(x => new SelectListItem { Text = x.Title, Value = x.SystemName }).ToList();
            return PartialView(model);
        }
        
        [ChildActionOnly]
        public ActionResult HomepageCategories()
        {
            var categories = _categoryService.GetAllCategoriesDisplayedOnHomePage()
                .Where(c => _aclService.Authorize(c) && _storeMappingService.Authorize(c))
                .ToList();

            var listModel = categories
                .Select(x =>
                {
                    var catModel = x.ToModel();

                    //prepare picture model
                    int pictureSize = _mediaSettings.CategoryThumbPictureSize;
                    var categoryPictureCacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_PICTURE_MODEL_KEY, x.Id, pictureSize, true, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
                    catModel.PictureModel = _cacheManager.Get(categoryPictureCacheKey, () =>
                    {
                        var pictureModel = new PictureModel()
                        {
                            FullSizeImageUrl = _pictureService.GetPictureUrl(x.PictureId),
                            ImageUrl = _pictureService.GetPictureUrl(x.PictureId, pictureSize),
                            Title = string.Format(_localizationService.GetResource("Media.Category.ImageLinkTitleFormat"), catModel.Name),
                            AlternateText = string.Format(_localizationService.GetResource("Media.Category.ImageAlternateTextFormat"), catModel.Name)
                        };
                        return pictureModel;
                    });

                    return catModel;
                })
                .ToList();

            return PartialView(listModel);
        }

        #endregion

        #region Manufacturers

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Manufacturer(int manufacturerId, CatalogPagingFilteringModel command)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(manufacturerId);
            if (manufacturer == null || manufacturer.Deleted)
                return InvokeHttp404();

            //Check whether the current user has a "Manage catalog" permission
            //It allows him to preview a manufacturer before publishing
            if (!manufacturer.Published && !_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                return InvokeHttp404();

            //ACL (access control list)
            if (!_aclService.Authorize(manufacturer))
                return InvokeHttp404();

            //Store mapping
            if (!_storeMappingService.Authorize(manufacturer))
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.LastContinueShoppingPage,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            if (command.PageNumber <= 0) command.PageNumber = 1;

            var model = manufacturer.ToModel();




            //sorting
            model.PagingFilteringContext.AllowProductSorting = _catalogSettings.AllowProductSorting;
            if (model.PagingFilteringContext.AllowProductSorting)
            {
                foreach (ProductSortingEnum enumValue in Enum.GetValues(typeof(ProductSortingEnum)))
                {
                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "orderby=" + ((int)enumValue).ToString(), null);

                    var sortValue = enumValue.GetLocalizedEnum(_localizationService, _workContext);
                    model.PagingFilteringContext.AvailableSortOptions.Add(new SelectListItem()
                    {
                        Text = sortValue,
                        Value = sortUrl,
                        Selected = enumValue == (ProductSortingEnum)command.OrderBy
                    });
                }
            }



            //view mode
            model.PagingFilteringContext.AllowProductViewModeChanging = _catalogSettings.AllowProductViewModeChanging;
            var viewMode = !string.IsNullOrEmpty(command.ViewMode)
                ? command.ViewMode
                : _catalogSettings.DefaultViewMode;
            if (model.PagingFilteringContext.AllowProductViewModeChanging)
            {
                var currentPageUrl = _webHelper.GetThisPageUrl(true);
                //grid
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Manufacturers.ViewMode.Grid"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=grid", null),
                    Selected = viewMode == "grid"
                });
                //list
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Manufacturers.ViewMode.List"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=list", null),
                    Selected = viewMode == "list"
                });
            }

            //page size
            model.PagingFilteringContext.AllowCustomersToSelectPageSize = false;
            if (manufacturer.AllowCustomersToSelectPageSize && manufacturer.PageSizeOptions != null)
            {
                var pageSizes = manufacturer.PageSizeOptions.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (pageSizes.Any())
                {
                    // get the first page size entry to use as the default (manufacturer page load) or if customer enters invalid value via query string
                    if (command.PageSize <= 0 || !pageSizes.Contains(command.PageSize.ToString()))
                    {
                        int temp = 0;

                        if (int.TryParse(pageSizes.FirstOrDefault(), out temp))
                        {
                            if (temp > 0)
                            {
                                command.PageSize = temp;
                            }
                        }
                    }

                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "pagesize={0}", null);
                    sortUrl = _webHelper.RemoveQueryString(sortUrl, "pagenumber");

                    foreach (var pageSize in pageSizes)
                    {
                        int temp = 0;
                        if (!int.TryParse(pageSize, out temp))
                        {
                            continue;
                        }
                        if (temp <= 0)
                        {
                            continue;
                        }

                        model.PagingFilteringContext.PageSizeOptions.Add(new SelectListItem()
                        {
                            Text = pageSize,
                            Value = String.Format(sortUrl, pageSize),
                            Selected = pageSize.Equals(command.PageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        });
                    }

                    model.PagingFilteringContext.PageSizeOptions = model.PagingFilteringContext.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();

                    if (model.PagingFilteringContext.PageSizeOptions.Any())
                    {
                        model.PagingFilteringContext.PageSizeOptions = model.PagingFilteringContext.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();
                        model.PagingFilteringContext.AllowCustomersToSelectPageSize = true;

                        if (command.PageSize <= 0)
                        {
                            command.PageSize = int.Parse(model.PagingFilteringContext.PageSizeOptions.FirstOrDefault().Text);
                        }
                    }
                }
            }
            else
            {
                //customer is not allowed to select a page size
                command.PageSize = manufacturer.PageSize;
            }

            if (command.PageSize <= 0) command.PageSize = manufacturer.PageSize;


            //price ranges
            model.PagingFilteringContext.PriceRangeFilter.LoadPriceRangeFilters(manufacturer.PriceRanges, _webHelper, _priceFormatter);
            var selectedPriceRange = model.PagingFilteringContext.PriceRangeFilter.GetSelectedPriceRange(_webHelper, manufacturer.PriceRanges);
            decimal? minPriceConverted = null;
            decimal? maxPriceConverted = null;
            if (selectedPriceRange != null)
            {
                if (selectedPriceRange.From.HasValue)
                    minPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(selectedPriceRange.From.Value, _workContext.WorkingCurrency);

                if (selectedPriceRange.To.HasValue)
                    maxPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(selectedPriceRange.To.Value, _workContext.WorkingCurrency);
            }



            //featured products
            if (!_catalogSettings.IgnoreFeaturedProducts)
            {
                IPagedList<Product> featuredProducts = null;

                //We cache whether we have featured products
                var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                    .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
                string cacheKey = string.Format(ModelCacheEventConsumer.MANUFACTURER_HAS_FEATURED_PRODUCTS_KEY, manufacturerId,
                    string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
                var hasFeaturedProductsCache = _cacheManager.Get<bool?>(cacheKey);
                if (!hasFeaturedProductsCache.HasValue)
                {
                    featuredProducts = _productService.SearchProducts(
                       manufacturerId: manufacturer.Id,
                       storeId: _storeContext.CurrentStore.Id,
                       visibleIndividuallyOnly: true,
                       featuredProducts: true);
                    hasFeaturedProductsCache = featuredProducts.TotalCount > 0;
                    _cacheManager.Set(cacheKey, hasFeaturedProductsCache, 60);
                }
                if (hasFeaturedProductsCache.Value && featuredProducts == null)
                {
                    featuredProducts = _productService.SearchProducts(
                       manufacturerId: manufacturer.Id,
                       storeId: _storeContext.CurrentStore.Id,
                       visibleIndividuallyOnly: true,
                       featuredProducts: true);
                }
                if (featuredProducts != null)
                {
                    model.FeaturedProducts = PrepareProductOverviewModels(featuredProducts).ToList();
                }
            }



            //products
            IList<int> filterableSpecificationAttributeOptionIds = null;
            var products = _productService.SearchProducts(out filterableSpecificationAttributeOptionIds, true,
                manufacturerId: manufacturer.Id,
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                featuredProducts: _catalogSettings.IncludeFeaturedProductsInNormalLists ? null : (bool?)false,
                priceMin: minPriceConverted,
                priceMax: maxPriceConverted,
                orderBy: (ProductSortingEnum)command.OrderBy,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);
            model.Products = PrepareProductOverviewModels(products).ToList();

            model.PagingFilteringContext.LoadPagedList(products);
            model.PagingFilteringContext.ViewMode = viewMode;


            //template
            var templateCacheKey = string.Format(ModelCacheEventConsumer.MANUFACTURER_TEMPLATE_MODEL_KEY, manufacturer.ManufacturerTemplateId);
            var templateViewPath = _cacheManager.Get(templateCacheKey, () =>
            {
                var template = _manufacturerTemplateService.GetManufacturerTemplateById(manufacturer.ManufacturerTemplateId);
                if (template == null)
                    template = _manufacturerTemplateService.GetAllManufacturerTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                return template.ViewPath;
            });

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewManufacturer", _localizationService.GetResource("ActivityLog.PublicStore.ViewManufacturer"), manufacturer.Name);

            return View(templateViewPath, model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ManufacturerAll()
        {
            var model = new List<ManufacturerModel>();
            var manufacturers = _manufacturerService.GetAllManufacturers();
            foreach (var manufacturer in manufacturers)
            {
                var modelMan = manufacturer.ToModel();

                //prepare picture model
                int pictureSize = _mediaSettings.ManufacturerThumbPictureSize;
                var manufacturerPictureCacheKey = string.Format(ModelCacheEventConsumer.MANUFACTURER_PICTURE_MODEL_KEY, manufacturer.Id, pictureSize, true, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
                modelMan.PictureModel = _cacheManager.Get(manufacturerPictureCacheKey, () =>
                {
                    var pictureModel = new PictureModel()
                    {
                        FullSizeImageUrl = _pictureService.GetPictureUrl(manufacturer.PictureId),
                        ImageUrl = _pictureService.GetPictureUrl(manufacturer.PictureId, pictureSize),
                        Title = string.Format(_localizationService.GetResource("Media.Manufacturer.ImageLinkTitleFormat"), modelMan.Name),
                        AlternateText = string.Format(_localizationService.GetResource("Media.Manufacturer.ImageAlternateTextFormat"), modelMan.Name)
                    };
                    return pictureModel;
                });
                model.Add(modelMan);
            }

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ManufacturerNavigation(int currentManufacturerId)
        {
            if (_catalogSettings.ManufacturersBlockItemsToDisplay == 0)
                return Content("");

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.MANUFACTURER_NAVIGATION_MODEL_KEY, currentManufacturerId, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var currentManufacturer = _manufacturerService.GetManufacturerById(currentManufacturerId);

                var manufacturers = _manufacturerService.GetAllManufacturers(pageSize: _catalogSettings.ManufacturersBlockItemsToDisplay);
                var model = new ManufacturerNavigationModel()
                {
                    TotalManufacturers = manufacturers.TotalCount
                };

                foreach (var manufacturer in manufacturers)
                {
                    var modelMan = new ManufacturerBriefInfoModel()
                    {
                        Id = manufacturer.Id,
                        Name = manufacturer.GetLocalized(x => x.Name),
                        SeName = manufacturer.GetSeName(),
                        IsActive = currentManufacturer != null && currentManufacturer.Id == manufacturer.Id,
                    };
                    model.Manufacturers.Add(modelMan);
                }
                return model;
            });

            return PartialView(cacheModel);
        }

        #endregion

        #region Vendors

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Vendor(int vendorId, CatalogPagingFilteringModel command)
        {
            var vendor = _vendorService.GetVendorById(vendorId);
            if (vendor == null || vendor.Deleted)
                return InvokeHttp404();

            //Vendor is active?
            if (!vendor.Active)
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.LastContinueShoppingPage,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            if (command.PageNumber <= 0) command.PageNumber = 1;

            var model = new VendorModel()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Description = vendor.Description,
                SeName = SeoExtensions.GetSeName(vendor.Name)
            };




            //sorting
            model.PagingFilteringContext.AllowProductSorting = _catalogSettings.AllowProductSorting;
            if (model.PagingFilteringContext.AllowProductSorting)
            {
                foreach (ProductSortingEnum enumValue in Enum.GetValues(typeof(ProductSortingEnum)))
                {
                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "orderby=" + ((int)enumValue).ToString(), null);

                    var sortValue = enumValue.GetLocalizedEnum(_localizationService, _workContext);
                    model.PagingFilteringContext.AvailableSortOptions.Add(new SelectListItem()
                    {
                        Text = sortValue,
                        Value = sortUrl,
                        Selected = enumValue == (ProductSortingEnum)command.OrderBy
                    });
                }
            }



            //view mode
            model.PagingFilteringContext.AllowProductViewModeChanging = _catalogSettings.AllowProductViewModeChanging;
            var viewMode = !string.IsNullOrEmpty(command.ViewMode)
                ? command.ViewMode
                : _catalogSettings.DefaultViewMode;
            if (model.PagingFilteringContext.AllowProductViewModeChanging)
            {
                var currentPageUrl = _webHelper.GetThisPageUrl(true);
                //grid
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Vendors.ViewMode.Grid"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=grid", null),
                    Selected = viewMode == "grid"
                });
                //list
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Vendors.ViewMode.List"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=list", null),
                    Selected = viewMode == "list"
                });
            }

            //page size
            model.PagingFilteringContext.AllowCustomersToSelectPageSize = false;
            if (_vendorSettings.AllowCustomersToSelectPageSize && _vendorSettings.PageSizeOptions != null)
            {
                var pageSizes = _vendorSettings.PageSizeOptions.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (pageSizes.Any())
                {
                    // get the first page size entry to use as the default (manufacturer page load) or if customer enters invalid value via query string
                    if (command.PageSize <= 0 || !pageSizes.Contains(command.PageSize.ToString()))
                    {
                        int temp = 0;

                        if (int.TryParse(pageSizes.FirstOrDefault(), out temp))
                        {
                            if (temp > 0)
                            {
                                command.PageSize = temp;
                            }
                        }
                    }

                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "pagesize={0}", null);
                    sortUrl = _webHelper.RemoveQueryString(sortUrl, "pagenumber");

                    foreach (var pageSize in pageSizes)
                    {
                        int temp = 0;
                        if (!int.TryParse(pageSize, out temp))
                        {
                            continue;
                        }
                        if (temp <= 0)
                        {
                            continue;
                        }

                        model.PagingFilteringContext.PageSizeOptions.Add(new SelectListItem()
                        {
                            Text = pageSize,
                            Value = String.Format(sortUrl, pageSize),
                            Selected = pageSize.Equals(command.PageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        });
                    }

                    model.PagingFilteringContext.PageSizeOptions = model.PagingFilteringContext.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();

                    if (model.PagingFilteringContext.PageSizeOptions.Any())
                    {
                        model.PagingFilteringContext.PageSizeOptions = model.PagingFilteringContext.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();
                        model.PagingFilteringContext.AllowCustomersToSelectPageSize = true;

                        if (command.PageSize <= 0)
                        {
                            command.PageSize = int.Parse(model.PagingFilteringContext.PageSizeOptions.FirstOrDefault().Text);
                        }
                    }
                }
            }
            else
            {
                //customer is not allowed to select a page size
                command.PageSize = _vendorSettings.PageSize;
            }

            if (command.PageSize <= 0) command.PageSize = _vendorSettings.PageSize;


            //products
            IList<int> filterableSpecificationAttributeOptionIds = null;
            var products = _productService.SearchProducts(out filterableSpecificationAttributeOptionIds, true,
                vendorId: vendor.Id,
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                featuredProducts: null,
                priceMin: null,
                priceMax: null,
                orderBy: (ProductSortingEnum)command.OrderBy,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);
            model.Products = PrepareProductOverviewModels(products).ToList();

            model.PagingFilteringContext.LoadPagedList(products);
            model.PagingFilteringContext.ViewMode = viewMode;

            return View(model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult VendorAll()
        {
            //we don't allow viewing of vendors if "vendors" block is hidden
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return RedirectToRoute("HomePage");

            var model = new List<VendorModel>();
            var vendors = _vendorService.GetAllVendors();
            foreach (var vendor in vendors)
            {
                var vendorModel = new VendorModel()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    Description = vendor.Description,
                    SeName = SeoExtensions.GetSeName(vendor.Name)
                };
                model.Add(vendorModel);
            }

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult VendorNavigation()
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            string cacheKey = ModelCacheEventConsumer.MANUFACTURER_NAVIGATION_MODEL_KEY;
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var vendors = _vendorService.GetAllVendors(pageSize: _vendorSettings.VendorsBlockItemsToDisplay);
                var model = new VendorNavigationModel()
                {
                    TotalVendors = vendors.TotalCount
                };

                foreach (var vendor in vendors)
                {
                    model.Vendors.Add(new VendorBriefInfoModel()
                    {
                        Id = vendor.Id,
                        Name = vendor.Name,
                        SeName = SeoExtensions.GetSeName(vendor.Name),
                    });
                }
                return model;
            });

            return PartialView(cacheModel);
        }

        #endregion

        #region Products

        //product details page
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Product(int productId, int updatecartitemid = 0)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                return InvokeHttp404();

            //Is published?
            //Check whether the current user has a "Manage catalog" permission
            //It allows him to preview a product before publishing
            if (!product.Published && !_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return InvokeHttp404();

            //ACL (access control list)
            if (!_aclService.Authorize(product))
                return InvokeHttp404();

            //Store mapping
            if (!_storeMappingService.Authorize(product))
                return InvokeHttp404();

            //visible individually?
            if (!product.VisibleIndividually)
            {
                //is this one an associated products?
                var parentGroupedProduct = _productService.GetProductById(product.ParentGroupedProductId);
                if (parentGroupedProduct != null)
                {
                    return RedirectToRoute("Product", new { SeName = parentGroupedProduct.GetSeName() });
                }
                else
                {
                    return RedirectToRoute("HomePage");
                }
            }

            //update existing shopping cart item?
            ShoppingCartItem updatecartitem = null;
            if (_shoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
            {
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .Where(x => x.StoreId == _storeContext.CurrentStore.Id)
                    .ToList();
                updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);
                //not found?
                if (updatecartitem == null)
                {
                    return RedirectToRoute("Product", new { SeName = product.GetSeName() });
                }
                //is it this product?
                if (product.Id != updatecartitem.ProductId)
                {
                    return RedirectToRoute("Product", new { SeName = product.GetSeName() });
                }
            }

            //prepare the model
            var model = PrepareProductDetailsPageModel(product,null, false);

            //save as recently viewed
            _recentlyViewedProductsService.AddProductToRecentlyViewedList(product.Id);

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewProduct", _localizationService.GetResource("ActivityLog.PublicStore.ViewProduct"), product.Name);

            return View(model.ProductTemplateViewPath, model);
        }




        [ChildActionOnly]
        public ActionResult ProductBreadcrumb(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            if (!_catalogSettings.CategoryBreadcrumbEnabled)
                return Content("");

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            var cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_BREADCRUMB_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = new ProductDetailsModel.ProductBreadcrumbModel()
                {
                    ProductId = product.Id,
                    ProductName = product.GetLocalized(x => x.Name),
                    ProductSeName = product.GetSeName()
                };
                var productCategories = _categoryService.GetProductCategoriesByProductId(product.Id);
                if (productCategories.Count > 0)
                {
                    var category = productCategories[0].Category;
                    if (category != null)
                    {
                        foreach (var catBr in category.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService))
                        {
                            model.CategoryBreadcrumb.Add(new CategoryModel()
                            {
                                Id = catBr.Id,
                                Name = catBr.GetLocalized(x => x.Name),
                                SeName = catBr.GetSeName()
                            });
                        }
                    }
                }
                return model;
            });

            return PartialView(cacheModel);
        }

        [ChildActionOnly]
        public ActionResult ProductManufacturers(int productId)
        {
            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_MANUFACTURERS_MODEL_KEY, productId, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = _manufacturerService.GetProductManufacturersByProductId(productId)
                    .Select(x =>
                    {
                        var m = x.Manufacturer.ToModel();
                        return m;
                    })
                    .ToList();
                return model;
            });

            return PartialView(cacheModel);
        }

        [ChildActionOnly]
        public ActionResult ProductReviewOverview(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            var model = PrepareProductReviewOverviewModel(product);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ProductSpecifications(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            var model = PrepareProductSpecificationModel(product);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ProductTierPrices(int productId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                return Content(""); //hide prices

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            if (!product.HasTierPrices)
                return Content(""); //no tier prices

            var model = product.TierPrices
                .OrderBy(x => x.Quantity)
                .ToList()
                .FilterByStore(_storeContext.CurrentStore.Id)
                .FilterForCustomer(_workContext.CurrentCustomer)
                .RemoveDuplicatedQuantities()
                .Select(tierPrice =>
                {
                    var m = new ProductDetailsModel.TierPriceModel()
                    {
                        Quantity = tierPrice.Quantity,
                    };
                    decimal taxRate = decimal.Zero;
                    decimal priceBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, _workContext.CurrentCustomer, decimal.Zero, _catalogSettings.DisplayTierPricesWithDiscounts, tierPrice.Quantity), out taxRate);
                    decimal price = _currencyService.ConvertFromPrimaryStoreCurrency(priceBase, _workContext.WorkingCurrency);
                    m.Price = _priceFormatter.FormatPrice(price, false, false);
                    return m;
                })
                .ToList();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult RelatedProducts(int productId, int? productThumbPictureSize)
        {
            var products = new List<Product>();
            var relatedProducts = _productService
                .GetRelatedProductsByProductId1(productId);
            foreach (var product in _productService.GetProductsByIds(relatedProducts.Select(x => x.ProductId2).ToArray()))
            {
                //ensure has ACL permission and appropriate store mapping
                if (_aclService.Authorize(product) && _storeMappingService.Authorize(product))
                    products.Add(product);
            }
            var model = PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ProductsAlsoPurchased(int productId, int? productThumbPictureSize)
        {
            if (!_catalogSettings.ProductsAlsoPurchasedEnabled)
                return Content("");

            //load and cache report
            var productIds = _cacheManager.Get(string.Format(ModelCacheEventConsumer.PRODUCTS_ALSO_PURCHASED_IDS_KEY, productId, _storeContext.CurrentStore.Id),
                () =>
                    _orderReportService
                    .GetProductsAlsoPurchasedById(_storeContext.CurrentStore.Id, productId, _catalogSettings.ProductsAlsoPurchasedNumber)
                    .Select(x => x.Id)
                    .ToArray()
                    );

            //load products
            var products = _productService.GetProductsByIds(productIds);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //prepare model
            var model = PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();

            return PartialView(model);
        }


        public ActionResult ShareButton()
        {
            if (_catalogSettings.ShowShareButton && !String.IsNullOrEmpty(_catalogSettings.PageShareCode))
            {
                var shareCode = _catalogSettings.PageShareCode;
                if (_webHelper.IsCurrentConnectionSecured())
                {
                    //need to change the addthis link to be https linked when the page is, so that the page doesnt ask about mixed mode when viewed in https...
                    shareCode = shareCode.Replace("http://", "https://");
                }

                return PartialView("ShareButton", shareCode);
            }

            return Content("");
        }

        [ChildActionOnly]
        public ActionResult CrossSellProducts(int? productThumbPictureSize)
        {
            var cart = _workContext.CurrentCustomer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .Where(sci => sci.StoreId == _storeContext.CurrentStore.Id)
                .ToList();

            var products = _productService.GetCrosssellProductsByShoppingCart(cart, _shoppingCartSettings.CrossSellsNumber);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();


            //Cross-sell products are dispalyed on the shopping cart page.
            //We know that the entire shopping cart page is not refresh
            //even if "ShoppingCartSettings.DisplayCartAfterAddingProduct" setting  is enabled.
            //That's why we force page refresh (redirect) in this case
            var model = PrepareProductOverviewModels(products,
                productThumbPictureSize: productThumbPictureSize, forceRedirectionAfterAddingToCart: true)
                .ToList();

            return PartialView(model);
        }

        //recently viewed products
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult RecentlyViewedProducts()
        {
            var model = new List<ProductOverviewModel>();
            if (_catalogSettings.RecentlyViewedProductsEnabled)
            {
                var products = _recentlyViewedProductsService.GetRecentlyViewedProducts(_catalogSettings.RecentlyViewedProductsNumber);
                model.AddRange(PrepareProductOverviewModels(products));
            }
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult RecentlyViewedProductsBlock(int? productThumbPictureSize, bool? preparePriceModel)
        {
            var model = new List<ProductOverviewModel>();
            if (_catalogSettings.RecentlyViewedProductsEnabled)
            {
                var preparePictureModel = productThumbPictureSize.HasValue;
                var products = _recentlyViewedProductsService.GetRecentlyViewedProducts(_catalogSettings.RecentlyViewedProductsNumber);

                //ACL and store mapping
                products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

                //prepare model
                model.AddRange(PrepareProductOverviewModels(products,
                    preparePriceModel.HasValue ? preparePriceModel.Value : false,
                    preparePictureModel,
                    productThumbPictureSize));
            }
            return PartialView(model);
        }

        //recently added products
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult RecentlyAddedProducts()
        {
            var model = new List<ProductOverviewModel>();
            if (_catalogSettings.RecentlyAddedProductsEnabled)
            {
                var products = _productService.SearchProducts(
                    storeId: _storeContext.CurrentStore.Id,
                       visibleIndividuallyOnly: true,
                    orderBy: ProductSortingEnum.CreatedOn,
                    pageSize: _catalogSettings.RecentlyAddedProductsNumber);
                model.AddRange(PrepareProductOverviewModels(products));
            }
            return View(model);
        }

        public ActionResult RecentlyAddedProductsRss()
        {
            var feed = new SyndicationFeed(
                                    string.Format("{0}: Recently added products", _storeContext.CurrentStore.GetLocalized(x => x.Name)),
                                    "Information about products",
                                    new Uri(_webHelper.GetStoreLocation(false)),
                                    "RecentlyAddedProductsRSS",
                                    DateTime.UtcNow);

            if (!_catalogSettings.RecentlyAddedProductsEnabled)
                return new RssActionResult() { Feed = feed };

            var items = new List<SyndicationItem>();

            var products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                orderBy: ProductSortingEnum.CreatedOn,
                pageSize: _catalogSettings.RecentlyAddedProductsNumber);
            foreach (var product in products)
            {
                string productUrl = Url.RouteUrl("Product", new { SeName = product.GetSeName() }, "http");
                string productName = product.GetLocalized(x => x.Name);
                string productDescription = product.GetLocalized(x => x.ShortDescription);
                var item = new SyndicationItem(productName, productDescription, new Uri(productUrl), String.Format("RecentlyAddedProduct:{0}", product.Id), product.CreatedOnUtc);
                items.Add(item);
                //uncomment below if you want to add RSS enclosure for pictures
                //var picture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                //if (picture != null)
                //{
                //    var imageUrl = _pictureService.GetPictureUrl(picture, _mediaSettings.ProductDetailsPictureSize);
                //    item.ElementExtensions.Add(new XElement("enclosure", new XAttribute("type", "image/jpeg"), new XAttribute("url", imageUrl)).CreateReader());
                //}

            }
            feed.Items = items;
            return new RssActionResult() { Feed = feed };
        }

        [ChildActionOnly]
        public ActionResult HomepageBestSellers(int? productThumbPictureSize)
        {
            if (!_catalogSettings.ShowBestsellersOnHomepage || _catalogSettings.NumberOfBestsellersOnHomepage == 0)
                return Content("");

            //load and cache report
            var report = _cacheManager.Get(string.Format(ModelCacheEventConsumer.HOMEPAGE_BESTSELLERS_IDS_KEY, _storeContext.CurrentStore.Id),
                () =>
                    _orderReportService.BestSellersReport(storeId: _storeContext.CurrentStore.Id,
                    pageSize: _catalogSettings.NumberOfBestsellersOnHomepage));


            //load products
            var products = _productService.GetProductsByIds(report.Select(x => x.ProductId).ToArray());
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //prepare model
            var model = PrepareProductOverviewModels(products, true, true, productThumbPictureSize)
                .ToList();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult HomepageProducts(int? productThumbPictureSize)
        {
            var products = _productService.GetAllProductsDisplayedOnHomePage();
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

            var model = PrepareProductOverviewModels(products, true, true, productThumbPictureSize)
                .ToList();

            return PartialView(model);
        }

        public ActionResult BackInStockSubscribePopup(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                throw new ArgumentException("No product found with the specified id");

            var model = new BackInStockSubscribeModel();
            model.ProductId = product.Id;
            model.ProductName = product.GetLocalized(x => x.Name);
            model.ProductSeName = product.GetSeName();
            model.IsCurrentCustomerRegistered = _workContext.CurrentCustomer.IsRegistered();
            model.MaximumBackInStockSubscriptions = _catalogSettings.MaximumBackInStockSubscriptions;
            model.CurrentNumberOfBackInStockSubscriptions = _backInStockSubscriptionService
                .GetAllSubscriptionsByCustomerId(_workContext.CurrentCustomer.Id, _storeContext.CurrentStore.Id, 0, 1)
                .TotalCount;
            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                product.StockQuantity <= 0)
            {
                //out of stock
                model.SubscriptionAllowed = true;
                model.AlreadySubscribed = _backInStockSubscriptionService
                    .FindSubscription(_workContext.CurrentCustomer.Id, product.Id, _storeContext.CurrentStore.Id) != null;
            }
            return View(model);
        }

        [HttpPost, ActionName("BackInStockSubscribePopup")]
        public ActionResult BackInStockSubscribePopupPOST(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                throw new ArgumentException("No product found with the specified id");

            if (!_workContext.CurrentCustomer.IsRegistered())
                return Content(_localizationService.GetResource("BackInStockSubscriptions.OnlyRegistered"));

            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                product.StockQuantity <= 0)
            {
                //out of stock
                var subscription = _backInStockSubscriptionService
                    .FindSubscription(_workContext.CurrentCustomer.Id, product.Id, _storeContext.CurrentStore.Id);
                if (subscription != null)
                {
                    //unsubscribe
                    _backInStockSubscriptionService.DeleteSubscription(subscription);
                    return Content("Unsubscribed");
                }
                else
                {
                    if (_backInStockSubscriptionService
                        .GetAllSubscriptionsByCustomerId(_workContext.CurrentCustomer.Id, _storeContext.CurrentStore.Id, 0, 1)
                        .TotalCount >= _catalogSettings.MaximumBackInStockSubscriptions)
                        return Content(string.Format(_localizationService.GetResource("BackInStockSubscriptions.MaxSubscriptions"), _catalogSettings.MaximumBackInStockSubscriptions));

                    //subscribe   
                    subscription = new BackInStockSubscription()
                    {
                        Customer = _workContext.CurrentCustomer,
                        Product = product,
                        StoreId = _storeContext.CurrentStore.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    _backInStockSubscriptionService.InsertSubscription(subscription);
                    return Content("Subscribed");
                }

            }
            else
            {
                return Content(_localizationService.GetResource("BackInStockSubscriptions.NotAllowed"));
            }
        }

        #endregion

        #region Product tags

        //Product tags
        [ChildActionOnly]
        public ActionResult ProductTags(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("No product found with the specified id");

            var cacheKey = string.Format(ModelCacheEventConsumer.PRODUCTTAG_BY_PRODUCT_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = product.ProductTags
                    //filter by store
                    .Where(x => _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id) > 0)
                    .Select(x =>
                    {
                        var ptModel = new ProductTagModel()
                        {
                            Id = x.Id,
                            Name = x.GetLocalized(y => y.Name),
                            SeName = x.GetSeName(),
                            ProductCount = _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id)
                        };
                        return ptModel;
                    })
                    .ToList();
                return model;
            });

            return PartialView(cacheModel);
        }

        [ChildActionOnly]
        public ActionResult PopularProductTags()
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.PRODUCTTAG_POPULAR_MODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = new PopularProductTagsModel();

                //get all tags
                var allTags = _productTagService
                    .GetAllProductTags()
                    //filter by current store
                    .Where(x => _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id) > 0)
                    //order by product count
                    .OrderByDescending(x => _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id))
                    .ToList();

                var tags = allTags
                    .Take(_catalogSettings.NumberOfProductTags)
                    .ToList();
                //sorting
                tags = tags.OrderBy(x => x.GetLocalized(y => y.Name)).ToList();

                model.TotalTags = allTags.Count;

                foreach (var tag in tags)
                    model.Tags.Add(new ProductTagModel()
                    {
                        Id = tag.Id,
                        Name = tag.GetLocalized(y => y.Name),
                        SeName = tag.GetSeName(),
                        ProductCount = _productTagService.GetProductCount(tag.Id, _storeContext.CurrentStore.Id)
                    });
                return model;
            });

            return PartialView(cacheModel);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ProductsByTag(int productTagId, CatalogPagingFilteringModel command)
        {
            var productTag = _productTagService.GetProductTagById(productTagId);
            if (productTag == null)
                return InvokeHttp404();

            if (command.PageNumber <= 0) command.PageNumber = 1;

            var model = new ProductsByTagModel()
            {
                Id = productTag.Id,
                TagName = productTag.GetLocalized(y => y.Name)
            };


            //sorting
            model.PagingFilteringContext.AllowProductSorting = _catalogSettings.AllowProductSorting;
            if (model.PagingFilteringContext.AllowProductSorting)
            {
                foreach (ProductSortingEnum enumValue in Enum.GetValues(typeof(ProductSortingEnum)))
                {
                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "orderby=" + ((int)enumValue).ToString(), null);

                    var sortValue = enumValue.GetLocalizedEnum(_localizationService, _workContext);
                    model.PagingFilteringContext.AvailableSortOptions.Add(new SelectListItem()
                    {
                        Text = sortValue,
                        Value = sortUrl,
                        Selected = enumValue == (ProductSortingEnum)command.OrderBy
                    });
                }
            }


            //view mode
            model.PagingFilteringContext.AllowProductViewModeChanging = _catalogSettings.AllowProductViewModeChanging;
            var viewMode = !string.IsNullOrEmpty(command.ViewMode)
                ? command.ViewMode
                : _catalogSettings.DefaultViewMode;
            if (model.PagingFilteringContext.AllowProductViewModeChanging)
            {
                var currentPageUrl = _webHelper.GetThisPageUrl(true);
                //grid
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Categories.ViewMode.Grid"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=grid", null),
                    Selected = viewMode == "grid"
                });
                //list
                model.PagingFilteringContext.AvailableViewModes.Add(new SelectListItem()
                {
                    Text = _localizationService.GetResource("Categories.ViewMode.List"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=list", null),
                    Selected = viewMode == "list"
                });
            }

            //page size
            model.PagingFilteringContext.AllowCustomersToSelectPageSize = false;
            if (_catalogSettings.ProductsByTagAllowCustomersToSelectPageSize && _catalogSettings.ProductsByTagPageSizeOptions != null)
            {
                var pageSizes = _catalogSettings.ProductsByTagPageSizeOptions.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (pageSizes.Any())
                {
                    // get the first page size entry to use as the default ('products by tag' page load) or if customer enters invalid value via query string
                    if (command.PageSize <= 0 || !pageSizes.Contains(command.PageSize.ToString()))
                    {
                        int temp = 0;

                        if (int.TryParse(pageSizes.FirstOrDefault(), out temp))
                        {
                            if (temp > 0)
                            {
                                command.PageSize = temp;
                            }
                        }
                    }

                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "pagesize={0}", null);
                    sortUrl = _webHelper.RemoveQueryString(sortUrl, "pagenumber");

                    foreach (var pageSize in pageSizes)
                    {
                        int temp = 0;
                        if (!int.TryParse(pageSize, out temp))
                        {
                            continue;
                        }
                        if (temp <= 0)
                        {
                            continue;
                        }

                        model.PagingFilteringContext.PageSizeOptions.Add(new SelectListItem()
                        {
                            Text = pageSize,
                            Value = String.Format(sortUrl, pageSize),
                            Selected = pageSize.Equals(command.PageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        });
                    }

                    if (model.PagingFilteringContext.PageSizeOptions.Any())
                    {
                        model.PagingFilteringContext.PageSizeOptions = model.PagingFilteringContext.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();
                        model.PagingFilteringContext.AllowCustomersToSelectPageSize = true;

                        if (command.PageSize <= 0)
                        {
                            command.PageSize = int.Parse(model.PagingFilteringContext.PageSizeOptions.FirstOrDefault().Text);
                        }
                    }
                }
            }
            else
            {
                //customer is not allowed to select a page size
                command.PageSize = _catalogSettings.ProductsByTagPageSize;
            }

            if (command.PageSize <= 0) command.PageSize = _catalogSettings.ProductsByTagPageSize;

            //products
            var products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                productTagId: productTag.Id,
                visibleIndividuallyOnly: true,
                orderBy: (ProductSortingEnum)command.OrderBy,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);
            model.Products = PrepareProductOverviewModels(products).ToList();

            model.PagingFilteringContext.LoadPagedList(products);
            model.PagingFilteringContext.ViewMode = viewMode;
            return View(model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ProductTagsAll()
        {
            var model = new PopularProductTagsModel();
            model.Tags = _productTagService
                .GetAllProductTags()
                //filter by current store
                .Where(x => _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id) > 0)
                //sort by name
                .OrderBy(x => x.GetLocalized(y => y.Name))
                .Select(x =>
                {
                    var ptModel = new ProductTagModel()
                    {
                        Id = x.Id,
                        Name = x.GetLocalized(y => y.Name),
                        SeName = x.GetSeName(),
                        ProductCount = _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id)
                    };
                    return ptModel;
                })
                .ToList();
            return View(model);
        }

        #endregion

        #region Product reviews

        //products reviews
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ProductReviews(int productId)
        {

            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted || !product.Published || !product.AllowCustomerReviews)
                return RedirectToRoute("HomePage");

            var model = new ProductReviewsModel();

            PrepareProductReviewsModel(model, product);
            if (!_workContext.CurrentCustomer.IsGuest())
                model.AddProductReview.CustomerName = _workContext.CurrentCustomer.GetFullName();
            //only registered users can leave reviews
            if (_workContext.CurrentCustomer.IsGuest() && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
                ModelState.AddModelError("", _localizationService.GetResource("Reviews.OnlyRegisteredUsersCanWriteReviews"));
            //default value
            model.AddProductReview.Rating = _catalogSettings.DefaultProductRatingValue;
            return View(model);
        }

        public ActionResult GetProductReview(int productId, int pageIndex = 0, int pageSize = 10)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted || !product.Published || !product.AllowCustomerReviews)
                return Json("Comment not valid!");
            if (_workContext.CurrentCustomer.IsGuest() && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
                return Json("NOT LOGIN");
            var model = product.ProductReviews.Where(x => x.ParentId == 0).Select(pr =>
            {
                var item = PreparingReviewOverrideModel(pr);
                item.Childrens = product.ProductReviews.Where(x => x.ParentId == pr.Id).Select(ch =>
                {
                    return PreparingReviewOverrideModel(ch);
                }).ToList();
                return item;
            }).AsQueryable();
            return View(new PagedList<ProductReviewModel>(model, pageIndex, pageSize));





        }
        private ProductReviewModel PreparingReviewOverrideModel(ProductReview pr)
        {

            var model = new ProductReviewModel
            {
                Id = pr.Id,
                CustomerId = pr.CustomerId,
                CustomerName = pr.CustomerName,
                CustomerEmail = pr.CustomerEmail,
                AllowViewingProfiles = _customerSettings.AllowViewingProfiles && pr.Customer != null && !pr.Customer.IsGuest(),
                Title = pr.Title,
                ReviewText = pr.ReviewText,
                Rating = pr.Rating,
                Helpfulness = new ProductReviewHelpfulnessModel()
                {
                    ProductReviewId = pr.Id,
                    HelpfulYesTotal = pr.HelpfulYesTotal,
                    HelpfulNoTotal = pr.HelpfulNoTotal,
                },
                WrittenOnStr = _dateTimeHelper.ConvertToUserTime(pr.CreatedOnUtc, DateTimeKind.Utc).ToString("g"),
            };
            return model;
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult ProductReviewsAdd(int productId, ProductReviewsModel model, bool captchaValid)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted || !product.Published || !product.AllowCustomerReviews)
                return Json("Sản phẩm đã bị xóa!");

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnProductReviewPage && !captchaValid)
            {
                return Json(_localizationService.GetResource("Common.WrongCaptcha"));
            }

            if (_workContext.CurrentCustomer.IsGuest() && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
            {
                return Json(_localizationService.GetResource("Reviews.OnlyRegisteredUsersCanWriteReviews"));
            }

            if (ModelState.IsValid)
            {
                //save review
                int rating = model.AddProductReview.Rating;
                if (rating < 1 || rating > 5)
                    rating = _catalogSettings.DefaultProductRatingValue;
                bool isApproved = !_catalogSettings.ProductReviewsMustBeApproved;

                var productReview = new ProductReview()
                {
                    ProductId = product.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    Title = model.AddProductReview.Title,
                    ReviewText = model.AddProductReview.ReviewText,
                    Rating = rating,
                    HelpfulYesTotal = 0,
                    HelpfulNoTotal = 0,
                    IsApproved = isApproved,
                    CreatedOnUtc = DateTime.UtcNow,
                    CustomerName = model.AddProductReview.CustomerName,
                    ParentId = model.AddProductReview.ParentId
                };
                product.ProductReviews.Add(productReview);
                _productService.UpdateProduct(product);

                //update product totals
                _productService.UpdateProductReviewTotals(product);

                //notify store owner
                if (_catalogSettings.NotifyStoreOwnerAboutNewProductReviews)
                    _workflowMessageService.SendProductReviewNotificationMessage(productReview, _localizationSettings.DefaultAdminLanguageId);

                //activity log
                _customerActivityService.InsertActivity("PublicStore.AddProductReview", _localizationService.GetResource("ActivityLog.PublicStore.AddProductReview"), product.Name);


                //  PrepareProductReviewsModel(model, product);
                // model.AddProductReview.Title = null;
                //  model.AddProductReview.ReviewText = null;

                // model.AddProductReview.SuccessfullyAdded = true;
                if (!isApproved)
                    return Json(_localizationService.GetResource("Reviews.SeeAfterApproving"));
                else
                    return Json("OK");


            }
            return Json("Error!");
        }

        [HttpPost]
        public ActionResult SetProductReviewHelpfulness(int productReviewId, bool washelpful)
        {
            var productReview = _productService.GetProductReviewById(productReviewId);
            if (productReview == null)
                throw new ArgumentException("No product review found with the specified id");

            if (_workContext.CurrentCustomer.IsGuest() && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
            {
                return Json(new
                {
                    Result = _localizationService.GetResource("Reviews.Helpfulness.OnlyRegistered"),
                    TotalYes = productReview.HelpfulYesTotal,
                    TotalNo = productReview.HelpfulNoTotal
                });
            }

            //customers aren't allowed to vote for their own reviews
            if (productReview.CustomerId == _workContext.CurrentCustomer.Id)
            {
                return Json(new
                {
                    Result = _localizationService.GetResource("Reviews.Helpfulness.YourOwnReview"),
                    TotalYes = productReview.HelpfulYesTotal,
                    TotalNo = productReview.HelpfulNoTotal
                });
            }

            //delete previous helpfulness
            var prh = productReview.ProductReviewHelpfulnessEntries
                .FirstOrDefault(x => x.CustomerId == _workContext.CurrentCustomer.Id);
            if (prh != null)
            {
                //existing one
                prh.WasHelpful = washelpful;
            }
            else
            {
                //insert new helpfulness
                prh = new ProductReviewHelpfulness()
                {
                    ProductReviewId = productReview.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    WasHelpful = washelpful,
                };
                productReview.ProductReviewHelpfulnessEntries.Add(prh);
            }
            _productService.UpdateProduct(productReview.Product);

            //new totals
            productReview.HelpfulYesTotal = productReview.ProductReviewHelpfulnessEntries.Count(x => x.WasHelpful);
            productReview.HelpfulNoTotal = productReview.ProductReviewHelpfulnessEntries.Count(x => !x.WasHelpful);
            _productService.UpdateProduct(productReview.Product);

            return Json(new
            {
                Result = _localizationService.GetResource("Reviews.Helpfulness.SuccessfullyVoted"),
                TotalYes = productReview.HelpfulYesTotal,
                TotalNo = productReview.HelpfulNoTotal
            });
        }

        #endregion

        #region Email a friend

        //products email a friend
        [ChildActionOnly]
        public ActionResult ProductEmailAFriendButton(int productId)
        {
            if (!_catalogSettings.EmailAFriendEnabled)
                return Content("");
            var model = new ProductEmailAFriendModel()
            {
                ProductId = productId
            };

            return PartialView("ProductEmailAFriendButton", model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ProductEmailAFriend(int productId, bool isBook = false)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted || !product.Published || !_catalogSettings.EmailAFriendEnabled)
                return RedirectToRoute("HomePage");

            var model = new ProductEmailAFriendModel();
            model.ProductId = product.Id;
            model.ProductName = product.GetLocalized(x => x.Name);
            model.ProductSeName = product.GetSeName();
            model.YourEmailAddress = _workContext.CurrentCustomer.Email;
            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnEmailProductToFriendPage;
            if (isBook)
                return View("BookDate", model);
            return View(model);
        }

        [HttpPost]
        [FormValueRequired("send-email")]
        [CaptchaValidator]
        public ActionResult ProductEmailAFriendSend(ProductEmailAFriendModel model, bool captchaValid)
        {
            DateTime d = new DateTime(model.Year,model.Month,model.Date,model.Hour,model.Minute,0);
            
            var product = _productService.GetProductById(model.ProductId);
            if (product == null || product.Deleted || !product.Published || !_catalogSettings.EmailAFriendEnabled)
                return Json("Lỗi");

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnEmailProductToFriendPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptcha"));
            }

            //check whether the current customer is guest and ia allowed to email a friend
            //if (_workContext.CurrentCustomer.IsGuest() && !_catalogSettings.AllowAnonymousUsersToEmailAFriend)
            //{
            //    ModelState.AddModelError("", _localizationService.GetResource("Products.EmailAFriend.OnlyRegisteredUsers"));
            //}

            if (ModelState.IsValid)
            {
                //email
                if(model.Year>0)
                {
                    var mes = new Message
                    {
                        Email=model.FriendEmail,
                        Body=model.PersonalMessage,
                        CreatedOn=DateTime.Now,
                        BookDate=d,
                        ProductId=model.ProductId,
                        Phone=model.Phone,
                        Name=model.Name,
                        Type=(int)MessageType.Book

                    };
                    _messagesService.InsertMessage(mes);
                    return Json("Gửi thành công");
                    
                }


                _workflowMessageService.SendProductEmailAFriendMessage(_workContext.CurrentCustomer,
                        _workContext.WorkingLanguage.Id, product,
                        model.YourEmailAddress, model.FriendEmail,
                        Core.Html.HtmlHelper.FormatText(model.PersonalMessage, false, true, false, false, false, false));

                model.ProductId = product.Id;
                model.ProductName = product.GetLocalized(x => x.Name);
                model.ProductSeName = product.GetSeName();

                model.SuccessfullySent = true;
                model.Result = _localizationService.GetResource("Products.EmailAFriend.SuccessfullySent");
                return Json("Gửi thành công!");
               // return View(model);
            }

            //If we got this far, something failed, redisplay form
            model.ProductId = product.Id;
            model.ProductName = product.GetLocalized(x => x.Name);
            model.ProductSeName = product.GetSeName();
            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnEmailProductToFriendPage;
            return Json("Gửi thành công!");
        }

        #endregion

        #region Comparing products

        //compare products
        public ActionResult AddProductToCompareList(string productId)
        {
            foreach (var id in productId.Split(','))
            {
                int i = 0;
                if (!int.TryParse(id, out i))
                    continue;
                var product = _productService.GetProductById(i);
                if (product == null || product.Deleted || !product.Published)
                    continue;

                if (!_catalogSettings.CompareProductsEnabled)
                    continue;

                _compareProductsService.AddProductToCompareList(i);

                //activity log
                _customerActivityService.InsertActivity("PublicStore.AddToCompareList", _localizationService.GetResource("ActivityLog.PublicStore.AddToCompareList"), product.Name);
            }
            return RedirectToRoute("CompareProducts");
        }

        public ActionResult RemoveProductFromCompareList(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return RedirectToRoute("HomePage");

            if (!_catalogSettings.CompareProductsEnabled)
                return RedirectToRoute("HomePage");

            _compareProductsService.RemoveProductFromCompareList(productId);

            return RedirectToRoute("CompareProducts");
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult CompareProducts()
        {
            if (!_catalogSettings.CompareProductsEnabled)
                return RedirectToRoute("HomePage");

            var model = new CompareProductsModel()
            {
                IncludeShortDescriptionInCompareProducts = _catalogSettings.IncludeShortDescriptionInCompareProducts,
                IncludeFullDescriptionInCompareProducts = _catalogSettings.IncludeFullDescriptionInCompareProducts,
            };

            var products = _compareProductsService.GetComparedProducts();

            //ACL and store mapping
            // products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

            //prepare model
            PrepareProductOverviewModels(products, prepareSpecificationAttributes: true)
                .ToList()
                .ForEach(model.Products.Add);
            return View(model);
        }

        public ActionResult ClearCompareList()
        {
            if (!_catalogSettings.CompareProductsEnabled)
                return RedirectToRoute("HomePage");

            _compareProductsService.ClearCompareProducts();

            return RedirectToRoute("CompareProducts");
        }

        [ChildActionOnly]
        public ActionResult CompareProductsButton(int productId)
        {
            if (!_catalogSettings.CompareProductsEnabled)
                return Content("");

            var model = new AddToCompareListModel()
            {
                ProductId = productId
            };

            return PartialView("CompareProductsButton", model);
        }

        #endregion

        #region Searching

        [NopHttpsRequirement(SslRequirement.No)]
        [ValidateInput(false)]
        public ActionResult Search(SearchModel model, SearchPagingFilteringModel command, int streetId = 0, int wardId = 0, int districtId = 0, int stateProvinceId = 0, string priceString="", string attributeOptionIds="")
        {
            if (model == null)
                model = new SearchModel();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.LastContinueShoppingPage,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            if (command.PageSize <= 0) command.PageSize = _catalogSettings.SearchPageProductsPerPage;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            if (model.Q == null)
                model.Q = "";
            model.Q = model.Q.Trim();

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();

            string cacheKey = string.Format(ModelCacheEventConsumer.SEARCH_CATEGORIES_MODEL_KEY, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var categories = _cacheManager.Get(cacheKey, () =>
            {
                var categoriesModel = new List<SearchPagingFilteringModel.CategoryModel>();
                //all categories
                foreach (var c in _categoryService.GetAllCategories())
                {
                    //generate full category name (breadcrumb)
                    string categoryBreadcrumb = "";
                    var breadcrumb = c.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService);
                    for (int i = 0; i <= breadcrumb.Count - 1; i++)
                    {
                        categoryBreadcrumb += breadcrumb[i].GetLocalized(x => x.Name);
                        if (i != breadcrumb.Count - 1)
                            categoryBreadcrumb += " >> ";
                    }
                    categoriesModel.Add(new SearchPagingFilteringModel.CategoryModel()
                    {
                        Id = c.Id,
                        Breadcrumb = categoryBreadcrumb
                    });
                }
                return categoriesModel;
            });
            if (categories.Count > 0)
            {
                //first empty entry
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Value = "0",
                    Text = _localizationService.GetResource("Common.All")
                });
                //all other categories
                foreach (var c in categories)
                {
                    model.AvailableCategories.Add(new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Breadcrumb,
                        Selected = model.Cid == c.Id
                    });
                }
            }

            var manufacturers = _manufacturerService.GetAllManufacturers();
            if (manufacturers.Count > 0)
            {
                model.AvailableManufacturers.Add(new SelectListItem()
                {
                    Value = "0",
                    Text = _localizationService.GetResource("Common.All")
                });
                foreach (var m in manufacturers)
                    model.AvailableManufacturers.Add(new SelectListItem()
                    {
                        Value = m.Id.ToString(),
                        Text = m.GetLocalized(x => x.Name),
                        Selected = model.Mid == m.Id
                    });
            }


            IPagedList<Product> products = new PagedList<Product>(new List<Product>(), 0, 1);            // only search if query string search keyword is set (used to avoid searching or displaying search term min length error message on /search page load)

            var categoryIds = new List<int>();
            var selectedOptionIds = new List<int>();
            int manufacturerId = 0;
            decimal? minPriceConverted = null;
            decimal? maxPriceConverted = null;
            bool searchInDescriptions = false;
            if (model.As)
            {
                //advanced search
                var categoryId = model.Cid;
                if (categoryId > 0)
                {
                    categoryIds.Add(categoryId);
                    if (model.Isc)
                    {
                        //include subcategories
                        categoryIds.AddRange(GetChildCategoryIds(categoryId));
                    }
                }


                manufacturerId = model.Mid;
               
                //min price
                if (!string.IsNullOrEmpty(model.Pf))
                {
                    decimal minPrice = decimal.Zero;
                    if (decimal.TryParse(model.Pf, out minPrice))
                        minPriceConverted = minPrice * 1000000;//_currencyService.ConvertToPrimaryStoreCurrency(minPrice, _workContext.WorkingCurrency);
                }
                //max price
                if (!string.IsNullOrEmpty(model.Pt))
                {
                    decimal maxPrice = decimal.Zero;
                    if (decimal.TryParse(model.Pt, out maxPrice))
                        maxPriceConverted = maxPrice * 1000000;//_currencyService.ConvertToPrimaryStoreCurrency(maxPrice, _workContext.WorkingCurrency);
                }
                
                searchInDescriptions = model.Sid;
                                
               
            }else
            {
                if (!String.IsNullOrEmpty(priceString))
                {
                    var p = priceString.Split('-');                   
                    decimal minPrice = decimal.Zero;
                    if (decimal.TryParse(p[0].ToString(), out minPrice))
                        minPriceConverted = minPrice * 1000000;
                    decimal maxPrice = decimal.Zero;
                    if (decimal.TryParse(p[1].ToString(), out maxPrice))
                        maxPriceConverted = maxPrice * 1000000;
                }
                if (!String.IsNullOrEmpty(attributeOptionIds))
                {
                    var options = attributeOptionIds.Split('-');
                    foreach (var option in options)
                    {
                        selectedOptionIds.Add(Int32.Parse(option));
                    }
                }
            }

            //var searchInProductTags = false;
            var searchInProductTags = searchInDescriptions;

            //products
            products = _productService.SearchProducts(
                categoryIds: categoryIds,
                manufacturerId: manufacturerId,
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                priceMin: minPriceConverted,
                priceMax: maxPriceConverted,
                keywords: null,
                searchDescriptions: searchInDescriptions,
                searchSku: searchInDescriptions,
                searchProductTags: searchInProductTags,
                languageId: _workContext.WorkingLanguage.Id,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize,
                filteredSpecs: selectedOptionIds,
                stateProvinceId: stateProvinceId,
                dictrictIds: new List<int> { districtId },
                wardId: wardId,
                streetId: streetId);
            model.Products = PrepareProductOverviewModels(products).ToList();

            model.NoResults = !model.Products.Any();

            //search term statistics
            if (!String.IsNullOrEmpty(model.Q))
            {
                var searchTerm = _searchTermService.GetSearchTermByKeyword(model.Q, _storeContext.CurrentStore.Id);
                if (searchTerm != null)
                {
                    searchTerm.Count++;
                    _searchTermService.UpdateSearchTerm(searchTerm);
                }
                else
                {
                    searchTerm = new SearchTerm()
                    {
                        Keyword = model.Q,
                        StoreId = _storeContext.CurrentStore.Id,
                        Count = 1
                    };
                    _searchTermService.InsertSearchTerm(searchTerm);
                }
            }

            //event
            _eventPublisher.Publish(new ProductSearchEvent()
            {
                SearchTerm = model.Q,
                SearchInDescriptions = searchInDescriptions,
                CategoryIds = categoryIds,
                ManufacturerId = manufacturerId,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            });



            model.PagingFilteringContext.LoadPagedList(products);
            string districtName,cateName,priceName,huong;
            districtName=cateName=priceName=huong="";
            if (model.DistrictId > 0)
                districtName = _stateProvinceService.GetDistHCM().Where(x => x.Id == model.DistrictId).FirstOrDefault().Name;// model.Districts.FirstOrDefault(x => x.Value == model.DistrictId.ToString()).Text;
            if (model.Cid > 0)
                cateName = model.AvailableCategories.FirstOrDefault(x => x.Value == model.Cid.ToString()).Text;
            //if (model.PriceString!="0-0")
            //    priceName = model.PriceRange.FirstOrDefault(x => x.Value == model.PriceRange.ToString()).Text;
            ViewBag.ResultString = string.Format("{0} {1} {2}",cateName,districtName,priceName);
            
            if (Request.IsAjaxRequest())
                return PartialView("_ProductListPartial", model);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult SearchBox()
        {
            var model = new SearchBoxModel()
            {
                AutoCompleteEnabled = _catalogSettings.ProductSearchAutoCompleteEnabled,
                ShowProductImagesInSearchAutoComplete = _catalogSettings.ShowProductImagesInSearchAutoComplete,
                SearchTermMinimumLength = _catalogSettings.ProductSearchTermMinimumLength
            };
            return PartialView(model);
        }

        public ActionResult SearchTermAutoComplete(string term)
        {
            if (String.IsNullOrWhiteSpace(term) || term.Length < _catalogSettings.ProductSearchTermMinimumLength)
                return Content("");

            //products
            var productNumber = _catalogSettings.ProductSearchAutoCompleteNumberOfProducts > 0 ?
                _catalogSettings.ProductSearchAutoCompleteNumberOfProducts : 10;

            var products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                keywords: term,
                searchSku: false,
                languageId: _workContext.WorkingLanguage.Id,
                visibleIndividuallyOnly: true,
                pageSize: productNumber);

            var models = PrepareProductOverviewModels(products, false, _catalogSettings.ShowProductImagesInSearchAutoComplete, _mediaSettings.AutoCompleteSearchThumbPictureSize).ToList();
            var result = (from p in models
                          select new
                          {
                              label = p.Name,
                              producturl = Url.RouteUrl("Product", new { SeName = p.SeName }),
                              productpictureurl = p.DefaultPictureModel.ImageUrl
                          })
                          .ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetSlugFromId(string domainName,string priceString, string attributeOptionIds, int categoryId=0, int stateProvinceId=0, int districtId=0, int wardId=0, int streetId=0)
        {
            if (String.IsNullOrEmpty(domainName)) domainName = Request.Url.Host;
            if (categoryId == 0) categoryId = 1;
            var slug = _urlRecordService.GetSlugFromId(domainName, categoryId, stateProvinceId, districtId, wardId, streetId, priceString, attributeOptionIds);
            return slug;
        }
        #endregion

        #region Insert/Update Product

        public ActionResult InsertProductSuccess(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return HttpNotFound();
            if (_workContext.CurrentCustomer.Id != product.Id)
                return HttpNotFound();
            return View(productId);
        }
        public ActionResult InsertProduct()
        {

            var model = new InsertProductModel();
            PreparingInsertProductModel(model);
            return View(model);


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertProduct(InsertProductModel model, FormCollection form)
        {

            if (model == null)
                throw new Exception("Product is null");
            if (ModelState.IsValid)
            {
                var product = new Product();
                var selectedId = form.GetValues("SelectedOptionAttributes");
                if (selectedId.Length > 0)
                    model.SelectedOptionAttributes = selectedId.Where(x => x != "0").Select(x => int.Parse(x)).ToList();

                var titlePic = form.AllKeys.Where(x => x.Contains("PictureTitle_"));
                foreach (var tit in titlePic)
                {
                    model.PictureIds.Add(new InsertProductModel.PictureUploadModel
                    {
                        Id = int.Parse(tit.Replace("PictureTitle_", "")),
                        Title = form.GetValue(tit).AttemptedValue

                    });
                }        
                    PreapringProductToEntity(model, product);
                product.Price = model.Price * 1000000;
                string seName = product.ValidateSeName(product.Name, product.Name, true);
                _productService.InsertProduct(product);
                _urlRecordService.SaveSlug(product, seName, 0);
                #region Insert Categories
                var productCate = new ProductCategory
                {
                    CategoryId = model.CateId,
                    ProductId = product.Id,
                    IsFeaturedProduct = false


                };
                _categoryService.InsertProductCategory(productCate);
                #endregion

                #region InsertPictures
                foreach (var i in model.PictureIds)
                {
                    var pictureproduct = new ProductPicture
                    {
                        PictureId = i.Id,
                        ProductId = product.Id,
                        Description=i.Title

                    };
                    _productService.InsertProductPicture(pictureproduct);
                    _pictureService.SetSeoFilename(i.Id, _pictureService.GetPictureSeName(product.Name));
                }
                #endregion

                #region Insert SPA
                foreach (var i in model.SelectedOptionAttributes)
                {
                    var SPAProduct = new ProductSpecificationAttribute
                    {
                        ProductId = product.Id,
                        SpecificationAttributeOptionId = i,
                        ShowOnProductPage = false,
                        AllowFiltering = true,

                    };
                    _specificationAttributeService.InsertProductSpecificationAttribute(SPAProduct);
                }
                #endregion

                RedirectToAction("InsertProductSuccess", new { productId = product.Id });
            }
            return View(model);

        }
        public ActionResult EditProduct(int id)
        {

            if (!_workContext.CurrentCustomer.IsAdmin())
                return RedirectToRoute("HomePage");
            var product = _productService.GetProductById(id);
            if (product == null)
                return RedirectToRoute("HomePage");
            var model = ProductToInsertModel(product);
            PreparingInsertProductModel(model);
            PreapringProductToEntity(model, product);

            return View(model);


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProduct(InsertProductModel model, FormCollection form)
        {
            if (!_workContext.CurrentCustomer.IsAdmin())
                return RedirectToRoute("HomePage");

            if (model == null)
                throw new Exception("Product is null");

            var product = _productService.GetProductById(model.Id);
            if (product == null)
                return RedirectToRoute("HomePage");

            if (product.CustomerId != _workContext.CurrentCustomer.Id)
                return RedirectToRoute("HomePage");
            if (ModelState.IsValid)
            {
                PreapringProductToEntity(model, product);
                product.Price = model.Price * 1000000;
                var selectedId = form.GetValues("SelectedOptionAttributes");
                if (selectedId.Length > 0)
                    model.SelectedOptionAttributes = selectedId.Where(x => x != "0").Select(x => int.Parse(x)).ToList();

                var titlePic = form.AllKeys.Where(x=>x.Contains("PictureTitle_"));                
                foreach (var tit in titlePic)
                {
                    model.PictureIds.Add(new InsertProductModel.PictureUploadModel
                    {
                        Id = int.Parse(tit.Replace("PictureTitle_","")),
                        Title = form.GetValue(tit).AttemptedValue

                    });
                }               

                #region Insert Categories
                var cate = product.ProductCategories.FirstOrDefault();
                if (cate != null)
                {
                    cate.CategoryId = model.CateId;
                    _categoryService.UpdateProductCategory(cate);
                }

                #endregion

                #region EditPictures
                //delete

                //add new
                var productPic = product.ProductPictures;
                foreach (var i in model.PictureIds)
                {



                    if (!productPic.All(x => x.PictureId == i.Id))
                    {
                        var pictureproduct = new ProductPicture
                        {
                            PictureId = i.Id,
                            ProductId = product.Id,
                            Description = i.Title

                        };
                        _productService.InsertProductPicture(pictureproduct);
                        _pictureService.SetSeoFilename(i.Id, _pictureService.GetPictureSeName(product.Name));
                    }
                    else
                    {
                        var pictureProduct = productPic.FirstOrDefault(x => x.PictureId == i.Id);
                        pictureProduct.Description = i.Title;
                        _productService.UpdateProductPicture(pictureProduct);
                    }
                }
                #endregion

                #region Edit SPA
                //delete
                var listAttrDelete = product.ProductSpecificationAttributes.Where(x => !model.SelectedOptionAttributes.Contains(x.SpecificationAttributeOptionId)).ToList();
                foreach (var i in listAttrDelete)                {
                    
                    _specificationAttributeService.DeleteProductSpecificationAttribute(i);
                }
                //add new
                foreach (var i in model.SelectedOptionAttributes)
                {
                    if (product.ProductSpecificationAttributes.Where(x => x.SpecificationAttributeOptionId == i).Count() == 0)
                    {
                        var SPAProduct = new ProductSpecificationAttribute
                        {
                            ProductId = product.Id,
                            SpecificationAttributeOptionId = i,
                            ShowOnProductPage = false,
                            AllowFiltering = true,

                        };
                        _specificationAttributeService.InsertProductSpecificationAttribute(SPAProduct);
                    }
                }
                #endregion
                _productService.UpdateProduct(product);
                string seName = product.ValidateSeName(product.Name, product.Name, true);
                _urlRecordService.SaveSlug(product, seName, 0);

                RedirectToAction("InsertProductSuccess", new { productId = product.Id });
            }
            return View(model);

        }

        private void PreapringProductToEntity(InsertProductModel inPd, Product p)
        {


            #region default Properties
            if (p.Id == 0)
            {
                p.Id = inPd.Id;
                p.Price = inPd.Price;
                p.CreatedOnUtc = DateTime.Now;
                p.UpdatedOnUtc = DateTime.Now;
                //p.OriginId = inPd.pOriginId;
                p.AvailableEndDateTimeUtc = null;
                p.AvailableStartDateTimeUtc = null;
                p.SpecialPriceEndDateTimeUtc = null;
                p.SpecialPriceStartDateTimeUtc = null;
                p.UpdatedOnUtc = DateTime.Now;
                p.StartConstructionDate = null;
                p.FinishConstructionDate = null;
                p.ConstructionDate = null;

                //p.ProductStatus = inPd.pPStatus;
                p.ProductTypeId = 5;
                p.ParentGroupedProductId = 0;
                p.VisibleIndividually = true;
                p.ProductTemplateId = 1;
                p.VendorId = 0;
                p.ShowOnHomePage = false;
                p.AllowCustomerReviews = true;
                p.ApprovedRatingSum = 0;
                p.NotApprovedRatingSum = 0;
                p.ApprovedTotalReviews = 0;
                p.NotApprovedTotalReviews = 0;

                p.SubjectToAcl = false;
                p.LimitedToStores = false;
                p.IsGiftCard = false;
                p.GiftCardTypeId = 0;
                p.RequireOtherProducts = false;

                p.AutomaticallyAddRequiredProducts = false;

                p.IsDownload = false;
                p.DownloadId = 0;
                p.UnlimitedDownloads = false;

                p.MaxNumberOfDownloads = 0;
                p.DownloadActivationTypeId = 0;
                p.HasSampleDownload = false;
                p.SampleDownloadId = 0;
                p.HasUserAgreement = false;
                p.IsRecurring = false;
                p.RecurringCycleLength = 0;
                p.RecurringCyclePeriodId = 0;
                p.RecurringTotalCycles = 0;
                p.IsShipEnabled = true;
                p.IsFreeShipping = false;
                p.AdditionalShippingCharge = 0;

                p.IsTaxExempt = false;
                p.TaxCategoryId = 0;
                p.ManageInventoryMethodId = 0;
                p.StockQuantity = 10;
                p.DisplayStockAvailability = false;

                p.DisplayStockQuantity = false;
                p.MinStockQuantity = 0;
                p.OrderMaximumQuantity = 10;
                p.LowStockActivityId = 1;

                p.NotifyAdminForQuantityBelow = 1;
                p.BackorderModeId = 0;
                p.AllowBackInStockSubscriptions = false;
                p.OrderMinimumQuantity = 1;
                p.DisableBuyButton = true;
                p.DisableWishlistButton = false;
                p.AvailableForPreOrder = false;
                p.OldPrice = 0;
                p.ProductCost = 0;
                p.CustomerEntersPrice = false;
                p.MinimumCustomerEnteredPrice = 0;
                p.MaximumCustomerEnteredPrice = 0;

                p.HasTierPrices = false;
                p.HasDiscountsApplied = false;
                p.Weight = 0;
                p.Width = 0;
                p.Height = 0;
                p.Length = 0;
                p.DisplayOrder = 0;
                p.Published = true;
                p.Deleted = false;
            }
            #endregion

            if (!_workContext.CurrentCustomer.IsRegistered())
            {
                p.ContactEmail = inPd.Email;
                p.ContactName = inPd.ContactName;
                p.ContactPhone = inPd.ContactPhone;

            }
            else
            {
                p.ContactEmail = _workContext.CurrentCustomer.Email;
                p.ContactName = _workContext.CurrentCustomer.GetFullName();
                p.ContactPhone = _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);
                var add = _workContext.CurrentCustomer.Addresses.FirstOrDefault();
                if (add != null)
                {
                    p.DistrictId = add.DistrictId ?? 0;
                    p.WardId = add.WardId ?? 0;
                    p.StreetId = add.StreetId ?? 0;

                }
            }
            p.HouseNumber = inPd.NumberOfHome;
            p.DistrictId = inPd.DistrictId;
            p.WardId = inPd.WardId;
            p.StreetId = inPd.StreetId;
            p.StateProvinceId = 23;//23 ho chi minh
            p.CallForPrice = inPd.Price == 0;
            p.FullDescription = inPd.Desription;
            p.ShortDescription = GetShortDes(inPd.Desription);
            p.Width = inPd.Width;
            p.Height = inPd.Dept;
            p.Area = inPd.Area;
            p.AreaUse = inPd.AreaUse;
            p.Status = (int)ProductStatusEnum.PendingAproved;
            p.CurrencyId = 0;
            p.CustomerId = _workContext.CurrentCustomer.Id;
            p.Name = inPd.Name;
            p.FullAddress = inPd.FullAddress;

            p.FullDescription +="<br><br>" +inPd.NoteFacilities;
            p.FullDescription += "<br><br>" + inPd.NoteEnvironments;
            p.FullDescription += "<br><br>" + inPd.NoteThichHop;





        }

        private InsertProductModel ProductToInsertModel(Product p)
        {
            var model = new InsertProductModel
            {
                Id = p.Id,
                Dept = p.Height,
                Width = p.Width,
                Desription = p.FullDescription,
                DistrictId = p.DistrictId,
                WardId = p.WardId,
                StreetId = p.StreetId,
                ContactName = p.ContactName,
                Email = p.ContactEmail,
                ContactPhone = p.ContactPhone,
                Area = p.Area,
                AreaUse = p.AreaUse,
                Price = p.Price,
                NumberOfHome = p.HouseNumber,
                SelectedOptionAttributes = p.ProductSpecificationAttributes.Select(x => x.SpecificationAttributeOptionId).ToList(),
                Name = p.Name,
                PictureIds = p.ProductPictures.Select(x => new InsertProductModel.PictureUploadModel {Id=x.PictureId,Title=x.Description }).ToList(),
                FullAddress = p.FullAddress
            };
            var cate = p.ProductCategories.FirstOrDefault();
            if (cate != null)
                model.CateId = cate.CategoryId;

            model.PictureModels = p.ProductPictures.Select(x => new PictureModel
            {
                ImageUrl = _pictureService.GetPictureUrl(x.PictureId, 100),
                Description=x.Description,
                PictureId=x.PictureId,
                Id=x.Id

            }).ToList();

            return model;


        }

        [NonAction]
        private void PreparingInsertProductModel(InsertProductModel model)
        {
            model.Districts = _stateProvinceService.GetDistHCM().ToSelectList(x => x.Name, x => x.Id.ToString());
            model.Directors = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.Director).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.BadRooms = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.NumberOfBadRoom).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.BedRooms = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.NumberOfBedRoom).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.Environments = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.Enviroment).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.Facilities = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.CoSoVatChat).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.NumberFloors = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.NumberOfFloor).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            model.NumberBlocks = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.NumberBlock).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.PhapLy = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.PhapLy).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.ThichHop = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.ThichHop).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            model.Categories = _categoryService.GetAllCategories().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
           

           

        }

        public ActionResult UploadPicture(HttpPostedFileBase Filedata)
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (Filedata != null)
            {
                // IE
                HttpPostedFileBase httpPostedFile = Filedata;
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                //contentType = httpPostedFile.ContentType;
            }
            //else
            //{
            //    //Webkit, Mozilla
            //    stream = Request.InputStream;
            //    fileName = Request[fileControl];
            //}

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    default:
                        break;
                }
            }
            fileBinary = CreateThumbnail(fileBinary, 700);
            var picture = _pictureService.InsertPicture(fileBinary, contentType, null, true);
            return Content(picture.Id.ToString());
            //return Json(new { data = picture.Id},JsonRequestBehavior.AllowGet);
            //return picture.Id;
        }
        [NonAction]
        public byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    if (startBitmap.Height <= LargestSide)
                    {
                        newHeight = startBitmap.Height;
                        newWidth = startBitmap.Width;
                    }
                    else
                    {
                        newHeight = LargestSide;
                        HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                        newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                    }
                }
                else
                {
                    if (startBitmap.Width > LargestSide)
                    {
                        newWidth = LargestSide;
                        HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                        newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                    }
                    else
                    {
                        newHeight = startBitmap.Height;
                        newWidth = startBitmap.Width;
                    }
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        [NonAction]
        public Bitmap ResizeImage(Bitmap image, int width, int height)
        {

            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }
        #endregion

        #region productListCustomer
        #region ProductList
        [NopHttpsRequirement(SslRequirement.Yes)]
        public ActionResult ProductList()
        {

            if (!_workContext.CurrentCustomer.IsRegistered())
                return new HttpUnauthorizedResult();
            var model = new SearchModel();
            PreparingSearchModel(model);
            model.OnlyCustomer = true;
            return View(model);
        }
        
        [NopHttpsRequirement(SslRequirement.No)]
        [HttpGet]
        public ActionResult ProductSearch(SearchModel model)
        {
            if (model.PagingFilteringContext.PageSize <= 0) model.PagingFilteringContext.PageSize = _catalogSettings.SearchPageProductsPerPage;
            if (model.PagingFilteringContext.PageNumber <= 0) model.PagingFilteringContext.PageNumber = 1;
            decimal minPriceConverted = decimal.Zero, maxPriceConverted = decimal.Zero;
            if (!string.IsNullOrEmpty(model.PriceString))
            {
                var priceString = model.PriceString.Split('-');
                if (priceString.Length > 1)
                {
                    decimal.TryParse(priceString[0], out minPriceConverted);
                    decimal.TryParse(priceString[1], out maxPriceConverted);
                    minPriceConverted = minPriceConverted * 1000000;
                    maxPriceConverted = maxPriceConverted * 1000000;
                }
            }

            ProductStatusEnum? status = null;
            IList<int> categories = null;
            IList<int> districtIds = new List<int>();
            if (model.Cid > 0)
                categories.Add(model.Cid);
            if (model.DistrictId > 0)
                districtIds.Add(model.DistrictId);
            model.DistrictIds.Select(x =>
            {
                districtIds.Add(x);
                return x;
            }).ToList();
            if (model.StatusId != 0)
                status = (ProductStatusEnum)model.StatusId;
            DateTime? startDate = null, endDate = null;
            if (!string.IsNullOrWhiteSpace(model.StartDate))
                startDate = Convert.ToDateTime(model.StartDate);
            if (!string.IsNullOrWhiteSpace(model.EndDate))
                endDate = Convert.ToDateTime(model.EndDate);

            var products = _productService.SearchProducts(
                            categoryIds: categories,
                            manufacturerId: 0,
                            storeId: 0,
                            customerId: model.OnlyCustomer ? _workContext.CurrentCustomer.Id : 0,
                            visibleIndividuallyOnly: true,
                            priceMin: minPriceConverted,
                            priceMax: maxPriceConverted,
                            keywords: null,
                            searchDescriptions: false,
                            searchSku: false,
                            searchProductTags: false,
                            languageId: 0,
                            filteredSpecs: model.SelectedOptionIds,
                            dictrictIds: districtIds,
                            status: status,
                            startDateTimeUtc: startDate,
                            endDateTimeUtc: endDate,
                            pageIndex: model.PagingFilteringContext.PageNumber - 1,
                            pageSize: model.PagingFilteringContext.PageSize);
            model.Products = PrepareProductOverviewModels(products).ToList();
            if (products.Count > 0)
                model.PagingFilteringContext.LoadPagedList(products);
            return PartialView("_ProductListPartial", model);

        }
        private void PreparingSearchModel(SearchModel model, bool isproject = false, int categoryId = 0)
        {            
            IList<Category> cate = null;
            //if (!isproject)
                cate = _categoryService.GetAllCategoriesByParentCategoryId(categoryId).OrderBy(x => x.DisplayOrder).ToList();
            //else
                //cate = _categoryService.GetAllCategoriesByParentCategoryId(2);

            model.AvailableCategories = cate.ToSelectList(x => x.Name, x => x.Id.ToString()).ToList();
            model.AvailableCategories.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Product.Search.SelectCate"), Selected = true, Value = "0" });
            model.Districts = _stateProvinceService.GetDistHCM().OrderBy(x => x.DisplayOrder).ToSelectList(x => x.Name, x => x.Id.ToString()).ToList();
            model.Districts.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Product.Search.SelectDistrict"), Selected = true, Value = "0" });
            model.Status = Enum.GetValues(typeof(ProductStatusEnum)).Cast<ProductStatusEnum>().ToSelectList(x => _localizationService.GetResource("Product.Status.Enum." + x.ToString()), x => ((int)x).ToString());
            model.Status.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Product.Search.SelectStatus"), Selected = true, Value = "0" });

            model.BedRooms = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.NumberOfBedRoom).ToSelectList(x => x.Name, x => x.Id.ToString());
            model.BadRooms = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.NumberOfBadRoom).ToSelectList(x => x.Name, x => x.Id.ToString());
            model.Directories = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute((int)ProductAttributeEnum.Director).ToSelectList(x => x.Name, x => x.Id.ToString());
            model.Directories.Insert(0, new SelectListItem { Text = _localizationService.GetResource(" Product.Search.SelectDirector"), Selected = true, Value = "0" });
           

        }

        private string ReturnPriceString(decimal price, string symbol)
        {
            price = price / 1000000;
            if (Math.Floor(price / 1000) > 0)
            {
                if (price % 1000 != 0)
                {
                    return ((int)Math.Floor(price / 1000)).ToString() + " tỉ " + ((int)(price % 1000)).ToString() + " triệu " + symbol;
                }
                else
                {
                    return ((int)Math.Floor(price / 1000)).ToString() + " tỉ " + symbol;
                }
            }
            else
            {
                return ((int)price).ToString() + " triệu " + symbol;
            }
        }
      
        [ChildActionOnly]
        public ActionResult SearchBoxLeft(bool isProject = false, int categoryId = 0)
        {
            var model = new SearchModel();
            PreparingSearchModel(model, isProject, categoryId);
            model.Districts.RemoveAt(0);
            model.AvailableCategories.RemoveAt(0);
            if (isProject)
                return PartialView("ProjectSearchBoxLeft", model);
            return PartialView("SearchBoxLeft", model);
        }
        [ChildActionOnly]
        public ActionResult SearchBoxHead(bool isHome)
        {

            var model = new SearchModel();
            PreparingSearchModel(model, categoryId:1);
            if (isHome)
                return View("SearchHome", model);
            return View(model);
        }
        [NonAction]
        public string GetShortDes(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";
            input = Regex.Replace(input, "<style>(.|\n)*?</style>", string.Empty);
            input = Regex.Replace(input, @"<xml>(.|\n)*?</xml>", string.Empty); // remove all <xml></xml> tags and anything inbetween.  
            input = Regex.Replace(input, @"<(.|\n)*?>", string.Empty);

            return input.TrimString(150);
        }

        [ChildActionOnly]
        public ActionResult RelatedProductCustomer(int customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                return Content("");
            var products = _productService.SearchProducts(
                   storeId: _storeContext.CurrentStore.Id,
                      visibleIndividuallyOnly: true,
                      customerId: customerId,
                   orderBy: ProductSortingEnum.CreatedOn,
                   pageSize: 2);
            var model = PrepareProductOverviewModels(products, preparePictureModel: true, productThumbPictureSize: 220);
            return View(model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ProductSearch1(int categoryId, int streetId, int wardId, int districtId, int stateProvinceId, CatalogPagingFilteringModel command)
        {
            var model = new SearchModel();

            //'Continue shopping' URL
            //_genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
            //    SystemCustomerAttributeNames.LastContinueShoppingPage,
            //    _webHelper.GetThisPageUrl(false),
            //    _storeContext.CurrentStore.Id);

            if (command.PageSize <= 0) command.PageSize = _catalogSettings.SearchPageProductsPerPage;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            if (model.Q == null)
                model.Q = "";
            model.Q = model.Q.Trim();

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();

            string cacheKey = string.Format(ModelCacheEventConsumer.SEARCH_CATEGORIES_MODEL_KEY, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var categories = _cacheManager.Get(cacheKey, () =>
            {
                var categoriesModel = new List<SearchPagingFilteringModel.CategoryModel>();
                //all categories
                foreach (var c in _categoryService.GetAllCategories())
                {
                    //generate full category name (breadcrumb)
                    string categoryBreadcrumb = "";
                    var breadcrumb = c.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService);
                    for (int i = 0; i <= breadcrumb.Count - 1; i++)
                    {
                        categoryBreadcrumb += breadcrumb[i].GetLocalized(x => x.Name);
                        if (i != breadcrumb.Count - 1)
                            categoryBreadcrumb += " >> ";
                    }
                    categoriesModel.Add(new SearchPagingFilteringModel.CategoryModel()
                    {
                        Id = c.Id,
                        Breadcrumb = categoryBreadcrumb
                    });
                }
                return categoriesModel;
            });
            if (categories.Count > 0)
            {
                //first empty entry
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Value = "0",
                    Text = _localizationService.GetResource("Common.All")
                });
                //all other categories
                foreach (var c in categories)
                {
                    model.AvailableCategories.Add(new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Breadcrumb,
                        Selected = model.Cid == c.Id
                    });
                }
            }

            var manufacturers = _manufacturerService.GetAllManufacturers();
            if (manufacturers.Count > 0)
            {
                model.AvailableManufacturers.Add(new SelectListItem()
                {
                    Value = "0",
                    Text = _localizationService.GetResource("Common.All")
                });
                foreach (var m in manufacturers)
                    model.AvailableManufacturers.Add(new SelectListItem()
                    {
                        Value = m.Id.ToString(),
                        Text = m.GetLocalized(x => x.Name),
                        Selected = model.Mid == m.Id
                    });
            }


            IPagedList<Product> products = new PagedList<Product>(new List<Product>(), 0, 1);            // only search if query string search keyword is set (used to avoid searching or displaying search term min length error message on /search page load)

            var categoryIds = new List<int>();
            int manufacturerId = 0;
            decimal? minPriceConverted = null;
            decimal? maxPriceConverted = null;
            bool searchInDescriptions = false;
            if (model.As)
            {
                //advanced search
                //var categoryId = model.Cid;
                if (categoryId > 0)
                {
                    categoryIds.Add(categoryId);
                    if (model.Isc)
                    {
                        //include subcategories
                        categoryIds.AddRange(GetChildCategoryIds(categoryId));
                    }
                }


                manufacturerId = model.Mid;

                //min price
                if (!string.IsNullOrEmpty(model.Pf))
                {
                    decimal minPrice = decimal.Zero;
                    if (decimal.TryParse(model.Pf, out minPrice))
                        minPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(minPrice, _workContext.WorkingCurrency);
                }
                //max price
                if (!string.IsNullOrEmpty(model.Pt))
                {
                    decimal maxPrice = decimal.Zero;
                    if (decimal.TryParse(model.Pt, out maxPrice))
                        maxPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(maxPrice, _workContext.WorkingCurrency);
                }

                searchInDescriptions = model.Sid;
            }

            //var searchInProductTags = false;
            var searchInProductTags = searchInDescriptions;

            //products
            products = _productService.SearchProducts(
                categoryIds: categoryIds,
                manufacturerId: manufacturerId,
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                priceMin: minPriceConverted,
                priceMax: maxPriceConverted,
                keywords: null,
                searchDescriptions: searchInDescriptions,
                searchSku: searchInDescriptions,
                searchProductTags: searchInProductTags,
                languageId: _workContext.WorkingLanguage.Id,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize,                
                dictrictIds: new List<int> { districtId },
                streetId: streetId,
                wardId: wardId,
                stateProvinceId: stateProvinceId
                );
            model.Products = PrepareProductOverviewModels(products).ToList();

            model.NoResults = !model.Products.Any();

            //search term statistics
            if (!String.IsNullOrEmpty(model.Q))
            {
                var searchTerm = _searchTermService.GetSearchTermByKeyword(model.Q, _storeContext.CurrentStore.Id);
                if (searchTerm != null)
                {
                    searchTerm.Count++;
                    _searchTermService.UpdateSearchTerm(searchTerm);
                }
                else
                {
                    searchTerm = new SearchTerm()
                    {
                        Keyword = model.Q,
                        StoreId = _storeContext.CurrentStore.Id,
                        Count = 1
                    };
                    _searchTermService.InsertSearchTerm(searchTerm);
                }
            }

            //event
            _eventPublisher.Publish(new ProductSearchEvent()
            {
                SearchTerm = model.Q,
                SearchInDescriptions = searchInDescriptions,
                CategoryIds = categoryIds,
                ManufacturerId = manufacturerId,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            });



            model.PagingFilteringContext.LoadPagedList(products);
            if (Request.IsAjaxRequest())
                return PartialView("_ProductListPartial", model);
            return View("Search", model);

        }
        [HttpPost]
        public ActionResult ProductPictureDelete(int id)
        {
            if ( !_workContext.CurrentCustomer.IsAdmin())
            {
                return Json("Bạn không có quyền thực hiện thao tác!");
            }
            var productPicture = _productService.GetProductPictureById(id);
            var product = _productService.GetProductById(productPicture.ProductId);
            if (product == null)
                return Json("Product is null");

           

            //var productimage = _productService.GetProductPicturesByProductId(product.Id);

            if (productPicture == null)
            {
                return Json(new { error = 1, message = "Ảnh không tồn tại" }, JsonRequestBehavior.AllowGet);
            }

            var productId = productPicture.ProductId;

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null)
            {
                //var product = _productService.GetProductById(productId);
                if (product != null && product.VendorId != _workContext.CurrentVendor.Id)
                {
                    return Content("This is not your product");
                }
            }
            var pictureId = productPicture.PictureId;
            _productService.DeleteProductPicture(productPicture);
            var picture = _pictureService.GetPictureById(pictureId);
            _pictureService.DeletePicture(picture);

            return Json(new { error = 0, message = "Xóa ảnh thành công" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion
    }
}
