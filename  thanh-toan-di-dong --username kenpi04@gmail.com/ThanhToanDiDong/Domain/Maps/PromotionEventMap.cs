using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
    public class PromotionEventMap : EntityTypeConfiguration<PromotionEvent>
    {
        public PromotionEventMap()
       {
           this.ToTable("PromotionEvent");
           this.HasKey(x => x.Id);
          
       }
    }
}
