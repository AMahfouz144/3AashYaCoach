using _3AashYaCoach.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach._3ash_ya_coach.Models
{
    public class TrainerRating
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CoachId { get; set; }

        [ForeignKey("CoachId")]
        public Coach Coach { get; set; }

        [Required]
        public Guid RatedByUserId { get; set; }

        [ForeignKey("RatedByUserId")]
        public User RatedBy { get; set; }

        [Range(1, 5)]
        public double RatingValue { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
