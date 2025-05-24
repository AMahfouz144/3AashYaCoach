namespace _3AashYaCoach.Dtos
{
    public class SaveCoachesDto
    {
        public Guid TraineeId { get; set; }
        public List<Guid> CoachIds { get; set; }
    }
}
