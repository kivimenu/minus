﻿using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DAL.Context
{
    public class MinusContext : IdentityDbContext<IdentityUser>
    {
        //public MinusContext(DbContextOptions<MinusContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"server=localhost;database=minus;user=root;password=admin");
        }

        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(85));
            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.EmailConfirmed).HasColumnType("BIT(1)"));
            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.PhoneNumberConfirmed).HasColumnType("BIT(1)"));
            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.TwoFactorEnabled).HasColumnType("BIT(1)"));
            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.LockoutEnabled).HasColumnType("BIT(1)"));
            modelBuilder.Entity<IdentityUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(85));

            modelBuilder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(85));

            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));

            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(85));

            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            //modelBuilder.Entity<Customer>().ToTable("Customers");
            //modelBuilder.Entity<Partner>().ToTable("Partners");

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssociateAddress).IsRequired();
                entity.Property(e => e.AssociateName).IsRequired();
                entity.Property(e => e.AssociateUrl).IsRequired();
            });

            modelBuilder.Entity<PartnerUser>(entity =>
            {
                entity.HasOne(e => e.Partner).WithMany(src => src.Users).HasForeignKey(a => a.PartnerId).IsRequired();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.PublishDate).IsRequired();
                entity.Property(e => e.IsVisible);
                entity.HasOne(e => e.Customer).WithMany(src => src.Comments).HasForeignKey(a => a.CustomerId).IsRequired();
                entity.HasOne(e => e.Product).WithMany(src => src.Comments).HasForeignKey(a => a.ProductId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Partner).WithMany(src => src.Orders).HasForeignKey(a => a.PartnerId).IsRequired();
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.TotalPrice).IsRequired();
                entity.Property(e => e.OrderStatus).IsRequired();
                entity.HasOne(e => e.PaymentType).WithMany(src => src.Orders).HasForeignKey(a => a.PaymentTypeId).IsRequired();
                entity.HasOne(e => e.OrderType).WithMany(src => src.Orders).HasForeignKey(a => a.OrderTypeId).IsRequired();
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Product).WithMany(src => src.OrderProducts).HasForeignKey(a => a.ProductId).IsRequired();
                entity.HasOne(e => e.Order).WithMany(src => src.OrderProducts).HasForeignKey(a => a.OrderId).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IsInStock).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.AwayOrderAvailable).IsRequired();
                entity.Property(e => e.ProductVolumeUnit);
                entity.Property(e => e.TotalProductVolume);
                entity.Property(e => e.UnitPrice).IsRequired();
                entity.Property(e => e.Rating);
                entity.HasOne(e => e.Category).WithMany(e => e.Products).HasForeignKey(a => a.CategoryId).IsRequired();
            });

            modelBuilder.Entity<Content>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PhysicalPath).IsRequired();
                entity.HasOne(e => e.Product).WithMany(src => src.Contents).HasForeignKey(a => a.ProductId);
                entity.HasOne(e => e.Partner).WithMany(src => src.Contents).HasForeignKey(a => a.PartnerId);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Partner).WithMany(src => src.ProductCategories).HasForeignKey(a => a.PartnerId).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.HasOne(e => e.Content).WithOne(t => t.ProductCategory).HasForeignKey<ProductCategory>(a => a.ContentId);
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Name = "AssitantManager",
                    NormalizedName = "ASSISTANTMANAGER"
                },
                new IdentityRole
                {
                    Name = "KitchenStaff",
                    NormalizedName = "KITCHENSTAFF"
                },
                new IdentityRole
                {
                    Name = "Waitstaff",
                    NormalizedName = "WAITSTAFF"
                },
                new IdentityRole
                {
                    Name = "Cashier",
                    NormalizedName = "CASHIER"
                },
                new IdentityRole
                {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
                );

            var cashPayment = new PaymentType();
            var mobilePayment = new PaymentType();
            var cardPayment = new PaymentType();

            cashPayment = PaymentTypeEnum.CASH;
            mobilePayment = PaymentTypeEnum.MOBILE;
            cardPayment = PaymentTypeEnum.CARD;

            modelBuilder.Entity<PaymentType>().HasData(
                cashPayment, mobilePayment, cardPayment);

            var hereOrder = new OrderType();
            var awayOrder = new OrderType();

            hereOrder = OrderTypeEnum.HERE;
            awayOrder = OrderTypeEnum.AWAY;

            modelBuilder.Entity<OrderType>().HasData(hereOrder, awayOrder);
        }
    }
}
