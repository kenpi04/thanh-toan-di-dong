using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Domain.Entity;

namespace Domain.DAO
{
    public class ModelBuilder
    {
        public ModelBuilder(DbModelBuilder builder)
        {
            builder.Entity<CardMobile>().HasRequired(c => c.CategoryCardMobile)
                .WithMany(cc => cc.CardMobile)
                .HasForeignKey(c => c.CategoryCardMobileId);

            builder.Entity<OrderNote>().HasRequired(o => o.Order)
                .WithMany(od=>od.OrderNotes)
                .HasForeignKey(o => o.OrderId);

            builder.Entity<Provider>().HasRequired(p => p.Service)
                .WithMany(s => s.Providers)
                .HasForeignKey(p => p.ServiceId);
        }
    }
}
