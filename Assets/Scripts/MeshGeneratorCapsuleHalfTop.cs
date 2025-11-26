using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGeneratorCapsuleHalfTop : MonoBehaviour
{
    [Header("Capsule settings")]
    public float radius = 0.5f;
    public int segments = 24;   // вокруг
    public int rings = 12;      // от экватора к вершине

    private Mesh _mesh;
    private List<Vector3> _vertices = new();
    private List<int> _triangles = new();

    private void OnValidate()
    {
        Generate();
    }

    private void Reset()
    {
        Generate();
    }

    public void Generate()
    {
        _mesh = new Mesh();
        _mesh.name = "CapsuleHalfTop";

        _vertices.Clear();
        _triangles.Clear();

        // ---------- ВЕРШИНЫ ----------
        for (int r = 0; r <= rings; r++)
        {
            float phi = Mathf.Lerp(0f, Mathf.PI / 2f, (float)r / rings); // 0..90°
            Debug.Log("______" + phi);

            for (int s = 0; s < segments; s++)
            {
                float theta = (s / (float)segments) * Mathf.PI * 2f;

                float x = radius * Mathf.Cos(phi) * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(phi);
                float z = radius * Mathf.Cos(phi) * Mathf.Sin(theta);

                _vertices.Add(new Vector3(x, y, z));
            }
        }

        // ---------- ТРИАНГУЛЯЦИЯ ----------
        for (int r = 0; r < rings; r++)
        {
            for (int s = 0; s < segments; s++)
            {
                int current = r * segments + s;
                int next = current + segments;
                int nextS = (s + 1) % segments;

                int a = current;
                int b = next;
                int c = r * segments + nextS;

                int d = next;
                int e = next + nextS - s;
                int f = r * segments + nextS;

                // 1-й треугольник
                _triangles.Add(a);
                _triangles.Add(b);
                _triangles.Add(c);

                // 2-й треугольник
                _triangles.Add(b);
                _triangles.Add(e);
                _triangles.Add(c);
            }
        }

        _mesh.SetVertices(_vertices);
        _mesh.SetTriangles(_triangles, 0);
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = _mesh;
    }

    // ---------- GIZMOS: нумерация вершин + линий треугольников + нумерация треугольников ----------
    private void OnDrawGizmos()
    {
        if (_vertices == null || _vertices.Count == 0) return;

        // ------ Рисуем вершины + номера ------
        Gizmos.color = Color.red;
        for (int i = 0; i < _vertices.Count; i++)
        {
            Vector3 wp = transform.TransformPoint(_vertices[i]);
            Gizmos.DrawSphere(wp, 0.015f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(wp, i.ToString());
#endif
        }

        // ------ Рисуем линии треугольников ------
        Gizmos.color = Color.green;
        for (int i = 0; i < _triangles.Count; i += 3)
        {
            int i0 = _triangles[i];
            int i1 = _triangles[i + 1];
            int i2 = _triangles[i + 2];

            Vector3 p0 = transform.TransformPoint(_vertices[i0]);
            Vector3 p1 = transform.TransformPoint(_vertices[i1]);
            Vector3 p2 = transform.TransformPoint(_vertices[i2]);

            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p0);

#if UNITY_EDITOR
            // центр треугольника для подписи
            Vector3 center = (p0 + p1 + p2) / 3f;
            UnityEditor.Handles.Label(center, $"T{i/3}");
#endif
        }
    }
}
