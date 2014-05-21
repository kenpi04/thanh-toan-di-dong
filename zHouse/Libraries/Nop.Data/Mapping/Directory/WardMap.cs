using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Directory;

namespace Nop.Data.Mapping.Directory
{
    public partial class WardMap : EntityTypeConfiguration<Ward>
    {
        public WardMap()
        {
            this.ToTable("Ward");
            this.HasKey(fw => fw.Id);
            this.Property(fw => fw.Name).IsRequired().HasMaxLength(100);
        }
    }
}
