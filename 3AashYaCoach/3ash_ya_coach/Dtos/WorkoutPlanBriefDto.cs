namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class WorkoutPlanBriefDto
    {
        public Guid Id { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string PrimaryGoal { get; set; } = string.Empty;
        public Guid CoachId { get; set; }
        public string CoachName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsPublic { get; set; }
        public bool IsFollowd { get; set; }
    }

}
