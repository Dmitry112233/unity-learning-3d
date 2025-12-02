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
            if (GUILayout.Button("Remove WayPoint"))
            {
                RemoveWayPoint();
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
        var selectedGo = Selection.activeGameObject;
        if (!selectedGo) return;

        var selectedWp = selectedGo.GetComponent<WayPoint>();
        if (!selectedWp) return;
        
        var selectedNode = _wayPointManager.NodeOf(selectedWp);
        if (selectedNode == null) return;
        
        GameObject newObj = new GameObject($"WayPoint_{parentTransform.childCount}", typeof(WayPoint));
        newObj.transform.SetParent(parentTransform, false);

        var newWp = newObj.GetComponent<WayPoint>();
        
        newWp.transform.position = selectedWp.transform.position;
        newWp.transform.forward  = selectedWp.transform.forward;
        
        _wayPointManager.AddAfter(selectedNode, newWp);
        
        newObj.transform.SetSiblingIndex(selectedWp.transform.GetSiblingIndex() + 1);

        Selection.activeGameObject = newObj;
        
        _wayPointManager.RebuildListFromChildren();
    }
    
    private void RemoveWayPoint()
    {
        var selectedGo = Selection.activeGameObject;
        if (!selectedGo) return;

        var selectedWp = selectedGo.GetComponent<WayPoint>();
        if (!selectedWp) return;
        
        var selectedNode = _wayPointManager.NodeOf(selectedWp);
        if (selectedNode == null) return;
        
        DestroyImmediate(selectedWp.gameObject);
        _wayPointManager.RebuildListFromChildren();
    }

}
