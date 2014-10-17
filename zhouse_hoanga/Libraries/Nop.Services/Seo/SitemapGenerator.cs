using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Services.Catalog;
using Nop.Services.Topics;
using Nop.Services.News;
using Nop.Services.Directory;
using System.Collections.Generic;

namespace Nop.Services.Seo
{
    /// <summary>
    /// Represents a sitemap generator
    /// </summary>
    public partial class SitemapGenerator : BaseSitemapGenerator, ISitemapGenerator
    {
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ITopicService _topicService;
        private readonly CommonSettings _commonSettings;
        private readonly INewsService _newsService;
        private readonly ICategoryNewsService _categoryNewsService;
        private readonly IStateProvinceService _stateProvinceService;

        public SitemapGenerator(IStoreContext storeContext,
            ICategoryService categoryService, IProductService productService,
            IManufacturerService manufacturerService, ITopicService topicService,
            CommonSettings commonSettings,
            INewsService newsService,
            ICategoryNewsService categoryNewsService,
            IStateProvinceService stateProvinceService)
        {
            this._storeContext = storeContext;
            this._categoryService = categoryService;
            this._productService = productService;
            this._manufacturerService = manufacturerService;
            this._topicService = topicService;
            this._commonSettings = commonSettings;
            this._newsService = newsService;
            this._categoryNewsService = categoryNewsService;
            this._stateProvinceService = stateProvinceService;
        }

        /// <summary>
        /// Method that is overridden, that handles creation of child urls.
        /// Use the method WriteUrlLocation() within this method.
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        protected override void GenerateUrlNodes(UrlHelper urlHelper)
        {
            var categoryUrl = new List<string>();

            if (_commonSettings.SitemapIncludeCategories)
            {
                WriteCategories(urlHelper, 0, _commonSettings.SitemapIncludeDistricts, categoryUrl);
            }

            if (_commonSettings.SitemapIncludeDistricts)
            {
                WriteDistricts(urlHelper, categoryUrl);
            }

            if (_commonSettings.SitemapIncludeWards)
            {
                WriteWards(urlHelper, categoryUrl);
            }

            if (_commonSettings.SitemapIncludeStreets)
            {
                WriteStreets(urlHelper, categoryUrl);
            }

            if (_commonSettings.SitemapIncludeManufacturers)
            {
                WriteManufacturers(urlHelper);
            }

            if (_commonSettings.SitemapIncludeProducts)
            {
                WriteProducts(urlHelper);
            }

            if (_commonSettings.SitemapIncludeTopics)
            {
                WriteTopics(urlHelper);
            }

            if (_commonSettings.SitemapIncludeNewsCategories)
            {
                WriteNewsCategories(urlHelper);
            }

            if (_commonSettings.SitemapIncludeNews)
            {
                WriteNews(urlHelper);
            }
        }

        private void WriteCategories(UrlHelper urlHelper, int parentCategoryId, bool isCategoriesOut = false, List<string> categoryUrlOut = null)
        {
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId);
            if (isCategoriesOut && categories != null)
            {
                if (categoryUrlOut == null) categoryUrlOut = new List<string>();
            }

            foreach (var category in categories)
            {
                var url = urlHelper.RouteUrl("Category", new { SeName = category.GetSeName() }, "http");
                var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapCategoriesChangefreq ?? "Daily");
                var updateTime = category.UpdatedOnUtc;
                WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapCategoriesPriority ?? "0.5");

                if (isCategoriesOut) categoryUrlOut.Add(url);

                WriteCategories(urlHelper, category.Id, isCategoriesOut, categoryUrlOut);
            }
        }

        private void WriteDistricts(UrlHelper urlHelper, List<string> categoryUrl)
        {
            var districts = _stateProvinceService.GetDistHCM();
            foreach (var district in districts)
            {
                foreach (var category in categoryUrl)
                {
                    var url = string.Format("{0}-{1}", category, district.GetSeName());
                    var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapDistrictsChangefreq ?? "Daily");
                    var updateTime = DateTime.Now;
                    WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapDistrictsPriority ?? "0.5");
                }
            }
        }

        private void WriteWards(UrlHelper urlHelper, List<string> categoryUrl)
        {
            var districts = _stateProvinceService.GetDistHCM();
            var wards = new System.Collections.Generic.List<Nop.Core.Domain.Directory.Ward>();
            foreach (var district in districts)
            {
                wards.AddRange(_stateProvinceService.GetWardByDistrictId(district.Id));
            }

            foreach (var ward in wards)
            {
                foreach (var category in categoryUrl)
                {
                    var url = string.Format("{0}-{1}", category, ward.GetSeName());
                    var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapWardsChangefreq ?? "Daily");
                    var updateTime = DateTime.Now;
                    WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapWardsPriority ?? "0.5");
                }
            }
        }

        private void WriteStreets(UrlHelper urlHelper, List<string> categoryUrl)
        {
            var districts = _stateProvinceService.GetDistHCM();
            var streets = new System.Collections.Generic.List<Nop.Core.Domain.Directory.Street>();
            foreach (var district in districts)
            {
                streets.AddRange(_stateProvinceService.GetStreetByDistrictId(district.Id));
            }

            foreach (var street in streets)
            {
                foreach (var category in categoryUrl)
                {
                    var url = string.Format("{0}-{1}", category, street.GetSeName());
                    var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapStreetsChangefreq ?? "Daily");
                    var updateTime = DateTime.Now;
                    WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapStreetsPriority ?? "0.5");
                }
            }
        }

        private void WriteManufacturers(UrlHelper urlHelper)
        {
            var manufacturers = _manufacturerService.GetAllManufacturers();
            foreach (var manufacturer in manufacturers)
            {
                var url = urlHelper.RouteUrl("Manufacturer", new { SeName = manufacturer.GetSeName() }, "http");
                var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapManufacturersChangefreq ?? "Daily");
                var updateTime = manufacturer.UpdatedOnUtc;
                WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapManufacturersPriority ?? "0.5");
            }
        }

        private void WriteProducts(UrlHelper urlHelper)
        {
            var products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                orderBy: ProductSortingEnum.CreatedOn);
            foreach (var product in products)
            {
                var url = urlHelper.RouteUrl("Product", new { SeName = product.GetSeName() }, "http");
                var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapProductsChangefreq ?? "Daily");
                var updateTime = product.UpdatedOnUtc;
                WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapProductsPriority ?? "0.5");
            }
        }

        private void WriteTopics(UrlHelper urlHelper)
        {
            var topics = _topicService.GetAllTopics(_storeContext.CurrentStore.Id).ToList().FindAll(t => t.IncludeInSitemap);
            foreach (var topic in topics)
            {
                var url = urlHelper.RouteUrl("Topic", new { SystemName = topic.SystemName.ToLowerInvariant() }, "http");
                var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapTopicsChangefreq ?? "Daily");
                var updateTime = DateTime.Now;
                WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapTopicsPriority ?? "0.5");
            }
        }

        private void WriteNewsCategories(UrlHelper urlHelper)
        {
            var categories = _categoryNewsService.GetAllCategories();
            foreach (var category in categories)
            {
                var url = urlHelper.RouteUrl("CateNews", new { SeName = category.GetSeName() }, "http");
                var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapNewsCategoriesChangefreq ?? "Daily");
                var updateTime = category.UpdatedOnUtc;
                WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapNewsCategoriesPriority ?? "0.5");
            }
        }

        private void WriteNews(UrlHelper urlHelper)
        {
            var news = _newsService.GetAllNews(0, _storeContext.CurrentStore.Id, 0, 0, int.MaxValue).ToList();
            foreach (var item in news)
            {
                var url = urlHelper.RouteUrl("NewsItem", new { SeName = item.GetSeName() }, "http");
                var updateFrequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), _commonSettings.SitemapNewsChangefreq ?? "Daily");
                var updateTime = DateTime.Now;
                WriteUrlLocation(url, updateFrequency, updateTime, _commonSettings.SitemapNewsPriority ?? "0.5");
            }
        }
    }
}
