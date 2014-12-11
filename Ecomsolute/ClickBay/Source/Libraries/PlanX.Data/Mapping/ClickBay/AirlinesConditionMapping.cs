using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class AirlinesConditionMapping : EntityTypeConfiguration<AirlinesCondition>
    {
        public AirlinesConditionMapping()
        {
            this.ToTable("AirlinesCondition");
            this.HasKey(x => x.Id);
           
        }
    }
}
