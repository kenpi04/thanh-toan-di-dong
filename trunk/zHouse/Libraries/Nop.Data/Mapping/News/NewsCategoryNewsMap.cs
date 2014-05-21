using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.News;

namespace Nop.Data.Mapping.News
{
    public partial class NewsCategoryNewsMap : EntityTypeConfiguration<NewsCategoryNews>
    {
        public NewsCategoryNewsMap()
        {
            this.ToTable("News_CategoryNews_Mapping");
            this.HasKey(pc => pc.Id);

            this.HasRequired(pc => pc.CategoryNews)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryNewsId).WillCascadeOnDelete(true); ;

            this.HasRequired(pc => pc.NewsItem)
                .WithMany(p => p.NewsCategoriesMap)
                .HasForeignKey(pc => pc.NewsId).WillCascadeOnDelete(true); ;
        }
    }

}
