using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlanX.Services.News
{
    public static class NewsExtension
    {
        /// <summary>
        /// Indicates whether a news tag exists
        /// </summary>
        /// <param name="newsitem">NewsItem</param>
        /// <param name="productTagId">Tag identifier</param>
        /// <returns>Result</returns>
        public static bool TagExists(this PlanX.Core.Domain.News.NewsItem newsItem,
            int tagId)
        {
            if (newsItem == null)
                throw new ArgumentNullException("news item");

            bool result = newsItem.Tags.ToList().Find(pt => pt.Id == tagId) != null;
            return result;
        }

        /// <summary>
        /// Indicates whether a news tag exists
        /// </summary>
        /// <param name="newsitem">NewsItem</param>
        /// <param name="productTagId">Tag identifier</param>
        /// <returns>Result</returns>
        public static async Task<bool> TagExistsAsync(this PlanX.Core.Domain.News.NewsItem newsItem,
            int tagId)
        {
            return await Task.Factory.StartNew<bool>(() => { return TagExists(newsItem, tagId); });
        }
    }
}
