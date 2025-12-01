using UnityEditor;
using UnityEngine;

public class WayPointEditorComponent : EditorWindow
{
    public Transform parentTransform;
    
    private WayPointManager _wayPointManager;
    
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
        else if(!parentTransform.GetComponent<WayPointManager>())
        {
            EditorGUILayout.HelpBox("The way point manager must be added as component to parent transform", MessageType.Warning);
        }
        else
        {
            _wayPointManager =  parentTransform.GetComponent<WayPointManager>();
            
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Last WayPoint"))
        {
            CreateLastWayPoint();
        }

        if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<WayPoint>())
        {
            if (GUILayout.Button("Create Next WayPoint"))
            {
                CreateNextWayPoint();
            }
        }
    }
    
    private void CreateLastWayPoint()
    {
        GameObject wayPointObj = new GameObject($"WayPoint_{parentTransform.childCount}", typeof(WayPoint));
        wayPointObj.transform.SetParent(parentTransform, false);

        var wayPoint = wayPointObj.GetComponent<WayPoint>();

        if (parentTransform.childCount > 1)
        {
            var last = parentTransform.GetChild(parentTransform.childCount - 2).GetComponent<WayPoint>();
            
            wayPoint.transform.position = last.transform.position;
            wayPoint.transform.forward = last.transform.forward;
        }

        _wayPointManager.AddLast(wayPoint);
        Selection.activeGameObject = wayPointObj;
    }
    
    private void CreateNextWayPoint()
    {
        var selectedGO = Selection.activeGameObject;
        if (!selectedGO) return;

        var selectedWP = selectedGO.GetComponent<WayPoint>();
        if (!selectedWP) return;
        
        var selectedNode = _wayPointManager.NodeOf(selectedWP);
        if (selectedNode == null) return;
        
        GameObject newObj = new GameObject($"WayPoint_{parentTransform.childCount}", typeof(WayPoint));
        newObj.transform.SetParent(parentTransform, false);

        var newWP = newObj.GetComponent<WayPoint>();
        
        newWP.transform.position = selectedWP.transform.position;
        newWP.transform.forward  = selectedWP.transform.forward;
        
        _wayPointManager.AddAfter(selectedNode, newWP);
        
        newObj.transform.SetSiblingIndex(selectedWP.transform.GetSiblingIndex() + 1);

        Selection.activeGameObject = newObj;
    }

}
