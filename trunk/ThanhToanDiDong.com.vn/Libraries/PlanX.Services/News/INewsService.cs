using System.Collections.Generic;
using PlanX.Core;
using PlanX.Core.Domain.News;
using System.Threading.Tasks;

namespace PlanX.Services.News
{
    /// <summary>
    /// News service interface
    /// </summary>
    public partial interface INewsService
    {
        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsItem">News item</param>
        void DeleteNews(NewsItem newsItem);

        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        NewsItem GetNewsById(int newsId);
        Task<NewsItem> GetNewsByIdAsync(int newsId);

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="languageId">Language identifier; 0 if you want to get all records</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News items</returns>
        IPagedList<NewsItem> GetAllNews(int languageId, int storeId,
            int pageIndex, int pageSize, bool showHidden = false, List<int> categoryNewsIds = null,
            int newsTagId = 0, bool isHostView = false, bool isMostView = false,
            bool includeBannerItem = false);
        Task<IPagedList<NewsItem>> GetAllNewsAsync(int languageId, int storeId,
            int pageIndex, int pageSize, bool showHidden = false, List<int> categoryNewsIds = null, int newsTagId = 0, bool isHostView = false, bool isMostView = false, bool includeBannerItem = false);

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="news">News item</param>
        void InsertNews(NewsItem news);

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="news">News item</param>
        void UpdateNews(NewsItem news);

        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <returns>Comments</returns>
        IList<NewsComment> GetAllComments(int customerId);
        Task<IList<NewsComment>> GetAllCommentsAsync(int customerId);
        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifier</param>
        /// <returns>News comment</returns>
        NewsComment GetNewsCommentById(int newsCommentId);
        Task<NewsComment> GetNewsCommentByIdAsync(int newsCommentId);
        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsComment">News comment</param>
        void DeleteNewsComment(NewsComment newsComment);

        Task<IPagedList<NewsItem>> GetAllNewShowBanner(int languageid, int pageSize = 0);

    }
}
