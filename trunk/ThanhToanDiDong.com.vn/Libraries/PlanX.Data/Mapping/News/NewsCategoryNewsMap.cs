using System.Data.Entity.ModelConfiguration;
using PlanX.Core.Domain.News;

namespace PlanX.Data.Mapping.News
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
                .WithMany(p => p.NewsCategoryNews)
                .HasForeignKey(pc => pc.NewsId).WillCascadeOnDelete(true); ;
        }
    }

}
