using Bdt.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bdt.Api.Infrastructure.Context;

public class BdtDbContext : IdentityDbContext<UserEntity>
{
    public BdtDbContext(DbContextOptions<BdtDbContext> options) : base(options) { }
    public DbSet<WorkoutEntity> Workout { get; set; }
    public DbSet<PlannerEntity> Planner { get; set; }
    public DbSet<WeekdayEntity> Weekdays { get; set; }
    public DbSet<WorkoutTypeEntity> WorkoutType { get; set; }
    public DbSet<LevelEntity> Level { get; set; }
    public DbSet<BdtProductEntity> BdtProduct { get; set; }
    public DbSet<WorkoutValuesEntity> WorkoutValues { get; set; }
    public DbSet<StripeCustomersEntity> StripeCustomer { get; set; }
    public DbSet<SubscriptionsEntity> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<WorkoutValuesEntity>()
            .HasOne(wv => wv.WorkoutType)
            .WithMany()
            .HasForeignKey(wv => wv.WorkoutTypeId)
            .IsRequired();

        // Enable eager loading for WorkoutType by default
        builder.Entity<WorkoutValuesEntity>().Navigation(wv => wv.WorkoutType).AutoInclude();

        base.OnModelCreating(builder);
    }
}
