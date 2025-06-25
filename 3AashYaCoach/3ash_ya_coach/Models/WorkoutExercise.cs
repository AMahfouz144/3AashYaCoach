using _3AashYaCoach.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach._3ash_ya_coach.Models
{
    public class WorkoutExercise
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid WorkoutDayId { get; set; }
        [ForeignKey("WorkoutDayId")]
        public WorkoutDay Day { get; set; }

        [Required]
        public string Name { get; set; }

        public string muscleGroup { get; set; }
        public string difficulty { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string VideoLink { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
