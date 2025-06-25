using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3AashYaCoach.Models
{

    public class WorkoutPlan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string PlanName { get; set; }
        public string? PrimaryGoal { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid CoachId { get; set; }
        [ForeignKey("CoachId")]
        public User Coach { get; set; }

        [Required]
        //public Guid? TraineeId { get; set; }
        //[ForeignKey("TraineeId")]
        //public User? Trainee { get; set; }
        public bool IsPublic { get; set; } = false;
        public bool IsFollwoed { get; set; } = false;
        public ICollection<WorkoutDay> Days { get; set; }

    }
    
}
