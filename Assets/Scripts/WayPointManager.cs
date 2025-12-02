using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WayPointManager : MonoBehaviour
{
    public LinkedList<WayPoint> List { get; private set; } = new LinkedList<WayPoint>();

    private Dictionary<WayPoint, LinkedListNode<WayPoint>> _nodeMap =
        new Dictionary<WayPoint, LinkedListNode<WayPoint>>();

    private void OnEnable()
    {
        RebuildListFromChildren();
    }

    private void OnValidate()
    {
        RebuildListFromChildren();
    }

    public void RebuildListFromChildren()
    {
        List = new LinkedList<WayPoint>();
        _nodeMap.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.name = $"WayPoint_{i}";
            var wp = child.GetComponent<WayPoint>();
            if (wp == null) continue;

            var node = List.AddLast(wp);
            _nodeMap[wp] = node;
        }
    }
    
    public LinkedListNode<WayPoint> NodeOf(WayPoint wp)
    {
        _nodeMap.TryGetValue(wp, out var node);
        return node;
    }

    public LinkedListNode<WayPoint> AddLast(WayPoint wp)
    {
        var node = List.AddLast(wp);
        _nodeMap[wp] = node;
        return node;
    }

    public LinkedListNode<WayPoint> AddAfter(LinkedListNode<WayPoint> node, WayPoint wp)
    {
        var newNode = List.AddAfter(node, wp);
        _nodeMap[wp] = newNode;
        return newNode;
    }

    public void Remove(WayPoint wp)
    {
        if (_nodeMap.TryGetValue(wp, out var node))
        {
            List.Remove(node);
            _nodeMap.Remove(wp);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (List.Count < 2) return;

        Gizmos.color = Color.blue;

        var node = List.First;
        while (node?.Next != null)
        {
            Gizmos.DrawLine(node.Value.transform.position, node.Next.Value.transform.position);
            node = node.Next;
        }
    }
}