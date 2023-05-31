using System.Collections.Generic;
using UnityEngine;

public class QuickHull
{
    public static int[] GetConvexHullIndices(Vector3[] points)
    {
        if (points == null || points.Length < 4)
        {
            Debug.LogError("At least four points are required to compute a convex hull.");
            return null;
        }

        // Find the extreme points (leftmost, rightmost, topmost, bottommost, etc.)
        int leftmostIndex = 0;
        int rightmostIndex = 0;
        int topmostIndex = 0;
        int bottommostIndex = 0;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].x < points[leftmostIndex].x)
            {
                leftmostIndex = i;
            }
            if (points[i].x > points[rightmostIndex].x)
            {
                rightmostIndex = i;
            }
            if (points[i].y > points[topmostIndex].y)
            {
                topmostIndex = i;
            }
            if (points[i].y < points[bottommostIndex].y)
            {
                bottommostIndex = i;
            }
        }

        // Create a list of indices that define the convex hull
        List<int> hullIndices = new List<int>();

        // Add the extreme points to the hull
        hullIndices.Add(leftmostIndex);
        hullIndices.Add(rightmostIndex);
        hullIndices.Add(topmostIndex);
        hullIndices.Add(bottommostIndex);

        // Recursively find the rest of the hull
        FindHullRecursive(points, leftmostIndex, rightmostIndex, hullIndices);
        FindHullRecursive(points, rightmostIndex, topmostIndex, hullIndices);
        FindHullRecursive(points, topmostIndex, bottommostIndex, hullIndices);
        FindHullRecursive(points, bottommostIndex, leftmostIndex, hullIndices);

        return hullIndices.ToArray();
    }

    private static void FindHullRecursive(Vector3[] points, int index1, int index2, List<int> hullIndices)
    {
        // Find the point with the maximum distance from the line between index1 and index2
        float maxDistance = 0f;
        int maxDistanceIndex = -1;
        for (int i = 0; i < points.Length; i++)
        {
            if (i == index1 || i == index2 || hullIndices.Contains(i))
            {
                continue;
            }

            float distance = DistanceToLine(points[i], points[index1], points[index2]);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                maxDistanceIndex = i;
            }
        }

        // If there are no more points to add, return
        if (maxDistanceIndex == -1)
        {
            return;
        }

        // Add the point to the hull
        hullIndices.Add(maxDistanceIndex);

        // Recursively find the rest of the hull
        FindHullRecursive(points, index1, maxDistanceIndex, hullIndices);
        FindHullRecursive(points, maxDistanceIndex, index2, hullIndices);
    }

    private static float DistanceToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        // Calculate the direction and length of the line
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLength = lineDirection.magnitude;

        // If the line is too short, return the distance to either end point
        if (lineLength < 0.0001f)
        {
            return Vector3.Distance(point, lineStart);
        }

        // Normalize the line direction
        lineDirection /= lineLength;

        // Calculate the distance from the point to the line
        float distance = Vector3.Dot(point - lineStart, lineDirection);

        // Clamp the distance to be within the length of the line
        distance = Mathf.Clamp(distance, 0f, lineLength);

        // Calculate the point on the line that is closest to the point
        Vector3 closestPoint = lineStart + lineDirection * distance;

        // Calculate the distance from the point to the closest point on the line
        return Vector3.Distance(point, closestPoint);
    }

}