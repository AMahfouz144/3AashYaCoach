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

        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Notes { get; set; }
    }
}
