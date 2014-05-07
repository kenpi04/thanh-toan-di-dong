using System.Data.Entity.ModelConfiguration;
using Domain.Entity;


namespace Domain.Maps
{
   public class ServiceMap:EntityTypeConfiguration<Service>
    {
       public ServiceMap()
       {
           this.ToTable("Service");
           this.HasKey(x => x.Id);
          
       }
    }
}
