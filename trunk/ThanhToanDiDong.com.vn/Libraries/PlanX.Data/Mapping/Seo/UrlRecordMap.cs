using System.Data.Entity.ModelConfiguration;
using PlanX.Core.Domain.Seo;

namespace PlanX.Data.Mapping.Seo
{
    public partial class UrlRecordMap : EntityTypeConfiguration<UrlRecord>
    {
        public UrlRecordMap()
        {
            this.ToTable("UrlRecord");
            this.HasKey(lp => lp.Id);

            this.Property(lp => lp.EntityName).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.Slug).IsRequired().HasMaxLength(400);
        }
    }
}