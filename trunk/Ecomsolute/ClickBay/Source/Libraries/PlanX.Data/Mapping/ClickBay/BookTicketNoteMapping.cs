using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class BookTicketNoteMapping : EntityTypeConfiguration<BookTicketNote>
    {
        public BookTicketNoteMapping()
        {
            this.ToTable("BookTicketNote");
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.Booking)
                .WithMany(x => x.BookTicketNotes)
                .HasForeignKey(x => x.BookTicketId);
        }
    }
}
