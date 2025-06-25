namespace _3AashYaCoach._3ash_ya_coach.Shared
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string error, string message)
        {
            Error = error;
            Message = message;
        }
    }
}
