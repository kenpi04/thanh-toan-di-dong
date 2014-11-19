using System.Data.Entity.ModelConfiguration;
using PlanX.Core.Domain.Messages;

namespace PlanX.Data.Mapping.Messages
{
    public partial class CampaignMap : EntityTypeConfiguration<Campaign>
    {
        public CampaignMap()
        {
            this.ToTable("Campaign");
            this.HasKey(ea => ea.Id);

            this.Property(ea => ea.Name).IsRequired();
            this.Property(ea => ea.Subject).IsRequired();
            this.Property(ea => ea.Body).IsRequired();
        }
    }
}