﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.DAO
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Reflection;
    using Domain.Entity;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        //public DbSet<CardMobile> CardMobile { get; set; }
        //public DbSet<CategoryCardMobile> CategoryCardMobile { get; set; }
        //public DbSet<Order> Order { get; set; }
        //public DbSet<OrderNote> OrderNote { get; set; }
        //public DbSet<PromotionEvent> PromotionEvent { get; set; }
        //public DbSet<Provider> Provider { get; set; }
        //public DbSet<Service> Service { get; set; }
        public Entities()
            : base("ThanhToanDiDongContext")
        {

        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
            
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }
    }
}