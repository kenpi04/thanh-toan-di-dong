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
            
            //this.Entity<CardMobile>().ToTable("CardMobile");
            //this.Entity<CategoryCardMobile>().ToTable("CategoryCardMobile");
            //this.Entity<OrderNote>().ToTable("OrderNote");
            //this.Entity<Order>().ToTable("Order");
            //this.Entity<PromotionEvent>().ToTable("PromotionEvent");
            //this.Entity<Service>().ToTable("Service");
            //this.Entity<Provider>().ToTable("Provider");

            //this.Entity<CardMobile>().HasRequired(c => c.CategoryCardMobile)
            //    .WithMany(cc => cc.CardMobile)
            //    .HasForeignKey(c => c.CategoryCardMobileId);

            //this.Entity<OrderNote>().HasRequired(o => o.Order)
            //    .WithMany(od=>od.OrderNotes)
            //    .HasForeignKey(o => o.OrderId);

            //this.Entity<Provider>().HasRequired(p => p.Service)
            //    .WithMany(s => s.Providers)
            //    .HasForeignKey(p => p.ServiceId);
           
        }
    }
}
