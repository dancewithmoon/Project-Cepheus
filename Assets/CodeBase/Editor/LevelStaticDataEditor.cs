using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Logic;
using CodeBase.Logic.Spawner;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelData = target as LevelStaticData;

            if(levelData == null)
                return;
            
            if (GUILayout.Button("Collect"))
            {
                FieldInfo levelKey = typeof(LevelStaticData).GetField("_levelKey", BindingFlags.NonPublic | BindingFlags.Instance);
                levelKey.SetValue(levelData, SceneManager.GetActiveScene().name);
                
                FieldInfo enemySpawners = typeof(LevelStaticData).GetField("_enemySpawners", BindingFlags.NonPublic | BindingFlags.Instance);
                enemySpawners.SetValue(levelData, CollectSpawners());
                
                FieldInfo savePoints = typeof(LevelStaticData).GetField("_savePoints", BindingFlags.NonPublic | BindingFlags.Instance);
                savePoints.SetValue(levelData, CollectSavePoints());
            }
            
            EditorUtility.SetDirty(target);
        }

        private List<EnemySpawnerData> CollectSpawners()
        {
            return FindObjectsOfType<SpawnMarker>()
                .Select(x =>
                    new EnemySpawnerData(
                        x.GetComponent<UniqueId>().Id,
                        x.EnemyTypeId,
                        x.transform.position))
                .ToList();
        }
        
        private List<SavePointData> CollectSavePoints()
        {
            return FindObjectsOfType<SavePointMarker>()
                .Select(x =>
                    new SavePointData(
                        x.GetComponent<UniqueId>().Id,
                        x.transform.position,
                        x.transform.localScale))
                .ToList();
        }
    }
}