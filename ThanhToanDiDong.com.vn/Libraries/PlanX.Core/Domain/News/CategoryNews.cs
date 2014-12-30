using System;
using PlanX.Core.Domain.Seo;

namespace PlanX.Core.Domain.News
{
    public partial class CategoryNews : BaseEntity, ISlugSupported
    {
        /// <summary>
        /// Get or set the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set the MetaTitle
        /// </summary>
        public virtual string MetaTitle { get; set; }

        /// <summary>
        /// Get or set the MetaKeywords
        /// </summary>
        public virtual string MetaKeywords { get; set; }

        /// <summary>
        /// Get or set the MetaDescription
        /// </summary>
        public virtual string MetaDescription { get; set; }

        /// <summary>
        /// Get or set the Parent category news id
        /// </summary>
        public virtual int ParentCategoryNewsId { get; set; }

        /// <summary>
        /// Get or set the Pagesize
        /// </summary>
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Get or set the  allow customers to select Pagesize
        /// </summary>
        public virtual bool AllowCustomersToSelectPageSize { get; set; }

        /// <summary>
        /// Get or set the PagesizeOptions
        /// </summary>
        public virtual string PageSizeOptions { get; set; }

        /// <summary>
        /// Get or set the Published
        /// </summary>
        public virtual bool Published { get; set; }

        /// <summary>
        /// Get or set the Deleted
        /// </summary>
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// Get or set the DisplayOrder
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Get or set the CreatedOnUtc
        /// </summary>
        public virtual DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Get or set the UpdatedOnUtc
        /// </summary>
        public virtual DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Get or set the NewsCount
        /// </summary>
        public virtual int NewsCount  { get; set; }
    }

}
