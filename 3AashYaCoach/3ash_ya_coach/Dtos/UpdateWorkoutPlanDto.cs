namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class UpdateWorkoutPlanDto
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; }
        public string PrimaryGoal { get; set; }
        public bool IsPublic { get; set; } = true;
        public List<WorkoutDayDto> Days { get; set; }
    }
}
