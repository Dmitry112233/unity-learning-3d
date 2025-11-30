using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WayPoint : MonoBehaviour
{
    public WayPoint prevWayPoint;
    public WayPoint nextWayPoint;
    
    [FormerlySerializedAs("with")] [Range(0f, 5f)] public float width = 5f;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;
        
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
