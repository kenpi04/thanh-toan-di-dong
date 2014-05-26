using Nop.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Directory
{
    public partial class StreetMap : EntityTypeConfiguration<Street>
    {
        public StreetMap()
        {
            this.ToTable("Street");
            this.HasKey(fs => fs.Id);
            this.Property(fs => fs.Name).IsRequired().HasMaxLength(100);
            this.HasRequired(x => x.District)
                .WithMany(x => x.Streets)
                    .HasForeignKey(x => x.DistrictId);
        }
    }
}
