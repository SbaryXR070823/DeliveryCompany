using DeliveryCompany.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.DataAccess.Data
{
    public class DataAppDbContext : DbContext
    {
        public DataAppDbContext(DbContextOptions<DataAppDbContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<DeliveryCars> DeliveryCars { get; set; }
        public DbSet<DeliveryCarOrder> DeliveryCarOrders { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Packages> Packages { get; set; }

        public DbSet<PageDescriptions> PageDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageDescriptions>().HasData(
                new PageDescriptions
                {
                    PageDescriptionsId = 1,
                    Description = "initial description"
                }
            );

            modelBuilder.Entity<DeliveryCarOrder>().HasKey(
                dco => new { dco.DeliveryCarId, dco.OrderId });

            modelBuilder.Entity<Order>()
               .HasOne(o => o.Packages)
               .WithOne(p => p.Order)
               .HasForeignKey<Order>(o => o.PackagesId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeliveryCarOrder>()
               .HasKey(dc => new { dc.DeliveryCarId, dc.OrderId });

            modelBuilder.Entity<DeliveryCarOrder>()
                .HasOne(dc => dc.DeliveryCars)
                .WithMany()
                .HasForeignKey(dc => dc.DeliveryCarId);

            modelBuilder.Entity<DeliveryCarOrder>()
                .HasOne(dc => dc.Order)
                .WithMany()
                .HasForeignKey(dc => dc.OrderId);


            modelBuilder.Entity<DeliveryCars>()
               .HasKey(dc => dc.DeliveryCarsId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DeliveryCars>()
                .HasOne(dc => dc.Employee)
                .WithOne(e => e.DeliveryCars)
                .HasForeignKey<DeliveryCars>(dc => dc.EmployeeId)
                .IsRequired(false);

            modelBuilder.Entity<DeliveryCars>()
                .HasOne(dc => dc.City)
                .WithMany(c => c.DeliveryCarsInCity)
                .HasForeignKey(dc => dc.CityId);

        }
    }
}
