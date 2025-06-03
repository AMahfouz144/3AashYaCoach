namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public List<Guid> SubscriptionIds { get; set; } = new();
        public List<Guid> CoachIds { get; set; } = new();
        public List<Guid> SavedCoachIds { get; set; } = new();
        public List<Guid> PlanIds { get; set; } = new();
    }

}
