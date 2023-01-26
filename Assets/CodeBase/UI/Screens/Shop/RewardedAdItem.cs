using System.Collections.Generic;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Screens.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        [SerializeField] private Button _showRewardedAdButton;
        [SerializeField] private List<GameObject> _adActiveObjects;
        [SerializeField] private List<GameObject> _adInactiveObjects;
        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        [Inject]
        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }
        
        public void Initialize()
        {
            _showRewardedAdButton.onClick.AddListener(OnShowRewardedAdClick);
            RefreshAvailableAd();
        }

        public void Subscribe()
        {
            _adsService.RewardedVideoLoaded += RefreshAvailableAd;
        }

        public void Cleanup()
        {
            _adsService.RewardedVideoLoaded -= RefreshAvailableAd;
        }

        private void OnShowRewardedAdClick() => 
            _adsService.ShowRewarded(OnVideoFinished);

        private void OnVideoFinished()
        {
            _progressService.Progress.LootData.Add(5);   
        }

        private void RefreshAvailableAd()
        {
            _adActiveObjects.ForEach(adActiveObject => adActiveObject.SetActive(_adsService.IsRewardedLoaded));
            _adInactiveObjects.ForEach(adActiveObject => adActiveObject.SetActive(!_adsService.IsRewardedLoaded));
        }
    }
}