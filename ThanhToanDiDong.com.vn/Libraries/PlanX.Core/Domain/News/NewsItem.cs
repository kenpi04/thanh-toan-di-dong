using System;
using System.Collections.Generic;
using PlanX.Core.Domain.Localization;
using PlanX.Core.Domain.Seo;
using PlanX.Core.Domain.Stores;

namespace PlanX.Core.Domain.News
{
    /// <summary>
    /// Represents a news item
    /// </summary>
    public partial class NewsItem : BaseEntity, ISlugSupported, IStoreMappingSupported
    {
        private ICollection<NewsComment> _newsComments;
        private ICollection<NewsCategoryNews> _newsCategoriesNews;
        private ICollection<Tag> _tags;

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the news title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short text
        /// </summary>
        public string Short { get; set; }

        /// <summary>
        /// Gets or sets the full text
        /// </summary>
        public string Full { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the news item is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the news item start date and time
        /// </summary>
        public DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the news item end date and time
        /// </summary>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the news post comments are allowed 
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        /// Gets or sets the total number of comments
        /// <remarks>
        /// We use this property for performance optimization (no SQL command executed)
        /// </remarks>
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the pictureid
        /// </summary>
        public int? PictureId { get; set; }

        /// <summary>
        /// Gets or sets the deleted
        /// </summary>
        public bool Deleted { get; set; }

        public bool IsShowSlider { get; set; }

        /// <summary>
        /// Gets or set Viewcount
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Gets or set Is hot view
        /// </summary>
        public bool IsHotView { get; set; }

        /// <summary>
        /// Gets or sets the news comments
        /// </summary>
        public virtual ICollection<NewsComment> NewsComments
        {
            get { return _newsComments ?? (_newsComments = new List<NewsComment>()); }
            protected set { _newsComments = value; }
        }

        /// <summary>
        /// Gets or sets the news comments
        /// </summary>
        public virtual ICollection<NewsCategoryNews> NewsCategoryNews
        {
            get { return _newsCategoriesNews ?? (_newsCategoriesNews = new List<NewsCategoryNews>()); }
            protected set { _newsCategoriesNews = value; }
        }
        
        public virtual ICollection<Tag> Tags
        {
            get { return _tags ?? (_tags = new List<Tag>()); }
            protected set { _tags = value; }
        }

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public virtual Language Language { get; set; }
    }
}