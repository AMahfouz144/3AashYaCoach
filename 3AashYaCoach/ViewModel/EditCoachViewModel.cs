using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach.ViewModel
{
    public class EditCoachViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Certificates { get; set; }

        [Required]
        public string ExperienceDetails { get; set; }
    }

}
