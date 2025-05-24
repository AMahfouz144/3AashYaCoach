using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3AashYaCoach.Models
{
    public class Subscription
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

        [Required]
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
