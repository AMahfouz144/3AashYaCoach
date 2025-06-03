namespace _3AashYaCoach._3ash_ya_coach.Services.LoginService
{
    public interface ILoginService
    {
        Task<string> LoginAsync(string email, string password);
    }
}
