using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
   public class CardMobileMap:EntityTypeConfiguration<CardMobile>
    {
       public CardMobileMap()
       {
           this.ToTable("CardMobile");
           this.HasKey(x => x.Id);
           this.HasRequired(c => c.CategoryCardMobile)
             .WithMany(cc => cc.CardMobile)
             .HasForeignKey(c => c.CategoryCardMobileId);
       }
    }
}
