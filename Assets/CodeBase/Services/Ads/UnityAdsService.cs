using System;
using CodeBase.StaticData.Ads;
using CodeBase.StaticData.Service;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class UnityAdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly IStaticDataService _staticData;
        private UnityAdsIds _adsIds;

        private Action _onRewardedCompleted;

        public bool IsRewardedLoaded { get; private set; }
        public event Action RewardedVideoLoaded;

        public UnityAdsService(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public void Initialize()
        {
            UnityAdsStaticData unityAdsStaticData = _staticData.GetUnityAdsData();
            _adsIds = unityAdsStaticData.GetIds();
            Advertisement.Initialize(_adsIds.GameId, unityAdsStaticData.IsTestMode, this);
        }
        
        public void ShowRewarded(Action onRewardedCompleted)
        {
            if(IsRewardedLoaded == false)
                return;
            
            _onRewardedCompleted = onRewardedCompleted;
            Advertisement.Show(_adsIds.RewardedId, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads successfully initialized!");
            LoadRewarded();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
            Debug.LogError($"Unity Ads initialization failed!. Error: {error}, message: {message}");

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"{nameof(OnUnityAdsAdLoaded)}: {placementId}");

            if (placementId == _adsIds.RewardedId)
            {
                IsRewardedLoaded = true;
                RewardedVideoLoaded?.Invoke();
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.LogError($"{nameof(OnUnityAdsFailedToLoad)}: Error: {error}, message: {message}");

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.LogError($"{nameof(OnUnityAdsShowFailure)}: Error: {error}, message: {message}");

        public void OnUnityAdsShowStart(string placementId) =>
            Debug.Log($"{nameof(OnUnityAdsShowStart)}: {placementId}");

        public void OnUnityAdsShowClick(string placementId) =>
            Debug.Log($"{nameof(OnUnityAdsShowClick)}: {placementId}");

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.Log($"{nameof(OnUnityAdsShowComplete)}: {placementId}, result: {showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    Debug.Log($"{nameof(OnUnityAdsShowComplete)}: {placementId}, result: {showCompletionState}");
                    _onRewardedCompleted?.Invoke();
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.LogError($"{nameof(OnUnityAdsShowComplete)}: UNKNOWN RESULT!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(showCompletionState), showCompletionState, null);
            }
            LoadRewarded();
        }

        private void LoadRewarded() =>
            Advertisement.Load(_adsIds.RewardedId, this);
    }
}