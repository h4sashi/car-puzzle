using UnityEngine;
using System.Collections.Generic;

public static class BezierUtility
{
    public static List<Vector3> GenerateQuadraticCurve(Vector3 p0, Vector3 p1, Vector3 p2, int resolution = 20)
    {
        List<Vector3> curve = new List<Vector3>();
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = Mathf.Pow(1 - t, 2) * p0 +
                            2 * (1 - t) * t * p1 +
                            Mathf.Pow(t, 2) * p2;
            curve.Add(point);
        }
        return curve;
    }

    public static List<Vector3> GenerateCubicCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution = 20)
    {
        List<Vector3> curve = new List<Vector3>();
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = Mathf.Pow(1 - t, 3) * p0 +
                            3 * Mathf.Pow(1 - t, 2) * t * p1 +
                            3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                            Mathf.Pow(t, 3) * p3;
            curve.Add(point);
        }
        return curve;
    }
}
