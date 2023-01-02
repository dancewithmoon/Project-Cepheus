using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Infrastructure.States;
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
    }
}