using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
 public class Topic:BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; } 
        public string Content { get; set; }
        public bool AddToMenu { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsHomePage { get; set; }
        public int LanguageId { get; set; }
    }
}
