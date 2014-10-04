using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Stores;
using Nop.Services.Events;
using System.Transactions;
using System.Threading.Tasks;

namespace Nop.Services.News
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
            this._newsCategoryNews = newsCategoryNews;
            this._newsItemRepository = newsItemRepository;
            this._newsCommentRepository = newsCommentRepository;
            this._storeMappingRepository = storeMappingRepository;
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

            _newsItemRepository.Delete(newsItem);
            
            //event notification
            _eventPublisher.EntityDeleted(newsItem);
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

            return _newsItemRepository.GetById(newsId);
        }
        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        public virtual async Task<NewsItem> GetNewsByIdAsync(int newsId)
        {
            if (newsId == 0)
                return null;

            return await System.Threading.Tasks.Task.Factory.StartNew<NewsItem>(() => { return _newsItemRepository.GetById(newsId); });
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
        public virtual IPagedList<NewsItem> GetAllNews(int languageId, int storeId,int cateid,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = _newsItemRepository.Table;
                if (languageId > 0)
                    query = query.Where(n => languageId == n.LanguageId);
                if (!showHidden)
                {
                    var utcNow = DateTime.UtcNow;
                    query = query.Where(n => n.Published);
                    query = query.Where(n => !n.StartDateUtc.HasValue || n.StartDateUtc <= utcNow);
                    query = query.Where(n => !n.EndDateUtc.HasValue || n.EndDateUtc >= utcNow);
                }
                if (cateid > 0)
                {
                    query = from a in query
                            join cn in _newsCategoryNews.Table on a.Id equals cn.NewsId
                            where cn.CategoryNewsId == cateid
                            select a;


                }
                query = query.OrderByDescending(n => n.CreatedOnUtc);

                //Store mapping
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

                var news = new PagedList<NewsItem>(query, pageIndex, pageSize);
                return news;
            }
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
        public virtual async Task<IPagedList<NewsItem>> GetAllNewsAsync(int languageId, int storeId, int cateid,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<IPagedList<NewsItem>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }))
                {
                    var query = _newsItemRepository.Table;
                    if (languageId > 0)
                        query = query.Where(n => languageId == n.LanguageId);
                    if (!showHidden)
                    {
                        var utcNow = DateTime.UtcNow;
                        query = query.Where(n => n.Published);
                        query = query.Where(n => !n.StartDateUtc.HasValue || n.StartDateUtc <= utcNow);
                        query = query.Where(n => !n.EndDateUtc.HasValue || n.EndDateUtc >= utcNow);
                    }
                    if (cateid > 0)
                    {
                        query = from a in query
                                join cn in _newsCategoryNews.Table on a.Id equals cn.NewsId
                                where cn.CategoryNewsId == cateid
                                select a;
                    }
                    query = query.OrderByDescending(n => n.CreatedOnUtc);

                    //Store mapping
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

                    var news = new PagedList<NewsItem>(query, pageIndex, pageSize);
                    return news;
                }
            });
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
        public virtual IList<NewsComment> GetAllComments(int customerId)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = from c in _newsCommentRepository.Table
                            orderby c.CreatedOnUtc
                            where (customerId == 0 || c.CustomerId == customerId)
                            select c;
                var content = query.ToList();
                return content;
            }
        }
        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <returns>Comments</returns>
        public virtual async Task<IList<NewsComment>> GetAllCommentsAsync(int customerId)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<IList<NewsComment>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = from c in _newsCommentRepository.Table
                                orderby c.CreatedOnUtc
                                where (customerId == 0 || c.CustomerId == customerId)
                                select c;
                    var content = query.ToList();
                    return content;
                }
            });
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

            return _newsCommentRepository.GetById(newsCommentId);
        }
        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifier</param>
        /// <returns>News comment</returns>
        public virtual async Task<NewsComment> GetNewsCommentByIdAsync(int newsCommentId)
        {
            if (newsCommentId == 0)
                return null;

            return await System.Threading.Tasks.Task.Factory.StartNew<NewsComment>(() => { return _newsCommentRepository.GetById(newsCommentId); });
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

        #endregion
    }
}
