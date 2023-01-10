using CodeBase.Logic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SavePointMarker))]
    public class SavePointMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SavePointMarker marker, GizmoType gizmo)
        {
            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(marker.transform.position, marker.transform.localScale);
        }
    }
}