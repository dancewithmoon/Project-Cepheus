using CodeBase.UI.Screens;
using CodeBase.UI.Services.Screens;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "Screens", menuName = "StaticData/Screens")]
    public class ScreenStaticData : ScriptableObject
    {
        public SerializableDictionary<ScreenId, BaseScreen> Screens;
    }
}