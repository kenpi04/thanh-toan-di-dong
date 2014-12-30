using System.Data.Entity.ModelConfiguration;
using PlanX.Core.Domain.Common;

namespace PlanX.Data.Mapping.Common
{
    public partial class BannerMap : EntityTypeConfiguration<Banner>
    {
        public BannerMap()
        {
            this.ToTable("Banner");
            this.HasKey(b => b.Id);
            this.Property(b => b.Name).IsRequired();
        }
    }
}
