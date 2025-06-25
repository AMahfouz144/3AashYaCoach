namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class TrainerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
        public string? Certificates { get; set; }
        public string? Experience { get; set; }
        public string? ProfileImagePath { get; set; }
        public double Rate { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsBookMarked { get; set; }
    }
}
