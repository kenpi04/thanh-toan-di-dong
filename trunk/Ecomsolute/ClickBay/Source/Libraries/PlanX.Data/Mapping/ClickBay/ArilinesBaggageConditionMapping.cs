using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
public class ArilinesBaggageConditionMapping : EntityTypeConfiguration<ArilinesBaggageCondition>
    {
    public ArilinesBaggageConditionMapping()
    {
        this.ToTable("ArilinesBaggageCondition");
        this.HasKey(x => x.Id);
     
    }
    }
}
