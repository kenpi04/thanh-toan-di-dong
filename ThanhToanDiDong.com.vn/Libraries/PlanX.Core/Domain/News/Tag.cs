using System.Collections.Generic;

namespace PlanX.Core.Domain.News
{
    public class Tag : BaseEntity
    {
        private ICollection<NewsItem> _newsItems;
        /// <summary>
        /// Get or set name tags
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Get or set english name tags
        /// </summary>
        public string EnglishName { get; set; }
        /// <summary>
        /// Get or set is show home page
        /// </summary>
        public bool IsShowHomePage { get; set; }
        /// <summary>
        /// Gets or sets the newsitem
        /// </summary>
        public virtual ICollection<NewsItem> NewsItems
        {
            get { return _newsItems ?? (_newsItems = new List<NewsItem>()); }
            protected set { _newsItems = value; }
        }
    }
}
