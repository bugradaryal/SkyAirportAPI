namespace Business.Abstract
{
    public interface IMailServices
    {
        Task ConfirmEmail(string userid, string token);
        Task SendingEmail(string email, string url);
    }
}
