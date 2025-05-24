using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3AashYaCoach.Models
{
  
    public class SavedCoach
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TraineeId { get; set; }

        [ForeignKey("TraineeId")]
        public User Trainee { get; set; }

        [Required]
        public Guid CoachId { get; set; }

        [ForeignKey("CoachId")]
        public User Coach { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }

}
