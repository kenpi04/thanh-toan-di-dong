
namespace Nop.Core.Domain.Directory
{
    public partial class Ward : BaseEntity
    {
        /// <summary>
        /// Gets or sets the district id
        /// </summary>
        public virtual int DistrictId { get; set; }


        /// <summary>
        /// Gets or sets name
        /// </summary>
        public virtual string Name { get; set; }
        public virtual District District { get; set; }
    }
}
