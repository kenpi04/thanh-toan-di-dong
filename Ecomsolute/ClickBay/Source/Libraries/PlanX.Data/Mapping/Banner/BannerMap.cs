using System.Data.Entity.ModelConfiguration;
using PlanX.Core.Domain.Banner;

namespace PlanX.Data.Mapping.Directory
{
    public partial class BannerMap : EntityTypeConfiguration<Banner>
    {
        public BannerMap()
        {
            this.ToTable("Banner");
            this.HasKey(fb => fb.Id);
            this.Property(fb => fb.Name).IsRequired();            
        }
    }
}
