using UnityEditor;
using UnityEngine;

public class WayPointEditorComponent : EditorWindow
{
    public Transform parentTransform;
    
    [MenuItem("Tools/WayPoint Editor")]
    public static void OnOpen()
    {
        GetWindow<WayPointEditorComponent>("WayPoint Editor Component");
    }

    private void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("parentTransform"));
        
        if (!parentTransform)
        {
            EditorGUILayout.HelpBox("The parent transform must be set", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButton();
            EditorGUILayout.EndVertical();
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawButton()
    {
        if (GUILayout.Button("Create WayPoint"))
        {
            CreateWayPoint();
        }

        if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<WayPoint>())
        {
            if (GUILayout.Button("Create Next WayPoint"))
            {
                CreateNextWayPoint();
            }
        }
    }

    private void CreateNextWayPoint()
    {
        GameObject wayPointObj = new GameObject($"WayPoint_{parentTransform.childCount}", typeof(WayPoint));
        wayPointObj.transform.SetParent(parentTransform, false);
        
        WayPoint wayPoint = wayPointObj.GetComponent<WayPoint>();
        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();
        
        wayPoint.transform.position = selectedWayPoint.transform.position;
        wayPoint.transform.forward = selectedWayPoint.transform.forward;
        
        wayPoint.prevWayPoint = selectedWayPoint;

        if (selectedWayPoint.nextWayPoint)
        {
            selectedWayPoint.nextWayPoint.prevWayPoint = wayPoint;
            wayPoint.nextWayPoint = selectedWayPoint.nextWayPoint;
        }
        selectedWayPoint.nextWayPoint = wayPoint;
        wayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());
        
        Selection.activeGameObject = wayPoint.gameObject;
    }

    private void CreateWayPoint()
    {
        GameObject wayPointObj = new GameObject($"WayPoint_{parentTransform.childCount}", typeof(WayPoint));
        wayPointObj.transform.SetParent(parentTransform, false);
        
        WayPoint wayPoint = wayPointObj.GetComponent<WayPoint>();
        
        if (parentTransform.childCount > 1)
        {
            wayPoint.prevWayPoint = parentTransform.GetChild(parentTransform.childCount - 2).GetComponent<WayPoint>();
            wayPoint.prevWayPoint.nextWayPoint = wayPoint;
            
            wayPoint.transform.position = wayPoint.prevWayPoint.transform.position;
            wayPoint.transform.forward = wayPoint.prevWayPoint.transform.forward;
        }
        
        Selection.activeGameObject = wayPointObj.gameObject;
    }
}
