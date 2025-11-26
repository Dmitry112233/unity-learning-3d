using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(
            fov.transform.position,
            Vector3.up,
            Vector3.forward,
            360,
            fov._viewRadius);
        
        Vector3 viewAngleA = fov.DirFromAngle(-fov._viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov._viewAngle / 2, false);
        
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov._viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov._viewRadius);
        
        fov._visibleTargets.ForEach(t => Handles.DrawLine(fov.transform.position, t.transform.position));
    }
}
