using _3AashYaCoach._3ash_ya_coach.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3AashYaCoach.Models
{
    public class WorkoutDay
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid WorkoutPlanId { get; set; }
        [ForeignKey("WorkoutPlanId")]
        public WorkoutPlan Plan { get; set; }

        [Required]
        public int DayNumber { get; set; } // optional: 1 - 30
        public string DayName { get; set; }
        public string? Notes { get; set; }

        public ICollection<WorkoutExercise> Exercises { get; set; }
    }
}
