using Common.Enums;

namespace Services.UserService
{
    public interface IUserService
    {
        List<string> Get(string username);
        SubscriptionResultEnum Subscribe(string username, SubscriptionTypeEnum subscriptionType);
    }
}
