using Common.Enums;
using Data.Entities;
using Data.Repositories.AuthRepository;

namespace Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<string> Get(string username)
        {
            User user = _userRepository.Get(username)!;
            List<string> SubscriptionsList = new List<string>();

            if (user.Subscription == null)
            {
                SubscriptionsList.Add("Free");
                if (!user.usedTrial)
                {
                    SubscriptionsList.Add("Trial");
                }
                SubscriptionsList.Add("Pro");
            } else if (user.SubscribedUntil < DateTime.UtcNow)
            {
                user.ConversionsCount = 0;
                user.SubscriptionId = null;
                _userRepository.Update(user);
                SubscriptionsList.Add("Free");
                if (!user.usedTrial)
                {
                    SubscriptionsList.Add("Trial");
                }
                SubscriptionsList.Add("Pro");
            } else if (user.Subscription.Name == "Free")
            {
                if (!user.usedTrial)
                {
                    SubscriptionsList.Add("Trial");
                }
                SubscriptionsList.Add("Pro");
            } else if (user.Subscription!.Name == "Trial")
            {
                SubscriptionsList.Add("Pro");
            }

            return SubscriptionsList;
        }

        public SubscriptionResultEnum Subscribe(string username, SubscriptionTypeEnum subscriptionType)
        {
            User user = _userRepository.Get(username)!;

            if (user.SubscriptionId >= subscriptionType)
            {
                return SubscriptionResultEnum.AlreadySubscribed;
            }

            if (subscriptionType == SubscriptionTypeEnum.Trial && user.usedTrial == true)
            {
                return SubscriptionResultEnum.AlreadyUsedTrial;
            }

            if (subscriptionType == SubscriptionTypeEnum.Trial)
            {
                user.usedTrial = true;
            }

            user.SubscribedUntil = DateTime.UtcNow.AddMonths(1);
            user.ConversionsCount = 0;
            user.SubscriptionId = subscriptionType;
            _userRepository.Update(user);

            return SubscriptionResultEnum.Success;
        }
    }
}
