namespace _3AashYaCoach
{
    public class ApplicationConfiguration
    {
        public OtpConfiguration OtpConfiguration { set; get; }
    }


    public class JwtConfiguration
    {
        public string Secret { set; get; }
    }

    public class OtpConfiguration
    {
        public bool Fixed { set; get; }
        public string FixedCode { set; get; }
    }
}
