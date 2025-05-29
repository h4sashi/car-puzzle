using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public enum ColorType { Red, Green, Blue }

    [System.Serializable]
    public struct SplinePath
    {
        public Transform[] controlPoints;
        public ArrowType arrow;
        public ColorType colorType;
    }

    public SplinePath[] splinePaths;
    public GameObject carPrefab;

    void Start()
    {
        SpawnSplineCars();
    }

    void SpawnSplineCars()
    {
        int i = 0;
        foreach (var path in splinePaths)
        {
            if (path.controlPoints.Length < 4)
            {
                Debug.LogWarning("Spline path requires at least 4 control points.");
                continue;
            }

            List<Vector3> ctrlPoints = new List<Vector3>();
            foreach (var pt in path.controlPoints)
                ctrlPoints.Add(pt.position);

            List<Vector3> spline = SplineUtility.GenerateCatmullRomSpline(ctrlPoints);

            GameObject car = Instantiate(carPrefab, spline[0], Quaternion.identity);
            i = i + 1;
            car.name = "Car "+i;
            CarController cc = car.GetComponent<CarController>();
            cc.Initialize(Vector3.zero, path.arrow, spline);
        }
    }

    void OnDrawGizmos()
    {
        if (splinePaths != null)
        {
            foreach (var path in splinePaths)
            {
                if (path.controlPoints.Length < 4) continue;

                List<Vector3> ctrlPoints = new List<Vector3>();
                foreach (var pt in path.controlPoints)
                    ctrlPoints.Add(pt.position);

                List<Vector3> spline = SplineUtility.GenerateCatmullRomSpline(ctrlPoints);

                switch (path.colorType)
                {
                    case ColorType.Red: Gizmos.color = Color.red; break;
                    case ColorType.Green: Gizmos.color = Color.green; break;
                    case ColorType.Blue: Gizmos.color = Color.blue; break;
                    default: Gizmos.color = Color.cyan; break;
                }

                for (int i = 0; i < spline.Count - 1; i++)
                    Gizmos.DrawLine(spline[i], spline[i + 1]);
            }
        }
    }
}


public enum ArrowType { Up, Down, Left, Right, LeftTurn, RightTurn, UTurn }
