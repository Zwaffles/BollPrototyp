using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatmullRomSpline : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    private List<Vector3> splinePoints = new List<Vector3>();
    private const int stepsPerSegment = 10;

    private void OnValidate()
    {
        // Recalculate spline points when control points change
        CalculateSplinePoints();
    }

    private void CalculateSplinePoints()
    {
        splinePoints.Clear();

        // Add first point
        splinePoints.Add(controlPoints[0].position);

        // Calculate spline points between each segment
        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            for (int j = 1; j <= stepsPerSegment; j++)
            {
                float t = j / (float)stepsPerSegment;
                Vector3 splinePoint = CalculateCatmullRomSpline(controlPoints[i], controlPoints[i + 1], t);
                splinePoints.Add(splinePoint);
            }
        }

        // Add last point
        splinePoints.Add(controlPoints[controlPoints.Length - 1].position);
    }

    private Vector3 CalculateCatmullRomSpline(Transform p0, Transform p1, float t)
    {
        Vector3 a = p0.position;
        Vector3 b = p0.position + p0.forward * p0.localScale.z * 0.5f;
        Vector3 c = p1.position - p1.forward * p1.localScale.z * 0.5f;
        Vector3 d = p1.position;

        // Calculate spline point using Catmull-Rom formula
        Vector3 splinePoint = 0.5f * ((2.0f * b) +
            (-a + c) * t +
            (2.0f * a - 5.0f * b + 4.0f * c - d) * t * t +
            (-a + 3.0f * b - 3.0f * c + d) * t * t * t);

        return splinePoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < splinePoints.Count - 1; i++)
        {
            Gizmos.DrawLine(splinePoints[i], splinePoints[i + 1]);
        }

        // Draw control points as draggable handles
        for (int i = 0; i < controlPoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(controlPoints[i].position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(controlPoints[i], "Move Control Point");
                controlPoints[i].position = newPos;
                CalculateSplinePoints();
            }
        }
    }
}