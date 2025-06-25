namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class TraineePlanProgressDto
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string PrimaryGoal { get; set; } = string.Empty;
        public Guid CoachId { get; set; }
        public string CoachName { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public int CompletedDays { get; set; }
    }

    public class TraineeDetailDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<TraineePlanProgressDto> SubscribedPlans { get; set; } = new();
    }
} 