using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            UniqueId uniqueId = (UniqueId)target;
            if (string.IsNullOrEmpty(uniqueId.Id))
            {
                Generate(uniqueId);
                return;
            }

            if (IsAnyObjectWithSameId(uniqueId))
                Generate(uniqueId);
        }

        private static void Generate(UniqueId uniqueId)
        {
            if (Application.isPlaying)
                return;

            Scene currentScene = uniqueId.gameObject.scene;

            uniqueId.Generate();

            EditorUtility.SetDirty(uniqueId);
            EditorSceneManager.MarkSceneDirty(currentScene);
        }

        private static bool IsAnyObjectWithSameId(UniqueId uniqueId)
        {
            UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();
            return uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id);
        }
    }
}