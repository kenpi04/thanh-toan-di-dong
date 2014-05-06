using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
   public class ProviderMap:EntityTypeConfiguration<Provider>
    {
       public ProviderMap()
       {
           this.ToTable("Provider");
           this.HasKey(x => x.Id);
          this.HasRequired(p => p.Service)
               .WithMany(s => s.Providers)
               .HasForeignKey(p => p.ServiceId);
          
       }
    }
}
