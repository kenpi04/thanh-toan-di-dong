using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
  public  class AirportMapping:EntityTypeConfiguration<Airport>
    {
      public AirportMapping()
      {
          this.ToTable("Airport");
          this.HasKey(x => x.Id);
      }
    }
}
