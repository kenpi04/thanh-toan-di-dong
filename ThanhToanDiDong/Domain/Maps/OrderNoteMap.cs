using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
   public class OrderNoteMap:EntityTypeConfiguration<OrderNote>
    {
       public OrderNoteMap()
       {
           this.ToTable("OrderNote");
           this.HasKey(x => x.Id);
           this.HasRequired(o => o.Order)
             .WithMany(od => od.OrderNotes)
             .HasForeignKey(o => o.OrderId);
       }
    }
}
