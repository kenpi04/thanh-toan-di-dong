using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using System.Transactions;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial class ProductService : IProductService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "Nop.product.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "Nop.product.";
        #endregion

        #region Fields

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<RelatedProduct> _relatedProductRepository;
        private readonly IRepository<CrossSellProduct> _crossSellProductRepository;
        private readonly IRepository<TierPrice> _tierPriceRepository;
        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<ProductSpecificationAttribute> _productSpecificationAttributeRepository;
        private readonly IRepository<ProductReview> _productReviewRepository;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILanguageService _languageService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="relatedProductRepository">Related product repository</param>
        /// <param name="crossSellProductRepository">Cross-sell product repository</param>
        /// <param name="tierPriceRepository">Tier price repository</param>
        /// <param name="localizedPropertyRepository">Localized property repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="productPictureRepository">Product picture repository</param>
        /// <param name="productSpecificationAttributeRepository">Product specification attribute repository</param>
        /// <param name="productReviewRepository">Product review repository</param>
        /// <param name="productAttributeService">Product attribute service</param>
        /// <param name="productAttributeParser">Product attribute parser service</param>
        /// <param name="languageService">Language service</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="eventPublisher">Event published</param>
        public ProductService(ICacheManager cacheManager,
            IRepository<Product> productRepository,
            IRepository<RelatedProduct> relatedProductRepository,
            IRepository<CrossSellProduct> crossSellProductRepository,
            IRepository<TierPrice> tierPriceRepository,
            IRepository<ProductPicture> productPictureRepository,
            IRepository<LocalizedProperty> localizedPropertyRepository,
            IRepository<AclRecord> aclRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IRepository<ProductSpecificationAttribute> productSpecificationAttributeRepository,
            IRepository<ProductReview> productReviewRepository,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            ILanguageService languageService,
            IWorkflowMessageService workflowMessageService,
            IDataProvider dataProvider, IDbContext dbContext,
            IWorkContext workContext, IStoreContext storeContext,
            LocalizationSettings localizationSettings, CommonSettings commonSettings,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._productRepository = productRepository;
            this._relatedProductRepository = relatedProductRepository;
            this._crossSellProductRepository = crossSellProductRepository;
            this._tierPriceRepository = tierPriceRepository;
            this._productPictureRepository = productPictureRepository;
            this._localizedPropertyRepository = localizedPropertyRepository;
            this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._productSpecificationAttributeRepository = productSpecificationAttributeRepository;
            this._productReviewRepository = productReviewRepository;
            this._productAttributeService = productAttributeService;
            this._productAttributeParser = productAttributeParser;
            this._languageService = languageService;
            this._workflowMessageService = workflowMessageService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationSettings = localizationSettings;
            this._commonSettings = commonSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Products

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            product.Deleted = true;
            //delete product
            UpdateProduct(product);
        }

        /// <summary>
        /// Gets all products displayed on the home page: Published,Deleted,ShowHomePage, Status
        /// </summary>
        /// <returns>Product collection</returns>
        public virtual IList<Product> GetAllProductsDisplayedOnHomePage()
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = from p in _productRepository.Table
                            orderby p.Name
                            where p.Published &&
                            !p.Deleted &&
                            p.ShowOnHomePage &&
                            p.Status.Equals((short)ProductStatusEnum.Approved)//only approved
                            select p;
                var products = query.ToList();
                return products;
            }
        }
        public virtual async Task<IList<Product>> GetAllProductsDisplayedOnHomePageAsync()
        {
            return await Task.Factory.StartNew<IList<Product>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = from p in _productRepository.Table
                                orderby p.Name
                                where p.Published &&
                                !p.Deleted &&
                                p.ShowOnHomePage &&
                                p.Status.Equals((short)ProductStatusEnum.Approved)//only approved
                                select p;
                    var products = query.ToList();
                    return products;
                }
            });
        }

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        public virtual Product GetProductById(int productId)
        {
            if (productId == 0)
                return null;

            string key = string.Format(PRODUCTS_BY_ID_KEY, productId);
            return _cacheManager.Get(key, () => { return _productRepository.GetById(productId); });
        }
        public virtual async Task<Product> GetProductByIdAsync(int productId)
        {
            if (productId == 0)
                return null;
            return await Task.Factory.StartNew<Product>(() =>
            {
                string key = string.Format(PRODUCTS_BY_ID_KEY, productId);
                return _cacheManager.Get(key, () => { return _productRepository.GetById(productId); });
            });
        }

        /// <summary>
        /// Get products by identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <returns>Products</returns>
        public virtual IList<Product> GetProductsByIds(int[] productIds)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }
                ))
            {
                if (productIds == null || productIds.Length == 0)
                    return new List<Product>();

                var query = from p in _productRepository.Table
                            where productIds.Contains(p.Id)
                            select p;
                var products = query.ToList();
                //sort by passed identifiers
                var sortedProducts = new List<Product>();
                foreach (int id in productIds)
                {
                    var product = products.Find(x => x.Id == id);
                    if (product != null)
                        sortedProducts.Add(product);
                }
                return sortedProducts;
            }
        }
        public virtual async Task<IList<Product>> GetProductsByIdsAsync(int[] productIds)
        {
            return await Task.Factory.StartNew<IList<Product>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    if (productIds == null || productIds.Length == 0)
                        return new List<Product>();

                    var query = from p in _productRepository.Table
                                where productIds.Contains(p.Id)
                                select p;
                    var products = query.ToList();
                    //sort by passed identifiers
                    var sortedProducts = new List<Product>();
                    foreach (int id in productIds)
                    {
                        var product = products.Find(x => x.Id == id);
                        if (product != null)
                            sortedProducts.Add(product);
                    }
                    return sortedProducts;
                }
            });
        }

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //insert
            _productRepository.Insert(product);

            //clear cache
            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(product);
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //update
            _productRepository.Update(product);

            //cache
            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(product);
        }

        #region search product

        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <param name="parentGroupedProductId">Parent product identifier (used with grouped products); 0 to load all records</param>
        /// <param name="productType">Product type; 0 to load all records</param>
        /// <param name="visibleIndividuallyOnly">A values indicating whether to load only products marked as "visible individually"; "false" to load all records; "true" to load "visible individually" only</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price; null to load all records</param>
        /// <param name="priceMax">Maximum price; null to load all records</param>
        /// <param name="productTagId">Product tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in product descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in product SKU</param>
        /// <param name="searchProductTags">A value indicating whether to search by a specified "keyword" in product tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Products</returns>
        public virtual IPagedList<Product> SearchProducts(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            int manufacturerId = 0,
            int storeId = 0,
            int vendorId = 0,
            int warehouseId = 0,
            int parentGroupedProductId = 0,
            ProductType? productType = null,
            bool visibleIndividuallyOnly = false,
            bool? featuredProducts = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            decimal? areaMin = null,
            decimal? areaMax = null,
            int productTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchSku = true,
            bool searchProductTags = false,
            int languageId = 0,
            int customerId = 0,
            IList<int> filteredSpecs = null,
            ProductStatusEnum? status = null,
            IList<int> districtIds = null,
            int streetId = 0,
            IList<int> wardId = null,
            int stateProvinceId = 0,
            DateTime? startDateTimeUtc = null,
            DateTime? endDateTimeUtc = null,
            ProductSortingEnum orderBy = ProductSortingEnum.Position,
            bool showHidden = false)
        {
            IList<int> filterableSpecificationAttributeOptionIds = null;
            return SearchProducts(out filterableSpecificationAttributeOptionIds, false,
                pageIndex, pageSize, categoryIds, manufacturerId,
                storeId, vendorId, warehouseId,
                parentGroupedProductId, productType, visibleIndividuallyOnly, featuredProducts,
                priceMin, priceMax, areaMin, areaMax, productTagId, keywords, searchDescriptions, searchSku,
                searchProductTags, languageId, customerId, filteredSpecs, status, districtIds, streetId, wardId, stateProvinceId, startDateTimeUtc, endDateTimeUtc, orderBy, showHidden);
        }

        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="filterableSpecificationAttributeOptionIds">The specification attribute option identifiers applied to loaded products (all pages)</param>
        /// <param name="loadFilterableSpecificationAttributeOptionIds">A value indicating whether we should load the specification attribute option identifiers applied to loaded products (all pages)</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <param name="parentGroupedProductId">Parent product identifier (used with grouped products); 0 to load all records</param>
        /// <param name="productType">Product type; 0 to load all records</param>
        /// <param name="visibleIndividuallyOnly">A values indicating whether to load only products marked as "visible individually"; "false" to load all records; "true" to load "visible individually" only</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price; null to load all records</param>
        /// <param name="priceMax">Maximum price; null to load all records</param>
        /// <param name="productTagId">Product tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in product descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in product SKU</param>
        /// <param name="searchProductTags">A value indicating whether to search by a specified "keyword" in product tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Products</returns>
        public virtual IPagedList<Product> SearchProducts(
            out IList<int> filterableSpecificationAttributeOptionIds,
            bool loadFilterableSpecificationAttributeOptionIds = false,
            int pageIndex = 0,
            int pageSize = 2147483647,  //Int32.MaxValue
            IList<int> categoryIds = null,
            int manufacturerId = 0,
            int storeId = 0,
            int vendorId = 0,
            int warehouseId = 0,
            int parentGroupedProductId = 0,
            ProductType? productType = null,
            bool visibleIndividuallyOnly = false,
            bool? featuredProducts = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            decimal? areaMin = null,
            decimal? areaMax = null,
            int productTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchSku = true,
            bool searchProductTags = false,
            int languageId = 0,
            int customerId = 0,
            IList<int> filteredSpecs = null,
            ProductStatusEnum? status = null,
            IList<int> districtIds = null,
            int streetId = 0,
            IList<int> wardId = null,
            int stateProvinceId = 0,
            DateTime? startDateTimeUtc = null,
            DateTime? endDateTimeUtc = null,
            ProductSortingEnum orderBy = ProductSortingEnum.Position,
            bool showHidden = false)
        {
            filterableSpecificationAttributeOptionIds = new List<int>();
            //validate "categoryIds" parameter
            if (categoryIds != null && categoryIds.Contains(0))
                categoryIds.Remove(0);

            #region Use stored procedure

            //pass category identifiers as comma-delimited string
            string commaSeparatedCategoryIds = "";
            if (categoryIds != null)
            {
                for (int i = 0; i < categoryIds.Count; i++)
                {
                    commaSeparatedCategoryIds += categoryIds[i].ToString();
                    if (i != categoryIds.Count - 1)
                    {
                        commaSeparatedCategoryIds += ",";
                    }
                }
            }
            //pass specification identifiers as comma-delimited string
            string commaSeparatedSpecIds = "";
            if (filteredSpecs != null)
            {
                ((List<int>)filteredSpecs).Sort();
                for (int i = 0; i < filteredSpecs.Count; i++)
                {
                    commaSeparatedSpecIds += filteredSpecs[i].ToString();
                    if (i != filteredSpecs.Count - 1)
                    {
                        commaSeparatedSpecIds += ",";
                    }
                }
            }
            //pass specification identifiers as comma-delimited string
            string commaSeparatedDistrictIds = "";
            if (districtIds != null)
            {
                ((List<int>)districtIds).Sort();
                for (int i = 0; i < districtIds.Count; i++)
                {
                    commaSeparatedDistrictIds += districtIds[i].ToString();
                    if (i != districtIds.Count - 1)
                    {
                        commaSeparatedDistrictIds += ",";
                    }
                }
            }

            string commaSeparatedWardIds = "";
            if (wardId != null)
            {
                ((List<int>)wardId).Sort();
                for (int i = 0; i < wardId.Count; i++)
                {
                    commaSeparatedWardIds += wardId[i].ToString();
                    if (i != wardId.Count - 1)
                    {
                        commaSeparatedWardIds += ",";
                    }
                }
            }

            //some databases don't support int.MaxValue
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            //prepare parameters
            var pCategoryIds = _dataProvider.GetParameter();
            pCategoryIds.ParameterName = "CategoryIds";
            pCategoryIds.Value = commaSeparatedCategoryIds != null ? (object)commaSeparatedCategoryIds : DBNull.Value;
            pCategoryIds.DbType = DbType.String;

            var pManufacturerId = _dataProvider.GetParameter();
            pManufacturerId.ParameterName = "ManufacturerId";
            pManufacturerId.Value = manufacturerId;
            pManufacturerId.DbType = DbType.Int32;

            var pStoreId = _dataProvider.GetParameter();
            pStoreId.ParameterName = "StoreId";
            pStoreId.Value = storeId;
            pStoreId.DbType = DbType.Int32;

            var pCustomerId = _dataProvider.GetParameter();
            pCustomerId.ParameterName = "CustomerId";
            pCustomerId.Value = customerId;
            pCustomerId.DbType = DbType.Int32;

            //var pVendorId = _dataProvider.GetParameter();
            //pVendorId.ParameterName = "VendorId";
            //pVendorId.Value = vendorId;
            //pVendorId.DbType = DbType.Int32;

            //var pWarehouseId = _dataProvider.GetParameter();
            //pWarehouseId.ParameterName = "WarehouseId";
            //pWarehouseId.Value = warehouseId;
            //pWarehouseId.DbType = DbType.Int32;

            //var pParentGroupedProductId = _dataProvider.GetParameter();
            //pParentGroupedProductId.ParameterName = "ParentGroupedProductId";
            //pParentGroupedProductId.Value = parentGroupedProductId;
            //pParentGroupedProductId.DbType = DbType.Int32;

            var pProductTypeId = _dataProvider.GetParameter();
            pProductTypeId.ParameterName = "ProductTypeId";
            pProductTypeId.Value = productType.HasValue ? (object)productType.Value : DBNull.Value;
            pProductTypeId.DbType = DbType.Int32;

            //var pVisibleIndividuallyOnly = _dataProvider.GetParameter();
            //pVisibleIndividuallyOnly.ParameterName = "VisibleIndividuallyOnly";
            //pVisibleIndividuallyOnly.Value = visibleIndividuallyOnly;
            //pVisibleIndividuallyOnly.DbType = DbType.Int32;

            //var pProductTagId = _dataProvider.GetParameter();
            //pProductTagId.ParameterName = "ProductTagId";
            //pProductTagId.Value = productTagId;
            //pProductTagId.DbType = DbType.Int32;

            //var pFeaturedProducts = _dataProvider.GetParameter();
            //pFeaturedProducts.ParameterName = "FeaturedProducts";
            //pFeaturedProducts.Value = featuredProducts.HasValue ? (object)featuredProducts.Value : DBNull.Value;
            //pFeaturedProducts.DbType = DbType.Boolean;

            var pPriceMin = _dataProvider.GetParameter();
            pPriceMin.ParameterName = "PriceMin";
            pPriceMin.Value = priceMin.HasValue ? (object)priceMin.Value : DBNull.Value;
            pPriceMin.DbType = DbType.Decimal;

            var pPriceMax = _dataProvider.GetParameter();
            pPriceMax.ParameterName = "PriceMax";
            pPriceMax.Value = priceMax.HasValue ? (object)priceMax.Value : DBNull.Value;
            pPriceMax.DbType = DbType.Decimal;

            var pAreaMin = _dataProvider.GetParameter();
            pAreaMin.ParameterName = "AreaMin";
            pAreaMin.Value = areaMin.HasValue ? (object)areaMin.Value : DBNull.Value;
            pAreaMin.DbType = DbType.Decimal;

            var pAreaMax = _dataProvider.GetParameter();
            pAreaMax.ParameterName = "AreaMax";
            pAreaMax.Value = areaMax.HasValue ? (object)areaMax.Value : DBNull.Value;
            pAreaMax.DbType = DbType.Decimal;

            var pKeywords = _dataProvider.GetParameter();
            pKeywords.ParameterName = "Keywords";
            pKeywords.Value = keywords != null ? (object)keywords : DBNull.Value;
            pKeywords.DbType = DbType.String;

            //var pSearchDescriptions = _dataProvider.GetParameter();
            //pSearchDescriptions.ParameterName = "SearchDescriptions";
            //pSearchDescriptions.Value = searchDescriptions;
            //pSearchDescriptions.DbType = DbType.Boolean;

            //var pSearchSku = _dataProvider.GetParameter();
            //pSearchSku.ParameterName = "SearchSku";
            //pSearchSku.Value = searchSku;
            //pSearchSku.DbType = DbType.Boolean;

            //var pSearchProductTags = _dataProvider.GetParameter();
            //pSearchProductTags.ParameterName = "SearchProductTags";
            //pSearchProductTags.Value = searchProductTags;
            //pSearchProductTags.DbType = DbType.Boolean;

            var pUseFullTextSearch = _dataProvider.GetParameter();
            pUseFullTextSearch.ParameterName = "UseFullTextSearch";
            pUseFullTextSearch.Value = _commonSettings.UseFullTextSearch;
            pUseFullTextSearch.DbType = DbType.Boolean;

            var pFullTextMode = _dataProvider.GetParameter();
            pFullTextMode.ParameterName = "FullTextMode";
            pFullTextMode.Value = (int)_commonSettings.FullTextMode;
            pFullTextMode.DbType = DbType.Int32;

            var pFilteredSpecs = _dataProvider.GetParameter();
            pFilteredSpecs.ParameterName = "FilteredSpecs";
            pFilteredSpecs.Value = commaSeparatedSpecIds != null ? (object)commaSeparatedSpecIds : DBNull.Value;
            pFilteredSpecs.DbType = DbType.String;
            //Add
            var pProducStatus = _dataProvider.GetParameter();
            pProducStatus.ParameterName = "ProductStatus";
            pProducStatus.Value = status.HasValue ? (object)status.Value : DBNull.Value;
            pProducStatus.DbType = DbType.Int16;

            var pdistricts = _dataProvider.GetParameter();
            pdistricts.ParameterName = "DistrictIds";
            pdistricts.Value = commaSeparatedDistrictIds != null ? (object)commaSeparatedDistrictIds : DBNull.Value;
            pdistricts.DbType = DbType.String;

            var pStreetId = _dataProvider.GetParameter();
            pStreetId.ParameterName = "StreetId";
            pStreetId.Value = streetId;
            pStreetId.DbType = DbType.Int32;

            var pWardId = _dataProvider.GetParameter();
            pWardId.ParameterName = "WardId";
            pWardId.Value = commaSeparatedWardIds != null ? (object)commaSeparatedWardIds : DBNull.Value;
            pWardId.DbType = DbType.String;

            var pStateProvinceId = _dataProvider.GetParameter();
            pStateProvinceId.ParameterName = "StateProvinceId";
            pStateProvinceId.Value = stateProvinceId;
            pStateProvinceId.DbType = DbType.Int32;

            var pStartDate = _dataProvider.GetParameter();
            pStartDate.ParameterName = "StartDate";
            pStartDate.Value = startDateTimeUtc.HasValue ? (object)startDateTimeUtc.Value : DBNull.Value;
            pStartDate.DbType = DbType.DateTime;

            var pEndDate = _dataProvider.GetParameter();
            pEndDate.ParameterName = "EndDate";
            pEndDate.Value = endDateTimeUtc.HasValue ? (object)endDateTimeUtc.Value : DBNull.Value;
            pEndDate.DbType = DbType.DateTime;

            //var pLanguageId = _dataProvider.GetParameter();
            //pLanguageId.ParameterName = "LanguageId";
            //pLanguageId.Value = 0;
            //pLanguageId.DbType = DbType.Int32;

            var pOrderBy = _dataProvider.GetParameter();
            pOrderBy.ParameterName = "OrderBy";
            pOrderBy.Value = (int)orderBy;
            pOrderBy.DbType = DbType.Int32;

            //var pAllowedCustomerRoleIds = _dataProvider.GetParameter();
            //pAllowedCustomerRoleIds.ParameterName = "AllowedCustomerRoleIds";
            //pAllowedCustomerRoleIds.Value = "";
            //pAllowedCustomerRoleIds.DbType = DbType.String;

            var pPageIndex = _dataProvider.GetParameter();
            pPageIndex.ParameterName = "PageIndex";
            pPageIndex.Value = pageIndex;
            pPageIndex.DbType = DbType.Int32;

            var pPageSize = _dataProvider.GetParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.Value = pageSize;
            pPageSize.DbType = DbType.Int32;

            var pShowHidden = _dataProvider.GetParameter();
            pShowHidden.ParameterName = "ShowHidden";
            pShowHidden.Value = showHidden;
            pShowHidden.DbType = DbType.Boolean;

            var pLoadFilterableSpecificationAttributeOptionIds = _dataProvider.GetParameter();
            pLoadFilterableSpecificationAttributeOptionIds.ParameterName = "LoadFilterableSpecificationAttributeOptionIds";
            pLoadFilterableSpecificationAttributeOptionIds.Value = loadFilterableSpecificationAttributeOptionIds;
            pLoadFilterableSpecificationAttributeOptionIds.DbType = DbType.Boolean;

            var pFilterableSpecificationAttributeOptionIds = _dataProvider.GetParameter();
            pFilterableSpecificationAttributeOptionIds.ParameterName = "FilterableSpecificationAttributeOptionIds";
            pFilterableSpecificationAttributeOptionIds.Direction = ParameterDirection.Output;
            pFilterableSpecificationAttributeOptionIds.Size = int.MaxValue - 1;
            pFilterableSpecificationAttributeOptionIds.DbType = DbType.String;

            var pTotalRecords = _dataProvider.GetParameter();
            pTotalRecords.ParameterName = "TotalRecords";
            pTotalRecords.Direction = ParameterDirection.Output;
            pTotalRecords.DbType = DbType.Int32;

            //invoke stored procedure
            var products = _dbContext.ExecuteStoredProcedureList<Product>(
                "ProductLoadAllPaged",
                pCategoryIds,
                pManufacturerId,
                pStoreId,
                //pVendorId,
                //pWarehouseId,
                //pParentGroupedProductId,
                pProductTypeId,
                //pVisibleIndividuallyOnly,
                //pProductTagId,
                //pFeaturedProducts,
                pPriceMin,
                pPriceMax,
                pAreaMin,
                pAreaMax,
                pKeywords,
                //pSearchDescriptions,
                //pSearchSku,
                //pSearchProductTags,
                pUseFullTextSearch,
                pFullTextMode,
                pFilteredSpecs,
                pdistricts,
                pStreetId,
                pWardId,
                pStateProvinceId,
                pStartDate,
                pEndDate,
                pProducStatus,
                pCustomerId,
                //pLanguageId,
                pOrderBy,
                //pAllowedCustomerRoleIds,
                pPageIndex,
                pPageSize,
                pShowHidden,
                pLoadFilterableSpecificationAttributeOptionIds,
                pFilterableSpecificationAttributeOptionIds,
                pTotalRecords);
            //get filterable specification attribute option identifier
            string filterableSpecificationAttributeOptionIdsStr = (pFilterableSpecificationAttributeOptionIds.Value != DBNull.Value) ? (string)pFilterableSpecificationAttributeOptionIds.Value : "";
            if (loadFilterableSpecificationAttributeOptionIds &&
                !string.IsNullOrWhiteSpace(filterableSpecificationAttributeOptionIdsStr))
            {
                filterableSpecificationAttributeOptionIds = filterableSpecificationAttributeOptionIdsStr
                   .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => Convert.ToInt32(x.Trim()))
                   .ToList();
            }
            //return products
            int totalRecords = (pTotalRecords.Value != DBNull.Value) ? Convert.ToInt32(pTotalRecords.Value) : 0;
            return new PagedList<Product>(products, pageIndex, pageSize, totalRecords);

            #endregion
        }

        #endregion

        /// <summary>
        /// Update product review totals
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void UpdateProductReviewTotals(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            int approvedRatingSum = 0;
            int notApprovedRatingSum = 0;
            int approvedTotalReviews = 0;
            int notApprovedTotalReviews = 0;
            var reviews = product.ProductReviews;
            foreach (var pr in reviews)
            {
                if (pr.IsApproved)
                {
                    approvedRatingSum += pr.Rating;
                    approvedTotalReviews++;
                }
                else
                {
                    notApprovedRatingSum += pr.Rating;
                    notApprovedTotalReviews++;
                }
            }

            product.ApprovedRatingSum = approvedRatingSum;
            product.NotApprovedRatingSum = notApprovedRatingSum;
            product.ApprovedTotalReviews = approvedTotalReviews;
            product.NotApprovedTotalReviews = notApprovedTotalReviews;
            UpdateProduct(product);
        }

        /// <summary>
        /// Get low stock products
        /// </summary>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <returns>Result</returns>
        public virtual IList<Product> GetLowStockProducts(int vendorId)
        {
            //Track inventory for product
            var query1 = from p in _productRepository.Table
                         orderby p.MinStockQuantity
                         where !p.Deleted &&
                         p.ManageInventoryMethodId == (int)ManageInventoryMethod.ManageStock &&
                         p.MinStockQuantity >= p.StockQuantity &&
                         (vendorId == 0 || p.VendorId == vendorId)
                         select p;
            var products1 = query1.ToList();

            //Track inventory for product by product attributes
            var query2 = from p in _productRepository.Table
                         from pvac in p.ProductVariantAttributeCombinations
                         where !p.Deleted &&
                         p.ManageInventoryMethodId == (int)ManageInventoryMethod.ManageStockByAttributes &&
                         pvac.StockQuantity <= 0 &&
                         (vendorId == 0 || p.VendorId == vendorId)
                         select p;
            //only distinct products (group by ID)
            //if we use standard Distinct() method, then all fields will be compared (low performance)
            query2 = from p in query2
                     group p by p.Id into pGroup
                     orderby pGroup.Key
                     select pGroup.FirstOrDefault();
            var products2 = query2.ToList();

            var result = new List<Product>();
            result.AddRange(products1);
            result.AddRange(products2);
            return result;
        }

        /// <summary>
        /// Gets a product by SKU
        /// </summary>
        /// <param name="sku">SKU</param>
        /// <returns>Product</returns>
        public virtual Product GetProductBySku(string sku)
        {
            if (String.IsNullOrEmpty(sku))
                return null;

            sku = sku.Trim();
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = from p in _productRepository.Table
                            orderby p.Id
                            where !p.Deleted &&
                            p.Sku == sku
                            select p;
                var product = query.FirstOrDefault();
                return product;
            }
        }
        public virtual async Task<Product> GetProductBySkuAsync(string sku)
        {
            if (String.IsNullOrEmpty(sku))
                return null;

            sku = sku.Trim();
            return await Task.Factory.StartNew<Product>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = from p in _productRepository.Table
                                orderby p.Id
                                where !p.Deleted &&
                                p.Sku == sku
                                select p;
                    var product = query.FirstOrDefault();
                    return product;
                }
            });
        }

        /// <summary>
        /// Adjusts inventory
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="decrease">A value indicating whether to increase or descrease product stock quantity</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        public virtual void AdjustInventory(Product product, bool decrease,
            int quantity, string attributesXml)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var prevStockQuantity = product.StockQuantity;

            switch (product.ManageInventoryMethod)
            {
                case ManageInventoryMethod.DontManageStock:
                    {
                        //do nothing
                        return;
                    }
                case ManageInventoryMethod.ManageStock:
                    {
                        int newStockQuantity = 0;
                        if (decrease)
                            newStockQuantity = product.StockQuantity - quantity;
                        else
                            newStockQuantity = product.StockQuantity + quantity;

                        bool newPublished = product.Published;
                        bool newDisableBuyButton = product.DisableBuyButton;
                        bool newDisableWishlistButton = product.DisableWishlistButton;

                        //check if minimum quantity is reached
                        if (decrease)
                        {
                            if (product.MinStockQuantity >= newStockQuantity)
                            {
                                switch (product.LowStockActivity)
                                {
                                    case LowStockActivity.DisableBuyButton:
                                        newDisableBuyButton = true;
                                        newDisableWishlistButton = true;
                                        break;
                                    case LowStockActivity.Unpublish:
                                        newPublished = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        product.StockQuantity = newStockQuantity;
                        product.DisableBuyButton = newDisableBuyButton;
                        product.DisableWishlistButton = newDisableWishlistButton;
                        product.Published = newPublished;
                        UpdateProduct(product);

                        //send email notification
                        if (decrease && product.NotifyAdminForQuantityBelow > newStockQuantity)
                            _workflowMessageService.SendQuantityBelowStoreOwnerNotification(product, _localizationSettings.DefaultAdminLanguageId);
                    }
                    break;
                case ManageInventoryMethod.ManageStockByAttributes:
                    {
                        var combination = _productAttributeParser.FindProductVariantAttributeCombination(product, attributesXml);
                        if (combination != null)
                        {
                            int newStockQuantity = 0;
                            if (decrease)
                                newStockQuantity = combination.StockQuantity - quantity;
                            else
                                newStockQuantity = combination.StockQuantity + quantity;

                            combination.StockQuantity = newStockQuantity;
                            _productAttributeService.UpdateProductVariantAttributeCombination(combination);
                        }
                    }
                    break;
                default:
                    break;
            }


            //bundled products
            var pvaValues = _productAttributeParser.ParseProductVariantAttributeValues(attributesXml);
            foreach (var pvaValue in pvaValues)
            {
                if (pvaValue.AttributeValueType == AttributeValueType.AssociatedToProduct)
                {
                    //associated product (bundle)
                    var associatedProduct = GetProductById(pvaValue.AssociatedProductId);
                    if (associatedProduct != null)
                    {
                        var totalQty = quantity * pvaValue.Quantity;
                        AdjustInventory(associatedProduct, decrease, totalQty, "");
                    }
                }
            }

            //TODO send back in stock notifications?
            //if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
            //    product.BackorderMode == BackorderMode.NoBackorders &&
            //    product.AllowBackInStockSubscriptions &&
            //    product.StockQuantity > 0 &&
            //    prevStockQuantity <= 0 &&
            //    product.Published &&
            //    !product.Deleted)
            //{
            //    //_backInStockSubscriptionService.SendNotificationsToSubscribers(product);
            //}
        }

        /// <summary>
        /// Update HasTierPrices property (used for performance optimization)
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void UpdateHasTierPricesProperty(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            product.HasTierPrices = product.TierPrices.Count > 0;
            UpdateProduct(product);
        }

        /// <summary>
        /// Update HasDiscountsApplied property (used for performance optimization)
        /// </summary>
        /// <param name="product">Product</param>
        public virtual void UpdateHasDiscountsApplied(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("productVariant");

            product.HasDiscountsApplied = product.AppliedDiscounts.Count > 0;
            UpdateProduct(product);
        }

        #endregion

        #region Related products

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void DeleteRelatedProduct(RelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _relatedProductRepository.Delete(relatedProduct);

            //event notification
            _eventPublisher.EntityDeleted(relatedProduct);
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public virtual IList<RelatedProduct> GetRelatedProductsByProductId1(int productId1, bool showHidden = false)
        {
            var query = from rp in _relatedProductRepository.Table
                        join p in _productRepository.Table on rp.ProductId2 equals p.Id
                        where rp.ProductId1 == productId1 &&
                        !p.Deleted &&
                        (showHidden || p.Published)
                        orderby rp.DisplayOrder
                        select rp;
            var relatedProducts = query.ToList();

            return relatedProducts;
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifier</param>
        /// <returns>Related product</returns>
        public virtual RelatedProduct GetRelatedProductById(int relatedProductId)
        {
            if (relatedProductId == 0)
                return null;

            return _relatedProductRepository.GetById(relatedProductId);
        }

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void InsertRelatedProduct(RelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _relatedProductRepository.Insert(relatedProduct);

            //event notification
            _eventPublisher.EntityInserted(relatedProduct);
        }

        /// <summary>
        /// Updates a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        public virtual void UpdateRelatedProduct(RelatedProduct relatedProduct)
        {
            if (relatedProduct == null)
                throw new ArgumentNullException("relatedProduct");

            _relatedProductRepository.Update(relatedProduct);

            //event notification
            _eventPublisher.EntityUpdated(relatedProduct);
        }

        #endregion

        #region Cross-sell products

        /// <summary>
        /// Deletes a cross-sell product
        /// </summary>
        /// <param name="crossSellProduct">Cross-sell identifier</param>
        public virtual void DeleteCrossSellProduct(CrossSellProduct crossSellProduct)
        {
            if (crossSellProduct == null)
                throw new ArgumentNullException("crossSellProduct");

            _crossSellProductRepository.Delete(crossSellProduct);

            //event notification
            _eventPublisher.EntityDeleted(crossSellProduct);
        }

        /// <summary>
        /// Gets a cross-sell product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell product collection</returns>
        public virtual IList<CrossSellProduct> GetCrossSellProductsByProductId1(int productId1, bool showHidden = false)
        {
            var query = from csp in _crossSellProductRepository.Table
                        join p in _productRepository.Table on csp.ProductId2 equals p.Id
                        where csp.ProductId1 == productId1 &&
                        !p.Deleted &&
                        (showHidden || p.Published)
                        orderby csp.Id
                        select csp;
            var crossSellProducts = query.ToList();
            return crossSellProducts;
        }

        /// <summary>
        /// Gets a cross-sell product
        /// </summary>
        /// <param name="crossSellProductId">Cross-sell product identifier</param>
        /// <returns>Cross-sell product</returns>
        public virtual CrossSellProduct GetCrossSellProductById(int crossSellProductId)
        {
            if (crossSellProductId == 0)
                return null;

            return _crossSellProductRepository.GetById(crossSellProductId);
        }

        /// <summary>
        /// Inserts a cross-sell product
        /// </summary>
        /// <param name="crossSellProduct">Cross-sell product</param>
        public virtual void InsertCrossSellProduct(CrossSellProduct crossSellProduct)
        {
            if (crossSellProduct == null)
                throw new ArgumentNullException("crossSellProduct");

            _crossSellProductRepository.Insert(crossSellProduct);

            //event notification
            _eventPublisher.EntityInserted(crossSellProduct);
        }

        /// <summary>
        /// Updates a cross-sell product
        /// </summary>
        /// <param name="crossSellProduct">Cross-sell product</param>
        public virtual void UpdateCrossSellProduct(CrossSellProduct crossSellProduct)
        {
            if (crossSellProduct == null)
                throw new ArgumentNullException("crossSellProduct");

            _crossSellProductRepository.Update(crossSellProduct);

            //event notification
            _eventPublisher.EntityUpdated(crossSellProduct);
        }

        /// <summary>
        /// Gets a cross-sells
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="numberOfProducts">Number of products to return</param>
        /// <returns>Cross-sells</returns>
        public virtual IList<Product> GetCrosssellProductsByShoppingCart(IList<ShoppingCartItem> cart, int numberOfProducts)
        {
            var result = new List<Product>();

            if (numberOfProducts == 0)
                return result;

            if (cart == null || cart.Count == 0)
                return result;

            var cartProductIds = new List<int>();
            foreach (var sci in cart)
            {
                int prodId = sci.ProductId;
                if (!cartProductIds.Contains(prodId))
                    cartProductIds.Add(prodId);
            }

            foreach (var sci in cart)
            {
                var crossSells = GetCrossSellProductsByProductId1(sci.ProductId);
                foreach (var crossSell in crossSells)
                {
                    //validate that this product is not added to result yet
                    //validate that this product is not in the cart
                    if (result.Find(p => p.Id == crossSell.ProductId2) == null &&
                        !cartProductIds.Contains(crossSell.ProductId2))
                    {
                        var productToAdd = GetProductById(crossSell.ProductId2);
                        //validate product
                        if (productToAdd == null || productToAdd.Deleted || !productToAdd.Published)
                            continue;

                        //add a product to result
                        result.Add(productToAdd);
                        if (result.Count >= numberOfProducts)
                            return result;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Tier prices

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        public virtual void DeleteTierPrice(TierPrice tierPrice)
        {
            if (tierPrice == null)
                throw new ArgumentNullException("tierPrice");

            _tierPriceRepository.Delete(tierPrice);

            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(tierPrice);
        }

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public virtual TierPrice GetTierPriceById(int tierPriceId)
        {
            if (tierPriceId == 0)
                return null;

            return _tierPriceRepository.GetById(tierPriceId);
        }

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        public virtual void InsertTierPrice(TierPrice tierPrice)
        {
            if (tierPrice == null)
                throw new ArgumentNullException("tierPrice");

            _tierPriceRepository.Insert(tierPrice);

            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(tierPrice);
        }

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        public virtual void UpdateTierPrice(TierPrice tierPrice)
        {
            if (tierPrice == null)
                throw new ArgumentNullException("tierPrice");

            _tierPriceRepository.Update(tierPrice);

            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(tierPrice);
        }

        #endregion

        #region Product pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void DeleteProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Delete(productPicture);

            //event notification
            _eventPublisher.EntityDeleted(productPicture);
        }

        /// <summary>
        /// Gets a product pictures by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product pictures</returns>
        public virtual IList<ProductPicture> GetProductPicturesByProductId(int productId)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = from pp in _productPictureRepository.Table
                            where pp.ProductId == productId
                            orderby pp.DisplayOrder
                            select pp;
                var productPictures = query.ToList();
                return productPictures;
            }
        }
        public virtual async Task<IList<ProductPicture>> GetProductPicturesByProductIdAsync(int productId)
        {
            return await Task.Factory.StartNew<IList<ProductPicture>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = from pp in _productPictureRepository.Table
                                where pp.ProductId == productId
                                orderby pp.DisplayOrder
                                select pp;
                    var productPictures = query.ToList();
                    return productPictures;
                }
            });
        }

        /// <summary>
        /// Gets a product picture
        /// </summary>
        /// <param name="productPictureId">Product picture identifier</param>
        /// <returns>Product picture</returns>
        public virtual ProductPicture GetProductPictureById(int productPictureId)
        {
            if (productPictureId == 0)
                return null;

            return _productPictureRepository.GetById(productPictureId);
        }
        public virtual async Task<ProductPicture> GetProductPictureByIdAsync(int productPictureId)
        {
            if (productPictureId == 0)
                return null;
            return await Task.Factory.StartNew<ProductPicture>(() =>
            {
                return _productPictureRepository.GetById(productPictureId);
            });
        }

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void InsertProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Insert(productPicture);

            //event notification
            _eventPublisher.EntityInserted(productPicture);
        }

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void UpdateProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Update(productPicture);

            //event notification
            _eventPublisher.EntityUpdated(productPicture);
        }

        #endregion

        #region Product reviews

        /// <summary>
        /// Gets all product reviews
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <param name="approved">A value indicating whether to content is approved; null to load all records</param> 
        /// <param name="fromUtc">Item creation from; null to load all records</param>
        /// <param name="toUtc">Item item creation to; null to load all records</param>
        /// <param name="message">Search title or review text; null to load all records</param>
        /// <returns>Reviews</returns>
        public virtual IList<ProductReview> GetAllProductReviews(int customerId, bool? approved,
            DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = null, int productId = 0)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = _productReviewRepository.Table;
                if (approved.HasValue)
                    query = query.Where(c => c.IsApproved == approved);
                if (customerId > 0)
                    query = query.Where(c => c.CustomerId == customerId);
                if (fromUtc.HasValue)
                    query = query.Where(c => fromUtc.Value <= c.CreatedOnUtc);
                if (toUtc.HasValue)
                    query = query.Where(c => toUtc.Value >= c.CreatedOnUtc);
                if (!String.IsNullOrEmpty(message))
                    query = query.Where(c => c.Title.Contains(message) || c.ReviewText.Contains(message));
                if (productId > 0)
                    query = query.Where(c => c.ProductId == productId);

                query = query.OrderBy(c => c.CreatedOnUtc);
                var content = query.ToList();
                return content;
            }
        }
        public virtual async Task<IList<ProductReview>> GetAllProductReviewsAsync(int customerId, bool? approved,
            DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = null, int productId = 0)
        {
            return await Task.Factory.StartNew<IList<ProductReview>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = _productReviewRepository.Table;
                    if (approved.HasValue)
                        query = query.Where(c => c.IsApproved == approved);
                    if (customerId > 0)
                        query = query.Where(c => c.CustomerId == customerId);
                    if (fromUtc.HasValue)
                        query = query.Where(c => fromUtc.Value <= c.CreatedOnUtc);
                    if (toUtc.HasValue)
                        query = query.Where(c => toUtc.Value >= c.CreatedOnUtc);
                    if (!String.IsNullOrEmpty(message))
                        query = query.Where(c => c.Title.Contains(message) || c.ReviewText.Contains(message));
                    if (productId > 0)
                        query = query.Where(c => c.ProductId == productId);

                    query = query.OrderBy(c => c.CreatedOnUtc);
                    var content = query.ToList();
                    return content;
                }
            });
        }

        /// <summary>
        /// Gets product review
        /// </summary>
        /// <param name="productReviewId">Product review identifier</param>
        /// <returns>Product review</returns>
        public virtual ProductReview GetProductReviewById(int productReviewId)
        {
            if (productReviewId == 0)
                return null;

            return _productReviewRepository.GetById(productReviewId);
        }
        public virtual async Task<ProductReview> GetProductReviewByIdAsync(int productReviewId)
        {
            if (productReviewId == 0)
                return null;
            return await Task.Factory.StartNew<ProductReview>(() =>
            {
                return _productReviewRepository.GetById(productReviewId);
            });
        }

        /// <summary>
        /// Deletes a product review
        /// </summary>
        /// <param name="productReview">Product review</param>
        public virtual void DeleteProductReview(ProductReview productReview)
        {
            if (productReview == null)
                throw new ArgumentNullException("productReview");

            _productReviewRepository.Delete(productReview);

            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);
        }

        #endregion

        #endregion
    }
}
