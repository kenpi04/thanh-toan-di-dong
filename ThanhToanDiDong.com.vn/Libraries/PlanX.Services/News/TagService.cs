using PlanX.Core.Caching;
using PlanX.Core.Data;
using PlanX.Core.Domain.Common;
using PlanX.Core.Domain.News;
using PlanX.Data;
using PlanX.Services.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PlanX.Services.News
{
    public class TagService : ITagService
    {
		#region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string NEWSTAG_COUNT_KEY = "planx.newstag.count-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string NEWSTAG_PATTERN_KEY = "planx.newstag.";

        #endregion

        #region Fields

        private readonly IRepository<Tag> _tagRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly CommonSettings _commonSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tagRepository">tag repository</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event published</param>
        public TagService(IRepository<Tag> tagRepository,
            IDataProvider dataProvider, 
            IDbContext dbContext,
            CommonSettings commonSettings,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher)
        {
            this._tagRepository = tagRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._commonSettings = commonSettings;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Nested classes

        private class NewsTagWithCount
        {
            public int TagId { get; set; }
            public int NewsCount { get; set; }
        }

        #endregion
        
        #region Utilities

        /// <summary>
        /// Get product count for each of existing product tag
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Dictionary of "product tag ID : product count"</returns>
        private Dictionary<int, int> GetNewsCount(int storeId)
        {
            string key = string.Format(NEWSTAG_COUNT_KEY, storeId);
            return _cacheManager.Get(key, () =>
            {

                if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
                {
                    //stored procedures are enabled and supported by the database. 
                    //It's much faster than the LINQ implementation below 

                    #region Use stored procedure

                    //prepare parameters
                    var pStoreId = _dataProvider.GetParameter();
                    pStoreId.ParameterName = "StoreId";
                    pStoreId.Value = storeId;
                    pStoreId.DbType = DbType.Int32;


                    //invoke stored procedure
                    var result = _dbContext.SqlQuery<NewsTagWithCount>(
                        "Exec NewsTagCountLoadAll @StoreId",
                        pStoreId);

                    var dictionary = new Dictionary<int, int>();
                    foreach (var item in result)
                        dictionary.Add(item.TagId, item.NewsCount);
                    return dictionary;

                    #endregion
                }
                else
                {
                    //stored procedures aren't supported. Use LINQ
                    #region Search products
                    var query = from pt in _tagRepository.Table
                                select new
                                {
                                    Id = pt.Id,
                                    NewsCount = pt.NewsItems
                                        //published and not deleted products
                                        .Count(p => !p.Deleted && p.Published)
                                    //UNDONE filter by store identifier if specified ( > 0 )
                                };

                    var dictionary = new Dictionary<int, int>();
                    foreach (var item in query)
                        dictionary.Add(item.Id, item.NewsCount);
                    return dictionary;

                    #endregion

                }
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a tag
        /// </summary>
        /// <param name="tag">Tag</param>
        public virtual void DeleteTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            _tagRepository.Delete(tag);

            //cache
            _cacheManager.RemoveByPattern(NEWSTAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(tag);
        }

        /// <summary>
        /// Gets all product tags
        /// </summary>
        /// <returns>Product tags</returns>
        public virtual IList<Tag> GetAllTags(bool isShowHomePage = false)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }
            ))
            {
                var query = _tagRepository.Table;
                if (isShowHomePage)
                    query = query.Where(x => x.IsShowHomePage);
                var tags = query.ToList();
                return tags;
            }
        }
        public virtual async Task<IList<Tag>> GetAllTagsAsync(bool isShowHomePage = false)
        {
            return await Task.Factory.StartNew<IList<Tag>>(() => GetAllTags(isShowHomePage));
        }

        /// <summary>
        /// Gets tag
        /// </summary>
        /// <param name="tagId">tag identifier</param>
        /// <returns>Tag</returns>
        public virtual Tag GetTagById(int tagId)
        {
            if (tagId == 0)
                return null;

            return _tagRepository.GetById(tagId);
        }

        /// <summary>
        /// Gets tag by name
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <returns>Tag</returns>
        public virtual Tag GetTagByName(string name)
        {
            var query = from pt in _tagRepository.Table
                        where pt.Name == name
                        select pt;

            var tag = query.FirstOrDefault();
            return tag;
        }
        public virtual IList<Tag> GetAllTagsByCategoryNewsId(int categoryNewsId, int pageSize=10)
        {
            if (categoryNewsId == 0)
                return null;

            var pCategoryId = _dataProvider.GetParameter();
            pCategoryId.ParameterName = "categoryId";
            pCategoryId.Value = categoryNewsId;
            pCategoryId.DbType = DbType.Int32;

            var pPageSize = _dataProvider.GetParameter();
            pPageSize.ParameterName = "pageSize";
            pPageSize.Value = pageSize;
            pPageSize.DbType = DbType.Int32;

            var tags = _dbContext.ExecuteStoredProcedureList<Tag>(
                "GetAllTagsByCategory",
                pCategoryId,
                pPageSize);

            return tags;
        }

        /// <summary>
        /// Inserts a tag
        /// </summary>
        /// <param name="Tag">Tag</param>
        public virtual void InsertTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            _tagRepository.Insert(tag);

            //cache
            _cacheManager.RemoveByPattern(NEWSTAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(tag);
        }

        /// <summary>
        /// Updates the tag
        /// </summary>
        /// <param name="tag">Tag</param>
        public virtual void UpdateTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            _tagRepository.Update(tag);

            //cache
            _cacheManager.RemoveByPattern(NEWSTAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(tag);
        }

        /// <summary>
        /// Get number of news
        /// </summary>
        /// <param name="tagId">tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Number of news</returns>
        public virtual int GetNewsCount(int tagId, int storeId)
        {
            var dictionary = GetNewsCount(storeId);
            if (dictionary.ContainsKey(tagId))
                return dictionary[tagId];
            else
                return 0;
        }

        #endregion
    }
}
