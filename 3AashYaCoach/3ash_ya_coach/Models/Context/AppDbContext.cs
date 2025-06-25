using _3AashYaCoach._3ash_ya_coach.Models;
using Microsoft.EntityFrameworkCore;
namespace _3AashYaCoach.Models.Context
{

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Coach> Trainers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SavedCoach> SavedCoaches { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutDay> WorkoutDays { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<PlanSubscription> PlanSubscriptions { get; set; }
        public DbSet<DeviceToken> DeviceTokens { get; set; }
        public DbSet<TrainerRating> TrainerRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WorkoutDay>()
      .HasIndex(d => new { d.WorkoutPlanId, d.DayNumber })
      .IsUnique();
            // إعداد العلاقة 1-1 بين User و Trainer
            modelBuilder.Entity<Coach>()
                .HasOne(t => t.User)
                .WithOne(u => u.TrainerProfile)
                .HasForeignKey<Coach>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // إعداد العلاقة 1-N بين المدرب والمشتركين
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Coach)
                .WithMany(u => u.SubscriptionsAsTrainer)
                .HasForeignKey(s => s.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            // إعداد العلاقة 1-N بين المتدرب والمدرب
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Trainee)
                .WithMany(u => u.SubscriptionsAsTrainee)
                .HasForeignKey(s => s.TraineeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SavedCoach>()
             .HasOne(s => s.Trainee)
             .WithMany(u => u.SavedCoaches)
             .HasForeignKey(s => s.TraineeId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedCoach>()
                .HasOne(s => s.Coach)
                .WithMany() // بدون عكس العلاقة من جانب المدرب
                .HasForeignKey(s => s.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkoutPlan>()
             .HasMany(p => p.Days)
             .WithOne(d => d.Plan)
             .HasForeignKey(d => d.WorkoutPlanId);

            modelBuilder.Entity<WorkoutDay>()
                .HasMany(d => d.Exercises)
                .WithOne(e => e.Day)
                .HasForeignKey(e => e.WorkoutDayId);
            modelBuilder.Entity<PlanSubscription>()
    .HasOne(p => p.Subscription)
    .WithMany(s => s.PlanSubscriptions)
    .HasForeignKey(p => p.SubscriptionId)
    .OnDelete(DeleteBehavior.Cascade);


        }
    }

}
