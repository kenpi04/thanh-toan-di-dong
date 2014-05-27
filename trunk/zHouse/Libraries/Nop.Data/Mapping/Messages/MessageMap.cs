using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Messages;

namespace Nop.Data.Mapping.Messages
{
    public partial class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            this.ToTable("Fland_Message");
            this.HasKey(mt => mt.Id);

        }
       
    }
}