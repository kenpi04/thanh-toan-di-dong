using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
  public  class FlightCountryMapping:EntityTypeConfiguration<FlightCountry>
    {
      public FlightCountryMapping()
      {
          this.ToTable("FlightCountry");
          this.HasKey(x => x.Id);
      }
    }
}
