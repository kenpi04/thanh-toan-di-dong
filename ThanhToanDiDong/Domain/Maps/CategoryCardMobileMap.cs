using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
   public class CategoryCardMobileMap:EntityTypeConfiguration<CategoryCardMobile>
    {
       public CategoryCardMobileMap()
       {
           this.ToTable("CategoryCardMobile");
           this.HasKey(x => x.Id);
        
       }
    }
}
