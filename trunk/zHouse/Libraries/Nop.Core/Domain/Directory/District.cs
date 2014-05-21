using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Directory
{  
    public partial class District : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Stateprovince Id
        /// </summary>
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
