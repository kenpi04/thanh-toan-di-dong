using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Domain.Entity;

namespace Domain.DAO
{
    public class ModelBuilder:DbModelBuilder
    {
        public ModelBuilder()
        {
            //Set table
            
            this.Entity<User>().ToTable("Users");
            this.Entity<User>().HasKey(x => x.Id);
            this.Entity<Topic>().ToTable("Topics");
            this.Entity<Topic>().HasKey(x => x.Id);
            
            
   
           
        }
    }
}
