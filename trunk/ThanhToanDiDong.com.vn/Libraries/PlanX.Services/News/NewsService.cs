using System;
using System.Collections.Generic;
using System.Linq;
using PlanX.Core;
using PlanX.Core.Data;
using PlanX.Core.Domain.News;
using PlanX.Core.Domain.Stores;
using PlanX.Services.Events;
using System.Threading.Tasks;
using System.Transactions;

namespace PlanX.Services.News
{
    /// <summary>
    /// News service
    /// </summary>
    public partial class NewsService : INewsService
    {
        #region Fields

        private readonly IRepository<NewsItem> _newsItemRepository;
        private readonly IRepository<NewsComment> _newsCommentRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<NewsCategoryNews> _newsCategoryNews;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public NewsService(IRepository<NewsItem> newsItemRepository,
            IRepository<NewsComment> newsCommentRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IRepository<NewsCategoryNews> newsCategoryNews,
            IEventPublisher eventPublisher)
        {
            this._newsItemRepository = newsItemRepository;
            this._newsCommentRepository = newsCommentRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._newsCategoryNews = newsCategoryNews;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsItem">News item</param>
        public virtual void DeleteNews(NewsItem newsItem)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem");

            newsItem.Deleted = true;
            _newsItemRepository.Update(newsItem);

            //event notification
            _eventPublisher.EntityUpdated(newsItem);
        }

        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        public virtual NewsItem GetNewsById(int newsId)
        {
            if (newsId == 0)
                return null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }
                ))
            {
                return _newsItemRepository.GetById(newsId);
            }
        }
        public virtual async Task<NewsItem> GetNewsByIdAsync(int newsId)
        {
            if (newsId == 0)
                return null;
            return await Task.Factory.StartNew<NewsItem>(() => { return GetNewsById(newsId); });
        }

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="languageId">Language identifier; 0 if you want to get all records</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News items</returns>
        public virtual IPagedList<NewsItem> GetAllNews(int languageId, int storeId,
            int pageIndex, int pageSize, bool showHidden = false, List<int> categoryNewsIds = null,
            int newsTagId = 0, bool isHostView = false, bool isMostView = false,
            bool includeBannerItem = false)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }
            ))
            {
                var query = _newsItemRepository.Table;
                query = query.Where(n => !n.Deleted);

                if (languageId > 0)
                    query = query.Where(n => languageId == n.LanguageId);
                if (includeBannerItem)
                    query = query.Where(n => n.IsShowSlider);
                if (!showHidden)
                {
                    var utcNow = DateTime.Now;
                    query = query.Where(n => n.Published);
                    query = query.Where(n => !n.StartDateUtc.HasValue || n.StartDateUtc <= utcNow);
                    query = query.Where(n => !n.EndDateUtc.HasValue || n.EndDateUtc >= utcNow);
                }
                if (categoryNewsIds != null && categoryNewsIds.Count() > 0)
                {
                    query = from a in query
                            join cn in _newsCategoryNews.Table on a.Id equals cn.NewsId
                            where categoryNewsIds.Any(x => x == cn.CategoryNewsId)
                            select a;
                }
                if (newsTagId > 0)
                {
                    query = query.Where(t => t.Tags.Any(x => x.Id == newsTagId));
                }
                if (isHostView)
                    query = query.Where(n => n.IsHotView);


                //Store mapping
                #region
                if (storeId > 0)
                {
                    query = from n in query
                            join sm in _storeMappingRepository.Table
                            on new { c1 = n.Id, c2 = "NewsItem" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into n_sm
                            from sm in n_sm.DefaultIfEmpty()
                            where !n.LimitedToStores || storeId == sm.StoreId
                            select n;

                    //only distinct items (group by ID)
                    query = from n in query
                            group n by n.Id
                                into nGroup
                                orderby nGroup.Key
                                select nGroup.FirstOrDefault();
                    query = query.OrderByDescending(n => n.CreatedOnUtc);
                }
                #endregion
                query = query.Distinct();
                if (isMostView)
                    query = query.OrderByDescending(n => n.ViewCount);

                query = query.OrderByDescending(n => n.CreatedOnUtc);


                var news = new PagedList<NewsItem>(query, pageIndex, pageSize);
                return news;
            }
        }
        public virtual async Task<IPagedList<NewsItem>> GetAllNewsAsync(int languageId, int storeId,
            int pageIndex, int pageSize, bool showHidden = false, List<int> categoryNewsIds = null, int newsTagId = 0, bool isHostView = false, bool isMostView = false, bool includeBannerItem = false)
        {
            return await Task.Factory.StartNew<IPagedList<NewsItem>>(() => { return GetAllNews(languageId, storeId, pageIndex, pageSize, showHidden, categoryNewsIds, newsTagId, isHostView, isMostView, includeBannerItem); });
        }

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="news">News item</param>
        public virtual void InsertNews(NewsItem news)
        {
            if (news == null)
                throw new ArgumentNullException("news");

            _newsItemRepository.Insert(news);

            //event notification
            _eventPublisher.EntityInserted(news);
        }

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="news">News item</param>
        public virtual void UpdateNews(NewsItem news)
        {
            if (news == null)
                throw new ArgumentNullException("news");

            _newsItemRepository.Update(news);

            //event notification
            _eventPublisher.EntityUpdated(news);
        }

        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <returns>Comments</returns>
        public virtual IPagedList<NewsComment> GetAllComments(int customerId, int newsId = 0, int parentId = 0,bool onlyApprove=false,
            int pageIndex = 0, int pageSize = 20)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }
                ))
            {
                var query =  _newsCommentRepository.Table.Where(x=>!x.Deleted);
               
                if (onlyApprove)
                    query = query.Where(x => x.IsApproved);                
                if (customerId != 0)
                    query = query.Where(x => x.CustomerId == customerId);
                if (newsId != 0)
                    query = query.Where(x => x.NewsItemId == newsId);
                if (parentId != 0)
                    query = query.Where(x => x.ParentId == parentId);
                query = query.OrderByDescending(x => x.CreatedOnUtc);
                return new PagedList<NewsComment>(query, pageIndex, pageSize);
            }
        }
        public virtual async Task<IPagedList<NewsComment>> GetAllCommentsAsync(int customerId, int newsId = 0, int parentId = 0,bool onlyApprove=false, int pageIndex = 0, int pageSize = 20)
        {
            return await Task.Factory.StartNew<IPagedList<NewsComment>>(() => { return GetAllComments(customerId,newsId,parentId,onlyApprove,pageIndex,pageSize); });
        }

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifier</param>
        /// <returns>News comment</returns>
        public virtual NewsComment GetNewsCommentById(int newsCommentId)
        {
            if (newsCommentId == 0)
                return null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }
                ))
            {
                return _newsCommentRepository.GetById(newsCommentId);
            }
        }
        public virtual async Task<NewsComment> GetNewsCommentByIdAsync(int newsCommentId)
        {
            return await Task.Factory.StartNew<NewsComment>(() => { return GetNewsCommentById(newsCommentId); });
        }

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsComment">News comment</param>
        public virtual void DeleteNewsComment(NewsComment newsComment)
        {
            if (newsComment == null)
                throw new ArgumentNullException("newsComment");

            _newsCommentRepository.Delete(newsComment);
        }
       public virtual void UpdateNewsComment(NewsComment entity)
        {
            if (entity == null)
                throw new ArgumentNullException("NewsComment");
            _newsCommentRepository.Update(entity);
       }

        public virtual Task<IPagedList<NewsItem>> GetAllNewShowBanner(int languageid, int pageSize = 0)
        {
            return Task.Factory.StartNew<IPagedList<NewsItem>>(() =>
            {
                return GetAllNews(languageid, 0, 0, pageSize, includeBannerItem: true);
            });
        }

        #endregion
    }
}
