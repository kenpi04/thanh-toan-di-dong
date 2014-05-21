using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Directory;

namespace Nop.Data.Mapping.Directory
{
    public partial class FBannerMap : EntityTypeConfiguration<FBanner>
    {
        public FBannerMap()
        {
            this.ToTable("Banner");
            this.HasKey(fb => fb.Id);
            this.Property(fb => fb.Name).IsRequired();            
        }
    }
}
