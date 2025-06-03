
namespace API.Controllers.ViewsControllers
{
    [Serializable]
    internal class BusinessException : Exception
    {
        private string v1;
        private string v2;

        public BusinessException()
        {
        }

        public BusinessException(string? message) : base(message)
        {
        }

        public BusinessException(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public BusinessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}