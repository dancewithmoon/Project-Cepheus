using System;
using System.Reflection;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(HeroDefaultStaticData))]
    public class HeroStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var heroData = target as HeroDefaultStaticData;

            if(heroData == null)
                return;
            
            if (GUILayout.Button("Collect"))
            {
                FieldInfo initialPointsField = typeof(HeroDefaultStaticData).GetField("_initialPoints", BindingFlags.NonPublic | BindingFlags.Instance);
                var initialPoints = initialPointsField.GetValue(heroData) as SerializableDictionary<string, Vector3>;

                string activeScene = SceneManager.GetActiveScene().name;
                
                if(initialPoints.ContainsKey(activeScene))
                    return;
                
                initialPoints.Add(activeScene, GetInitialPointForActiveScene());
            }
            
            EditorUtility.SetDirty(target);
        }

        private Vector3 GetInitialPointForActiveScene()
        {
            GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
            if (initialPoint == null)
                throw new NullReferenceException("Initial Point wasn't found on active scene!");

            return initialPoint.transform.position;
        }
    }
}