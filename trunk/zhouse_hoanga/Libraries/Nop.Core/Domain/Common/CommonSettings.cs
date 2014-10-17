
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Common
{
    public class CommonSettings : ISettings
    {
        public bool UseSystemEmailForContactUsForm { get; set; }

        public bool UseStoredProceduresIfSupported { get; set; }

        public bool HideAdvertisementsOnAdminArea { get; set; }

        public bool SitemapEnabled { get; set; }
        public bool SitemapIncludeCategories { get; set; }
        public bool SitemapIncludeManufacturers { get; set; }
        public bool SitemapIncludeProducts { get; set; }
        public bool SitemapIncludeTopics { get; set; }
        public bool SitemapIncludeNews { get; set; }
        public bool SitemapIncludeNewsCategories { get; set; }
        public bool SitemapIncludeStreets { get; set; }
        public bool SitemapIncludeWards { get; set; }
        public bool SitemapIncludeDistricts { get; set; }


        public string SitemapCategoriesChangefreq { get; set; }
        public string SitemapManufacturersChangefreq { get; set; }
        public string SitemapProductsChangefreq { get; set; }
        public string SitemapTopicsChangefreq { get; set; }
        public string SitemapNewsChangefreq { get; set; }
        public string SitemapNewsCategoriesChangefreq { get; set; }
        public string SitemapStreetsChangefreq { get; set; }
        public string SitemapWardsChangefreq { get; set; }
        public string SitemapDistrictsChangefreq { get; set; }

        public string SitemapCategoriesPriority { get; set; }
        public string SitemapManufacturersPriority { get; set; }
        public string SitemapProductsPriority  { get; set; }
        public string SitemapTopicsPriority { get; set; }
        public string SitemapNewsPriority { get; set; }
        public string SitemapNewsCategoriesPriority { get; set; }
        public string SitemapStreetsPriority { get; set; }
        public string SitemapWardsPriority { get; set; }
        public string SitemapDistrictsPriority { get; set; }


        /// <summary>
        /// Gets a sets a value indicating whether to display a warning if java-script is disabled
        /// </summary>
        public bool DisplayJavaScriptDisabledWarning { get; set; }

        /// <summary>
        /// Gets a sets a value indicating whether full-text search is supported
        /// </summary>
        public bool UseFullTextSearch { get; set; }

        /// <summary>
        /// Gets a sets a Full-Text search mode
        /// </summary>
        public FulltextSearchMode FullTextMode { get; set; }

        /// <summary>
        /// Gets a sets a value indicating whether 404 errors (page or file not found) should be logged
        /// </summary>
        public bool Log404Errors { get; set; }

        /// <summary>
        /// Gets a sets a breadcrumb delimiter used on the site
        /// </summary>
        public string BreadcrumbDelimiter { get; set; }

        /// <summary>
        /// Gets a sets a value indicating whether we should render <meta http-equiv="X-UA-Compatible" content="IE=edge"/> tag
        /// </summary>
        public bool RenderXuaCompatible { get; set; }

        /// <summary>
        /// Gets a sets a value of "X-UA-Compatible" META tag
        /// </summary>
        public string XuaCompatibleValue { get; set; }

        
    }
}