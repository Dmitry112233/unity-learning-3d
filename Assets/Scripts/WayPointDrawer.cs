using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class WayPointDrawer
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawGizmo(WayPoint wayPoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }
        
        Gizmos.DrawSphere(wayPoint.transform.position, 0.5f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wayPoint.transform.position + wayPoint.transform.right * wayPoint.width / 2f,
            wayPoint.transform.position - wayPoint.transform.right * wayPoint.width / 2);

        if (wayPoint.prevWayPoint)
        {
            Gizmos.color = Color.red;
            Vector3 offset = wayPoint.transform.right * wayPoint.prevWayPoint.width / 2f;
            Vector3 offsetTo = wayPoint.prevWayPoint.transform.right * wayPoint.prevWayPoint.width / 2f;
            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.prevWayPoint.transform.position + offsetTo);
        }

        if (wayPoint.nextWayPoint)
        {
            Gizmos.color = Color.green;
            Vector3 offset = wayPoint.transform.right * -wayPoint.width / 2f;
            Vector3 offsetTo = wayPoint.nextWayPoint.transform.right * -wayPoint.nextWayPoint.width / 2f;
            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.nextWayPoint.transform.position + offsetTo);
        }
    }
}
