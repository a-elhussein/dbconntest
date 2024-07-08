using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TestIdentity.Models;

namespace TestIdentity.Data
{
    public class TestIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public TestIdentityDbContext(DbContextOptions<TestIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships if needed
            builder.Entity<UserCompany>()
                .HasKey(uc => new { uc.UserId, uc.CompanyId });

            builder.Entity<UserCompany>()
                .HasOne(uc => uc.ApplicationUser)
                .WithMany(u => u.UserCompanies)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserCompany>()
                .HasOne(uc => uc.Company)
                .WithMany(c => c.UserCompanies)
                .HasForeignKey(uc => uc.CompanyId);


            var managerId = "b4c0037d - 740b - 460d - ae4e - ada97e8d4219";
            var userId = "8ed708a7-1c52-4d56-ab0c-57bad508b0b8";
            var deliveryId = "a8497c56-d656-4164-ba8a-fb69e99a35db";
            var financeId = "e2a289d0-3645-4cbb-8675-8a21847c83cc";



            var roles = new List<IdentityRole> 
            {
                new IdentityRole
                {
                    Id = managerId,
                    ConcurrencyStamp = managerId,
                    Name = "Manager",
                    NormalizedName = "Manager".ToUpper()
                },
                new IdentityRole
                {
                    Id = userId,
                    ConcurrencyStamp = userId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                },
                new IdentityRole
                {
                    Id = deliveryId,
                    ConcurrencyStamp = deliveryId,
                    Name = "Delivery",
                    NormalizedName = "Delivery".ToUpper()
                },
                new IdentityRole
                {
                    Id = financeId,
                    ConcurrencyStamp = financeId,
                    Name = "Finance",
                    NormalizedName = "Finance".ToUpper()
                },

            };

            builder.Entity<IdentityRole>().HasData(roles);

            var companies = new List<Company>
            {
                new Company
                {
                    Id = 1,
                    CompanyName = "Company1",
                },
                 new Company
                {
                    Id = 2,
                    CompanyName = "Company2",
                },
            };
            builder.Entity<Company>().HasData(companies);


        }
    }
}
