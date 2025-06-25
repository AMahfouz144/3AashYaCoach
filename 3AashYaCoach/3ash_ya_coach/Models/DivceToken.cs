namespace _3AashYaCoach._3ash_ya_coach.Models
{
    public class DeviceToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
