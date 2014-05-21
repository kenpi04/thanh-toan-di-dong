using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Directory;

namespace Nop.Data.Mapping.Directory
{
    /// <summary>
    /// Represents a FrtDistrict Map
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 20/09/2012      XuanDT@fpt.com.vn               Created.
    /// 
    /// </summary>
    public partial class DistrictMap : EntityTypeConfiguration<District>
    {
        public DistrictMap()
        {
            this.ToTable("District");
            this.HasKey(fd => fd.Id);
            this.Property(fd => fd.Name).IsRequired().HasMaxLength(100);
            this.HasRequired(x => x.StateProvince)
                .WithMany(x => x.Districts)
                .HasForeignKey(x => x.StateProvinceId);
           
        }

    }
}