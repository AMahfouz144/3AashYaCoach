using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach.ViewModel
{
    public class CreateCoachViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public string Certificates { get; set; }

        [Required]
        public string ExperienceDetails { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }

}
