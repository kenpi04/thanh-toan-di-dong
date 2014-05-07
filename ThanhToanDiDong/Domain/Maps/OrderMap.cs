using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
   public class OrderMap:EntityTypeConfiguration<Order>
    {
       public OrderMap()
       {
           this.ToTable("Order");
           this.HasKey(x => x.Id);
       
       }
    }
}
