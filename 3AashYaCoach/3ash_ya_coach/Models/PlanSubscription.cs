using _3AashYaCoach.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach._3ash_ya_coach.Models
{
    public class PlanSubscription
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid WorkoutPlanId { get; set; }
        [ForeignKey("WorkoutPlanId")]
        public WorkoutPlan Plan { get; set; }

        [Required]
        public Guid TraineeId { get; set; }
        [ForeignKey("TraineeId")]
        public User Trainee { get; set; }

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    }

}
