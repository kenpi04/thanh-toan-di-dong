using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Admin.Models.Directory
{
    public class DistrictModel : BaseNopEntityModel
    {
        public DistrictModel()
        {
            StateProvinces = new List<SelectListItem>();
        }

        /// <summary>
        /// Gets or sets the Stateprovince Id
        /// </summary>
        [NopResourceDisplayName("Admin.Configuration.Countries.District.Fields.StateProvinceId")]
        public int StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [NopResourceDisplayName("Admin.Configuration.Countries.District.Fields.Name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [NopResourceDisplayName("Admin.Configuration.Countries.District.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Get or sets Published
        /// </summary>
        [NopResourceDisplayName("Admin.Configuration.Countries.District.Fields.Published")]
        public bool Published { get; set; }

        public IList<SelectListItem> StateProvinces { get; set; }
    }
}