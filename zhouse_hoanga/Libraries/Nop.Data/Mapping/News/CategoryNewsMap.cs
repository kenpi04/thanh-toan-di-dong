using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.News;

namespace Nop.Data.Mapping.News
{
    public partial class CategoryNewsMap : EntityTypeConfiguration<CategoryNews>
    {
        public CategoryNewsMap()
        {
            this.ToTable("CategoryNews");
            this.HasKey(c => c.Id);

            this.Property(c => c.Name).IsRequired().HasMaxLength(400);
        }
    }

}
