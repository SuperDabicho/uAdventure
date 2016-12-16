﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Helpers;

public class GMLGeometry
{

    public enum GeometryType { Point, LineString, Polygon }

    public GMLGeometry()
    {
        Points = new List<Vector2d>();
    }

    public GeometryType Type { get; set; }
    public List<Vector2d> Points { get; set; }

    public void AddPoint(Vector2d point)
    {
        switch (Type)
        {
            case GMLGeometry.GeometryType.Point:
                if (Points.Count == 1) Points[0] = point;
                else Points.Add(point);
                break;
            case GMLGeometry.GeometryType.LineString:
                Points.Add(point);
                break;
            case GMLGeometry.GeometryType.Polygon:
                if (Points.Count <= 1)
                {
                    Points.Add(point);
                }
                else
                {
                    // Find the closest index
                    var min = Points.Min(p => (p - point).magnitude);
                    var closest = Points.FindIndex(p => (p - point).magnitude == min);

                    // Fix the previous and next
                    var prev = closest == 0 ? Points.Count - 1 : closest - 1;
                    var next = (closest + 1) % Points.Count;
                    // Calculate the normal to both adjacent axis to closest point
                    var c = Points[closest];
                    var v1 = (Points[closest] - Points[prev]).normalized;
                    var v2 = (Points[closest] - Points[next]).normalized;

                    var closestNormal = (v1 + v2).normalized;
                    var convex = Vector3.Cross(v1.ToVector2(), v2.ToVector2()).z > 0;

                    var pointVector = (point - c);
                    var left = Vector3.Cross(closestNormal.ToVector2(), pointVector.ToVector2()).z > 0;

                    Debug.Log(convex ? "Convex" : "Concave");
                    if ((left && convex) || (!left && !convex))
                    {
                        Debug.Log("Prev");
                        // We insert at the closest
                        Points.Insert(closest, point);
                    }
                    else
                    {
                        Debug.Log("Next");
                        // We insert at the next
                        Points.Insert(next, point);
                    }
                }
                break;
        }
    }
    
    public bool Inside(Vector2d point)
    {

        var originPoints = Points.ConvertAll(p => p - point);

        var inside = false;
        for (int i = 0; i < originPoints.Count - 1; i++)
        {
            if (((originPoints[i].y > 0) != (originPoints[i + 1].y > 0))
            && ((originPoints[i].y > 0) == (originPoints[i].y * originPoints[i + 1].x > originPoints[i + 1].y * originPoints[i].x)))
                inside = !inside;
        }

        return inside;
    }
}
