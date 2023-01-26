using System;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Services.Ads
{
    public interface IAdsService : IService
    {
        void LoadRewarded();
        void ShowRewarded(Action onRewardedCompleted);
    }
}