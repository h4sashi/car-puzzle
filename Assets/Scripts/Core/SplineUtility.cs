using UnityEngine;
using System.Collections.Generic;

public static class SplineUtility
{
    
    public static List<Vector3> GenerateCatmullRomSpline(List<Vector3> controlPoints, int resolution = 20)
    {
        List<Vector3> splinePoints = new List<Vector3>();

        if (controlPoints.Count < 4)
        {
            Debug.LogWarning("Catmull-Rom spline requires at least 4 points");
            return controlPoints;
        }

        for (int i = 0; i < controlPoints.Count - 3; i++)
        {
            Vector3 p0 = controlPoints[i];
            Vector3 p1 = controlPoints[i + 1];
            Vector3 p2 = controlPoints[i + 2];
            Vector3 p3 = controlPoints[i + 3];

            for (int j = 0; j <= resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 point = 0.5f * (
                    2f * p1 +
                    (-p0 + p2) * t +
                    (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                    (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
                );
                splinePoints.Add(point);
            }
        }

        return splinePoints;
    }
}