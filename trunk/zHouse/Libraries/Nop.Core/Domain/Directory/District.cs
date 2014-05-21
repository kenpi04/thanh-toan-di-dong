using System;
using Nop.Core.Domain.Localization;
using System.Collections.Generic;

namespace Nop.Core.Domain.Directory
{
    /// <summary>
    /// Represents a FrtDistrict
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 20/09/2012      XuanDT@fpt.com.vn               Created.
    /// 
    /// </summary>
    public partial class District : BaseEntity, ILocalizedEntity
    {

        public virtual int StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }
        public virtual StateProvince StateProvince { get; set; }




       











    }
}
