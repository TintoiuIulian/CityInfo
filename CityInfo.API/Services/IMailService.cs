namespace CityInfo.API.Services
{
    //contract
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}