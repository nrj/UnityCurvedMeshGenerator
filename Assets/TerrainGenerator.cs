using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh = null;
    Vector3[] curve = new Vector3[4];
    List<Vector3> vertices = new List<Vector3> ();
    List<int> triangles = new List<int> ();

    void Start ()
    {
        MeshFilter filter = GetComponent<MeshFilter> ();
        mesh = filter.mesh;
        mesh.Clear ();

        var xPos = 0f;
        for (int i = 0; i < curve.Length; i++) {
            curve [i] = new Vector3 (xPos, Random.Range (1f, 2f), 0f);
            xPos += 0.5f;
        }

        int resolution = 20;
        for (int i = 0; i < resolution; i++) {
            float t = (float)i / (float)(resolution - 1);
            Vector3 p = CalculateBezierPoint (t, curve [0], curve [1], curve [2], curve [3]);
            AddTerrainPoint (p);
        }

        mesh.vertices = vertices.ToArray ();
        mesh.triangles = triangles.ToArray ();
    }

    void AddTerrainPoint (Vector3 point)
    {
        vertices.Add (new Vector3 (point.x, 0f, 0f));
        vertices.Add (point);

        if (vertices.Count >= 4) {
            int start = vertices.Count - 4;
            triangles.Add (start + 0);
            triangles.Add (start + 1);
            triangles.Add (start + 2);
            triangles.Add (start + 1);
            triangles.Add (start + 3);
            triangles.Add (start + 2);
        }
    }

    private Vector3 CalculateBezierPoint (float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}