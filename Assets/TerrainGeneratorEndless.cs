using UnityEngine;
using System.Collections.Generic;

public class TerrainGeneratorEndless : MonoBehaviour
{
    Mesh mesh = null;
    List<Vector3[]> curves = new List<Vector3[]> ();
    List<Vector3> vertices = new List<Vector3> ();
    List<int> triangles = new List<int> ();

    void Start ()
    {
        var filter = GetComponent<MeshFilter> ();
        mesh = filter.mesh;
        mesh.Clear ();

        var xPos = 0f;
        for (int c = 0; c < 10; c++) {
            var curve = new Vector3[4];
            for (int i = 0; i < curve.Length; i++) {
                Vector3[] prev = null;
                if (curves.Count > 0) {
                    prev = curves [curves.Count - 1];
                }
                if (prev != null && i == 0) {
                    curve [i] = prev [curve.Length - 1];
                } else if (prev != null && i == 1) {
                    curve [i] = 2f * prev [curve.Length - 1] - prev [curve.Length - 2];
                } else {
                    curve [i] = new Vector3 (xPos, Random.Range (1f, 2f), 0f);
                }
                // AddTerrainPoint (curve [i]);
                xPos += 0.5f;
            }
            curves.Add (curve);
        }

        foreach (var curve in curves) {
            int resolution = 20;
            for (int i = 0; i < resolution; i++) {
                float t = (float)i / (float)(resolution - 1);
                Vector3 p = CalculateBezierPoint (t, curve [0], curve [1], curve [2], curve [3]);
                AddTerrainPoint (p);
            }
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