using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3AashYaCoach.Models
{
    public class Coach
    {
        [Key, ForeignKey("User")]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string Certificates { get; set; }

        [Required]
        public string ExperienceDetails { get; set; }
        public string? ProfileImagePath { get; set; }
        [MaxLength(6)]
        public double Rate { get; set; }

    }

}
