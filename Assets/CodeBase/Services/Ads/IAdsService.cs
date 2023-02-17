using System;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Services.Ads
{
    public interface IAdsService : IService
    {
        bool IsRewardedLoaded { get; }
        event Action RewardedVideoLoaded;

        void Initialize();
        void ShowRewarded(Action onRewardedCompleted);
    }
}