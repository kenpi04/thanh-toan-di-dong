using Nop.Core.Domain.Directory;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Data.Mapping.Directory
{
    public partial class StreetMap : EntityTypeConfiguration<Street>
    {
        public StreetMap()
        {
            this.ToTable("Street");
            this.HasKey(fs => fs.Id);
            this.Property(fs => fs.Name).IsRequired().HasMaxLength(100);
        }
    }
}
