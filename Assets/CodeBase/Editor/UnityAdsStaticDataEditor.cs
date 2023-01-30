using System.Reflection;
using CodeBase.StaticData.Ads;
using UnityEditor;
using UnityEditor.Advertisements;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(UnityAdsStaticData))]
    public class UnityAdsStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelData = target as UnityAdsStaticData;

            if(levelData == null)
                return;
            
            GUILayout.Space(10);
            if (GUILayout.Button("Update Game Ids"))
            {
                FieldInfo androidIds = typeof(UnityAdsStaticData).GetField("_androidIds", BindingFlags.NonPublic | BindingFlags.Instance);
                androidIds.SetValue(levelData, new UnityAdsIds()
                {
                    GameId = AdvertisementSettings.GetGameId(RuntimePlatform.Android)
                });
                
                FieldInfo iosIds = typeof(UnityAdsStaticData).GetField("_iosIds", BindingFlags.NonPublic | BindingFlags.Instance);
                iosIds.SetValue(levelData, new UnityAdsIds()
                {
                    GameId = AdvertisementSettings.GetGameId(RuntimePlatform.IPhonePlayer)
                });
            }   
            
            EditorUtility.SetDirty(target);
        }
    }
}