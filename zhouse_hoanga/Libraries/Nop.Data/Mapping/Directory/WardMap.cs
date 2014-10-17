using Nop.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Directory
{
    public partial class WardMap : EntityTypeConfiguration<Ward>
    {
        public WardMap()
        {
            this.ToTable("Ward");
            this.HasKey(fw => fw.Id);
            this.Property(fw => fw.Name).IsRequired().HasMaxLength(100);
            this.HasRequired(x => x.District)
                .WithMany(d => d.Wards)
                .HasForeignKey(x => x.DistrictId);
        }
    }
}
