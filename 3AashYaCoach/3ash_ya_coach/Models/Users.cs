using _3AashYaCoach.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required , MaxLength(8)]
        public UserRole Role { get; set; } // "Admin", "Trainer", "Trainee"

        // العلاقات
        public Coach? TrainerProfile { get; set; }

        public ICollection<Subscription>? SubscriptionsAsTrainee { get; set; } // اشتراكاته مع المدربين
        public ICollection<Subscription>? SubscriptionsAsTrainer { get; set; } // المشتركين المرتبطين به
        public ICollection<SavedCoach> ?SavedCoaches { get; set; }

    }

}
