namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            // key - value
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {

            //send mail - output to window
            Console.WriteLine($"Email from {_mailFrom} to {_mailTo}," + $"with {nameof(LocalMailService)}");
            Console.WriteLine($"subject:{subject}");
            Console.WriteLine($"message:{message}");

        }

    }
}
