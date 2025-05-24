namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class CreatePlanHeaderDto
    {
        public Guid CoachId { get; set; }
        public Guid TraineeId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
