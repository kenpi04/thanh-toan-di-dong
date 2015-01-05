using PlanX.Core.Domain.News;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.News
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            this.ToTable("Tags");
            this.HasKey(pt => pt.Id);
            this.Property(pt => pt.Name).IsRequired().HasMaxLength(400);

            this.Ignore(pt => pt.NewsItems);            
        }
    }
}
