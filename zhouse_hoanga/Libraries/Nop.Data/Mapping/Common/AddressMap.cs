﻿using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Common;

namespace Nop.Data.Mapping.Common
{
    public partial class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            this.ToTable("Address");
            this.HasKey(a => a.Id);

            this.HasOptional(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId).WillCascadeOnDelete(false);

            this.HasOptional(a => a.StateProvince)
                .WithMany()
                .HasForeignKey(a => a.StateProvinceId).WillCascadeOnDelete(false);

            this.HasOptional(a => a.District)
                .WithMany()
                .HasForeignKey(a => a.DistrictId).WillCascadeOnDelete(false);
            this.HasOptional(a => a.Ward)
                .WithMany()
                .HasForeignKey(a => a.WardId).WillCascadeOnDelete(false);
            this.HasOptional(a => a.Street)
                .WithMany()
                .HasForeignKey(a => a.StreetId).WillCascadeOnDelete(false);
        }
    }
}
