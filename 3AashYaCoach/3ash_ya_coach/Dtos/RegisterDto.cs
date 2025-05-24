using _3AashYaCoach.Models.Enums;

namespace _3AashYaCoach.Dtos
{
    public class RegisterDto
    {
        //public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
        public string Password { get; set; }=   string.Empty;
        public UserRole Role { get; set; } // "Trainee" فقط في هذه المرحلة
    }
}
