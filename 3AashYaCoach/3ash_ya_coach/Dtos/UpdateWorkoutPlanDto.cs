namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class UpdateWorkoutPlanDto
    {
        public Guid PlanId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; } = true;
    }
}
