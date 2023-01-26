using System;
using UnityEngine;

namespace CodeBase.StaticData.Ads
{
    [CreateAssetMenu(fileName = "UnityAdsData", menuName = "StaticData/Ads/UnityAds")]
    public class UnityAdsStaticData : ScriptableObject
    {
        [SerializeField] private UnityAdsIds _androidIds;
        [SerializeField] private UnityAdsIds _iosIds;
        [SerializeField] private bool _isTestMode;
        
        public bool IsTestMode => _isTestMode;

        public UnityAdsIds GetIds()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return _androidIds;
                case RuntimePlatform.IPhonePlayer:
                    return _iosIds;
                case RuntimePlatform.WindowsEditor:
                    return _androidIds;
                default:
                    throw new NotSupportedException("Unsupported platform!");
            }
        }
    }

    [Serializable]
    public struct UnityAdsIds
    {
        public string GameId;
        public string RewardedId;
    }
}