using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Directory
{
    public partial class Street : BaseEntity
    {
        /// <summary>
        /// Get or set the district id
        /// </summary>
        public virtual int DistrictId { get; set; }
        /// <summary>
        /// Get or set Name
        /// </summary>
        public virtual string Name { get; set; }
        public virtual District District { get; set; }

    }
}
