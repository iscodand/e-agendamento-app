using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Domain.Entities.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
                                                          ApplicationRole,
                                                          string,
                                                          IdentityUserClaim<string>,
                                                          UsersRoles,
                                                          IdentityUserLogin<string>,
                                                          IdentityRoleClaim<string>,
                                                          IdentityUserToken<string>>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IDateTimeService dateTime,
                                    IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UsersRoles> UsersRoles { get; set; }
        public DbSet<UsersCompanies> UsersCompanies { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UsersCompanies>()
                .HasKey(uc => new { uc.UserId, uc.CompanyId });

            builder.Entity<UsersCompanies>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UsersCompanies)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UsersCompanies>()
                .HasOne(uc => uc.Company)
                .WithMany(c => c.UsersCompanies)
                .HasForeignKey(uc => uc.CompanyId);

            // Isco: all decimals will have 18,6 range.
            // foreach (var property in builder.Model.GetEntityTypes()
            //     .SelectMany(t => t.GetProperties())
            //     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            // {
            //     property.SetColumnType("decimal(18,6)");
            // }


            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<UsersRoles>(entity =>
            {
                entity.ToTable(name: "Users_Roles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "Users_Claims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "Users_Logins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "Roles_Claims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "Users_Tokens");
            });

            builder.Entity<UsersCompanies>(entity =>
            {
                entity.ToTable(name: "Users_Companies");
            });
        }
    }
}
