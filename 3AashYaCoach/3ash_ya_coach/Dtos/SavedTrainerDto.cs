namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class SavedTrainerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
        public string? Certificates { get; set; }
        public string? ExperienceDetails { get; set; }
        public string? ProfileImagePath { get; set; }
        public double Rate { get; set; }
    }

}
