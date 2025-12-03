using UnityEditor;
using UnityEngine;

public class WayPointEditorComponent : EditorWindow
{
    public Transform parentTransform;
    private WayPointManager _wayPointManager;

    private int _routeNumber = 1;
    
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
            if (GUILayout.Button("Create Branch"))
            {
                CreateBranch();
            }
        }
    }

    private void CreateLastWayPoint()
    {
        GameObject newObj = new GameObject($"WayPoint_{parentTransform.childCount}", typeof(WayPoint));
        newObj.transform.SetParent(parentTransform, false);

        var newWp = newObj.GetComponent<WayPoint>();

        if (parentTransform.childCount > 1)
        {
            var last = parentTransform.GetChild(parentTransform.childCount - 2).GetComponent<WayPoint>();
            
            newWp.transform.position = last.transform.position;
            newWp.transform.forward = last.transform.forward;
        }

        newWp.ownerRoute = _wayPointManager;
        
        _wayPointManager.AddLast(newWp);
        Selection.activeGameObject = newObj;
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
        
        newWp.ownerRoute = _wayPointManager;
        
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
    
    private void CreateBranch()
    {
        var selectedGo = Selection.activeGameObject;
        if (!selectedGo) return;

        var fromWp = selectedGo.GetComponent<WayPoint>();
        if (!fromWp) return;
        
        GameObject newRouteObj = new GameObject($"Route_{_routeNumber++}");
        var newRoute = newRouteObj.AddComponent<WayPointManager>();
        
        GameObject newWpObj = new GameObject("WayPoint_0", typeof(WayPoint));
        newWpObj.transform.SetParent(newRouteObj.transform, false);
        
        var newWp = newWpObj.GetComponent<WayPoint>();
        
        newWp.transform.position = fromWp.transform.position;
        newWp.transform.forward  = fromWp.transform.forward;
        
        newWp.ownerRoute = newRoute;
        newRoute.RebuildListFromChildren();
        
        fromWp.branches.Add(newWp);
        
        parentTransform = newRouteObj.transform;
        _wayPointManager = newRoute;
        Repaint();
        
        Selection.activeGameObject = newWpObj;
    }

}
