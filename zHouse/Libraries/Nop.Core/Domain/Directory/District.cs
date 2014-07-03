using System.Collections.Generic;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Directory
{
    public partial class District : BaseEntity, ISlugSupported
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
        private ICollection<Ward> _Wards;
        public virtual ICollection<Ward> Wards
        {
            get { return _Wards ?? (_Wards = new List<Ward>()); }
            protected set { this._Wards = value; }
        }
        private ICollection<Street> _Streets ;
        public virtual ICollection<Street> Streets
        {
            get { return _Streets ?? (_Streets = new List<Street>()); }
            protected set { this._Streets = value; }
        }
        public virtual StateProvince StateProvince { get; set; }




       











    }
}
