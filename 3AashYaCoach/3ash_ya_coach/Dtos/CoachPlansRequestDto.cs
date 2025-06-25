namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class CoachPlansRequestDto
    {
        public List<Guid> CoachIds { get; set; } = [];
        public Guid TraineeId { get; set; }
    }
}
