using PlanX.Core.Domain.Localization;
using System;

namespace PlanX.Core.Domain.Common
{
    /// <summary>
    /// Create by Tiennn
    /// </summary>
    public partial class Banner : BaseEntity
    {
        /// <summary>
        /// Get or set the PictureId
        /// </summary>
        public virtual int PictureId { get; set; }
        //public virtual int StateProvinceId { get; set; }

        /// <summary>
        /// Get or set the position
        /// </summary>
        public virtual int Position { get; set; }

        /// <summary>
        /// Get or set the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set the title
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Get or set the description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Get or set the type
        /// </summary>
        public virtual int Type { get; set; }

        /// <summary>
        /// Get or set the target
        /// </summary>
        public virtual string Target { get; set; }

        /// <summary>
        /// Get or set the url
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Get or set the displayorder
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Get or set the published
        /// </summary>
        public virtual bool Published { get; set; }

        /// <summary>
        /// Get or set the start date
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Get or set the end date
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }
}
