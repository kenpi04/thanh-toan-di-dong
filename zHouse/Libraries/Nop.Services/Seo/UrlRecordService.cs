using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;
using System.Data;
using Nop.Data;
using System.Data.SqlClient;

namespace Nop.Services.Seo
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class UrlRecordService : IUrlRecordService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// {2} : language ID
        /// </remarks>
        private const string URLRECORD_ACTIVE_BY_ID_NAME_LANGUAGE_KEY = "Nop.urlrecord.active.id-name-language-{0}-{1}-{2}";
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string URLRECORD_ALL_KEY = "Nop.urlrecord.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string URLRECORD_PATTERN_KEY = "Nop.urlrecord.";

        #endregion

        #region Fields

        private readonly IRepository<UrlRecord> _urlRecordRepository;
        private readonly ICacheManager _cacheManager;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="urlRecordRepository">URL record repository</param>
        /// <param name="localizationSettings">Localization settings</param>
        public UrlRecordService(ICacheManager cacheManager,
            IRepository<UrlRecord> urlRecordRepository,
            LocalizationSettings localizationSettings,
            IDataProvider dataProvider,
            IDbContext dbContext)
        {
            this._cacheManager = cacheManager;
            this._urlRecordRepository = urlRecordRepository;
            this._localizationSettings = localizationSettings;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities
        public virtual string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",  
            "đ",  
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",  
            "í","ì","ỉ","ĩ","ị",  
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",  
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",  
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",  
            "d",  
            "e","e","e","e","e","e","e","e","e","e","e",  
            "i","i","i","i","i",  
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",  
            "u","u","u","u","u","u","u","u","u","u","u",  
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        /// <summary>
        /// Gets all cached URL records
        /// </summary>
        /// <returns>cached URL records</returns>
        protected virtual IList<UrlRecordForCaching> GetAllUrlRecordsCached()
        {
            //cache
            string key = string.Format(URLRECORD_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                var query = from ur in _urlRecordRepository.Table
                            select ur;
                var urlRecords = query.ToList();
                var list = new List<UrlRecordForCaching>();
                foreach (var ur in urlRecords)
                {
                    var localizedPropertyForCaching = new UrlRecordForCaching()
                    {
                        Id = ur.Id,
                        EntityId = ur.EntityId,
                        EntityName = ur.EntityName,
                        Slug = ur.Slug,
                        IsActive = ur.IsActive,
                        LanguageId = ur.LanguageId
                    };
                    list.Add(localizedPropertyForCaching);
                }
                return list;
            });
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class UrlRecordForCaching
        {
            public int Id { get; set; }
            public int EntityId { get; set; }
            public string EntityName { get; set; }
            public string Slug { get; set; }
            public bool IsActive { get; set; }
            public int LanguageId { get; set; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an URL record
        /// </summary>
        /// <param name="urlRecord">URL record</param>
        public virtual void DeleteUrlRecord(UrlRecord urlRecord)
        {
            if (urlRecord == null)
                throw new ArgumentNullException("urlRecord");

            _urlRecordRepository.Delete(urlRecord);

            //cache
            _cacheManager.RemoveByPattern(URLRECORD_PATTERN_KEY);
        }

        /// <summary>
        /// Gets an URL record
        /// </summary>
        /// <param name="urlRecordId">URL record identifier</param>
        /// <returns>URL record</returns>
        public virtual UrlRecord GetUrlRecordById(int urlRecordId)
        {
            if (urlRecordId == 0)
                return null;

            return _urlRecordRepository.GetById(urlRecordId);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="urlRecord">URL record</param>
        public virtual void InsertUrlRecord(UrlRecord urlRecord)
        {
            if (urlRecord == null)
                throw new ArgumentNullException("urlRecord");

            _urlRecordRepository.Insert(urlRecord);

            //cache
            _cacheManager.RemoveByPattern(URLRECORD_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="urlRecord">URL record</param>
        public virtual void UpdateUrlRecord(UrlRecord urlRecord)
        {
            if (urlRecord == null)
                throw new ArgumentNullException("urlRecord");

            _urlRecordRepository.Update(urlRecord);

            //cache
            _cacheManager.RemoveByPattern(URLRECORD_PATTERN_KEY);
        }

        /// <summary>
        /// Find URL record
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <returns>Found URL record</returns>
        public virtual UrlRecord GetBySlug(string slug)
        {
            if (String.IsNullOrEmpty(slug))
                return null;

            var query = from ur in _urlRecordRepository.Table
                        where ur.Slug == slug
                        select ur;
            var urlRecord = query.FirstOrDefault();
            return urlRecord;
        }

        public virtual UrlRecord GetBySlug(string slug, out int categoryId, out int streetId, out int wardId, out int districtId, out int stateProvinceId, out string priceString, out string specAttributeOptionIds)
        {
            categoryId = streetId = wardId = districtId = stateProvinceId = 0;
            priceString = specAttributeOptionIds = "";
            if (String.IsNullOrEmpty(slug))
                return null;

            //prepare parameters
            var pSlug = _dataProvider.GetParameter();
            pSlug.ParameterName = "slug";
            pSlug.Value = slug;
            pSlug.DbType = DbType.String;
            //out
            var pCategoryId = _dataProvider.GetParameter();
            pCategoryId.ParameterName = "categoryId";
            pCategoryId.Direction = ParameterDirection.Output;
            pCategoryId.DbType = DbType.Int32;

            var pStreetId = _dataProvider.GetParameter();
            pStreetId.ParameterName = "streetId";
            pStreetId.Direction = ParameterDirection.Output;
            pStreetId.DbType = DbType.Int32;

            var pWardId = _dataProvider.GetParameter();
            pWardId.ParameterName = "wardId";
            pWardId.Direction = ParameterDirection.Output;
            pWardId.DbType = DbType.Int32;

            var pDistrictId = _dataProvider.GetParameter();
            pDistrictId.ParameterName = "districtId";
            pDistrictId.Direction = ParameterDirection.Output;
            pDistrictId.DbType = DbType.Int32;

            var pStateProvinceId = _dataProvider.GetParameter();
            pStateProvinceId.ParameterName = "stateProvinceId";
            pStateProvinceId.Direction = ParameterDirection.Output;
            pStateProvinceId.DbType = DbType.Int32;

            var pPriceString = _dataProvider.GetParameter();
            pPriceString.ParameterName = "priceString";
            pPriceString.Size = 100;
            pPriceString.Direction = ParameterDirection.Output;
            pPriceString.DbType = DbType.String;

            var pAttributeOptions = _dataProvider.GetParameter();
            pAttributeOptions.ParameterName = "attributeOptions";
            pAttributeOptions.Size = 200;
            pAttributeOptions.Direction = ParameterDirection.Output;
            pAttributeOptions.DbType = DbType.String;

            var urlRecords = _dbContext.ExecuteStoredProcedureList<UrlRecord>(
                    "GetUrlLink",
                    pSlug,
                    pCategoryId,
                    pStreetId,
                    pWardId,
                    pDistrictId,
                    pStateProvinceId,
                    pPriceString,
                    pAttributeOptions
                    );

            var urlRecord = urlRecords.FirstOrDefault();
            if (urlRecord != null)
            {
                categoryId = (pCategoryId.Value != DBNull.Value) ? Convert.ToInt32(pCategoryId.Value) : 0;
                streetId = (pStreetId.Value != DBNull.Value) ? Convert.ToInt32(pStreetId.Value) : 0;
                wardId = (pWardId.Value != DBNull.Value) ? Convert.ToInt32(pWardId.Value) : 0;
                districtId = (pDistrictId.Value != DBNull.Value) ? Convert.ToInt32(pDistrictId.Value) : 0;
                stateProvinceId = (pStateProvinceId.Value != DBNull.Value) ? Convert.ToInt32(pStateProvinceId.Value) : 0;
                priceString = pPriceString.Value.ToString();
                specAttributeOptionIds = pAttributeOptions.Value.ToString();
            }
            return urlRecord;
        }

        /// <summary>
        /// Gets all URL records
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Customer collection</returns>
        public virtual IPagedList<UrlRecord> GetAllUrlRecords(string slug, int pageIndex, int pageSize)
        {
            var query = _urlRecordRepository.Table;
            if (!String.IsNullOrWhiteSpace(slug))
                query = query.Where(ur => ur.Slug.Contains(slug));
            query = query.OrderBy(ur => ur.Slug);

            var urlRecords = new PagedList<UrlRecord>(query, pageIndex, pageSize);
            return urlRecords;
        }

        /// <summary>
        /// Find slug
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="entityName">Entity name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Found slug</returns>
        public virtual string GetActiveSlug(int entityId, string entityName, int languageId)
        {
            if (_localizationSettings.LoadAllUrlRecordsOnStartup)
            {
                string key = string.Format(URLRECORD_ACTIVE_BY_ID_NAME_LANGUAGE_KEY, entityId, entityName, languageId);
                return _cacheManager.Get(key, () =>
                {
                    //load all records (we know they are cached)
                    var source = GetAllUrlRecordsCached();
                    var query = from ur in source
                                where ur.EntityId == entityId &&
                                ur.EntityName == entityName &&
                                ur.LanguageId == languageId &&
                                ur.IsActive
                                orderby ur.Id descending
                                select ur.Slug;
                    var slug = query.FirstOrDefault();
                    //little hack here. nulls aren't cacheable so set it to ""
                    if (slug == null)
                        slug = "";
                    return slug;
                });
            }
            else
            {
                //gradual loading
                string key = string.Format(URLRECORD_ACTIVE_BY_ID_NAME_LANGUAGE_KEY, entityId, entityName, languageId);
                return _cacheManager.Get(key, () =>
                {
                    var source = _urlRecordRepository.Table;
                    var query = from ur in source
                                where ur.EntityId == entityId &&
                                ur.EntityName == entityName &&
                                ur.LanguageId == languageId &&
                                ur.IsActive
                                orderby ur.Id descending
                                select ur.Slug;
                    var slug = query.FirstOrDefault();
                    //little hack here. nulls aren't cacheable so set it to ""
                    if (slug == null)
                        slug = "";
                    return slug;
                });
            }
        }

        /// <summary>
        /// Save slug
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="slug">Slug</param>
        /// <param name="languageId">Language ID</param>
        public virtual void SaveSlug<T>(T entity, string slug, int languageId) where T : BaseEntity, ISlugSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            int entityId = entity.Id;
            string entityName = typeof(T).Name;

            var query = from ur in _urlRecordRepository.Table
                        where ur.EntityId == entityId &&
                        ur.EntityName == entityName &&
                        ur.LanguageId == languageId
                        orderby ur.Id descending 
                        select ur;
            var allUrlRecords = query.ToList();
            var activeUrlRecord = allUrlRecords.FirstOrDefault(x => x.IsActive);

            if (activeUrlRecord == null && !string.IsNullOrWhiteSpace(slug))
            {
                //find in non-active records with the specified slug
                var nonActiveRecordWithSpecifiedSlug = allUrlRecords
                    .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
                if (nonActiveRecordWithSpecifiedSlug != null)
                {
                    //mark non-active record as active
                    nonActiveRecordWithSpecifiedSlug.IsActive = true;
                    UpdateUrlRecord(nonActiveRecordWithSpecifiedSlug);
                }
                else
                {
                    //new record
                    var urlRecord = new UrlRecord()
                    {
                        EntityId = entity.Id,
                        EntityName = entityName,
                        Slug = NonUnicode(slug),
                        LanguageId = languageId,
                        IsActive = true,
                    };
                    InsertUrlRecord(urlRecord);
                }
            }

            if (activeUrlRecord != null && string.IsNullOrWhiteSpace(slug))
            {
                //disable the previous active URL record
                activeUrlRecord.IsActive = false;
                UpdateUrlRecord(activeUrlRecord);
            }

            if (activeUrlRecord != null && !string.IsNullOrWhiteSpace(slug))
            {
                //is it the same slug as in active URL record?
                if (activeUrlRecord.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                {
                    //yes. do nothing
                    //P.S. wrote this way for more source code readability
                }
                else
                {
                    //find in non-active records with the specified slug
                    var nonActiveRecordWithSpecifiedSlug = allUrlRecords
                        .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
                    if (nonActiveRecordWithSpecifiedSlug != null)
                    {
                        //mark non-active record as active
                        nonActiveRecordWithSpecifiedSlug.IsActive = true;
                        UpdateUrlRecord(nonActiveRecordWithSpecifiedSlug);

                        //disable the previous active URL record
                        activeUrlRecord.IsActive = false;
                        UpdateUrlRecord(activeUrlRecord);
                    }
                    else
                    {
                        //insert new record
                        //we do not update the existing record because we should track all previously entered slugs
                        //to ensure that URLs will work fine
                        var urlRecord = new UrlRecord()
                        {
                            EntityId = entity.Id,
                            EntityName = entityName,
                            Slug =NonUnicode(slug),
                            LanguageId = languageId,
                            IsActive = true,
                        };
                        InsertUrlRecord(urlRecord);

                        //disable the previous active URL record
                        activeUrlRecord.IsActive = false;
                        UpdateUrlRecord(activeUrlRecord);
                    }

                }
            }
        }

        /// <summary>
        /// Get Slug from id elements
        /// </summary>
        /// <param name="domainName">Domain name: zhouse.com</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="stateProvinceId">Stateprovince id</param>
        /// <param name="districtId">District id</param>
        /// <param name="wardId">Ward id</param>
        /// <param name="streetId">Street id</param>
        /// <param name="priceString">Price string: 1000-1500</param>
        /// <param name="attributeOptionIds">id specification options id:  1-2-3-4-5</param>
        /// <returns>Link request: http://zhouse.com/nha-o-quan-1_pr-1000-15000_sa-1-2-3-4-5</returns>
        public virtual string GetSlugFromId(string domainName, int categoryId = 0, int stateProvinceId = 0, int districtId = 0, int wardId = 0, int streetId = 0, string priceString = "", string @attributeOptionIds = "")
        {
            string slug = "";
            SqlParameter pSlug = new SqlParameter("slug", SqlDbType.NVarChar, 1000);
            pSlug.Direction = ParameterDirection.Output;

            var excute = _dbContext.ExecuteSqlCommand("execute [GetSlugFromId] @domainName, @categoryId, @stateProvinceId, @districtId, @wardId, @streetId, @priceString, @attibuteOptionIds, @slug output", false, null,
                new SqlParameter("domainName", domainName),
                new SqlParameter("categoryId", categoryId),
                new SqlParameter("stateProvinceId", stateProvinceId),
                new SqlParameter("districtId", districtId),
                new SqlParameter("wardId", wardId),
                new SqlParameter("streetId", streetId),
                new SqlParameter("priceString", priceString),
                new SqlParameter("attibuteOptionIds", attributeOptionIds),
                pSlug
                );

            return slug = pSlug.Value == DBNull.Value ? "" : pSlug.Value.ToString();
        }

        #endregion
    }
}