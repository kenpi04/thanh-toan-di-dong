using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
   public class User:BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
