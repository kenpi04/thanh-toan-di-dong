using System;
using System.Data.Entity.ModelConfiguration;
using Domain.Entity;
namespace Domain.Maps
{
   public class UserMap:EntityTypeConfiguration<User>
    {
       public UserMap()
       {
           this.ToTable("User");
           this.HasKey(x => x.Id);
       }
    }
}
