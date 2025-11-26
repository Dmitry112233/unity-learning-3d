using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] internal float _viewRadius = 20;
    
    [Range(0, 360)]
    [SerializeField] internal float _viewAngle = 90;
    
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;
    
    [SerializeField] internal List<Transform> _visibleTargets;
    
    [SerializeField] internal float _meshResolution;
    
    [SerializeField] private MeshFilter _meshFilter;
    private Mesh _mesh;

    private void Start()
    {
        _mesh =  new Mesh();
        _mesh.name = "FiledOfView";
        _meshFilter.mesh = _mesh;
        
        StartCoroutine(nameof(GetTargetsWithDelay), 1f);
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    IEnumerator GetTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GetVisibleTargets();
        }
    }

    private void GetVisibleTargets()
    {
        _visibleTargets.Clear();
        
       var results = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);
        
        results.ToList().ForEach(target =>
        {
            var dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, _obstacleMask))
                {
                    _visibleTargets.Add(target.transform);
                }
            }
        });
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
        }
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.CeilToInt(_viewAngle * _meshResolution);
        float stepAngleSize = _viewAngle / stepCount;
        
        List<Vector3> verticesOfRayCast = new List<Vector3>();
        
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;
            
            // Debug.DrawLine(
            //     transform.position,
            //     transform.position + DirFromAngle(angle, true) * _viewRadius,
            //     Color.red);
            
            ViewCastInfo viewCast = ViewCast(angle);
            
            verticesOfRayCast.Add(viewCast.Point);
        }
        
        int allVerticesCount = verticesOfRayCast.Count + 1;
        
        Vector3[] verticesForMesh = new Vector3[allVerticesCount];
        int[] trianglesForMesh = new int[(allVerticesCount -2) * 3];
        
        verticesForMesh[0] = Vector3.zero;

        for (int i = 0; i < allVerticesCount - 1; i++)
        {
            verticesForMesh[i + 1] = transform.InverseTransformPoint(verticesOfRayCast[i]);

            if (i < allVerticesCount - 2)
            {
                trianglesForMesh[i * 3] = 0;
                trianglesForMesh[i * 3 + 1] = i + 1;
                trianglesForMesh[i * 3 + 2] = i + 2;
            }
        }
        
        _mesh.Clear();
        _mesh.vertices = verticesForMesh;
        _mesh.triangles = trianglesForMesh;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        
    }

    public struct ViewCastInfo
    {
        public bool Hit;
        public Vector3 Point;
        public float Distance;
        public float Angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            Hit = hit;
            Point = point;
            Distance = distance;
            Angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 PointA;
        public Vector3 PointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }
    }
}
