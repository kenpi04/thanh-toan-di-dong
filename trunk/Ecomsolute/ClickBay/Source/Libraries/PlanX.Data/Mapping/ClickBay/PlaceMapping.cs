using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
   public class PlaceMapping:EntityTypeConfiguration<Place>
    {
       public PlaceMapping()
       {
           this.ToTable("Place");
           this.HasKey(x => x.Id);
           
       }
    }
}
