using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    [Header("Points")]
    public List<Vector2> points = new();

    [Header("Line Settings")]
    [Min(1f)] public float lineWidth = 4f;
    public bool closedLoop = false;

    [Header("Caps & Joins")]
    public bool roundCaps = true;
    [Range(4, 24)] public int capSegments = 8;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Count < 2) return;

        int count = closedLoop ? points.Count : points.Count - 1;

        for (int i = 0; i < count; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[(i + 1) % points.Count];
            AddSegment(vh, a, b);
        }

        if (roundCaps && !closedLoop)
        {
            AddCap(vh, points[0],
                (points[0] - points[1]).normalized);
            AddCap(vh, points[^1],
                (points[^1] - points[^2]).normalized);
        }
    }

    void AddSegment(VertexHelper vh, Vector2 a, Vector2 b)
    {
        Vector2 dir = (b - a).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x) * (lineWidth * 0.5f);

        int start = vh.currentVertCount;

        vh.AddVert(a - perp, color, Vector2.zero);
        vh.AddVert(a + perp, color, Vector2.zero);
        vh.AddVert(b + perp, color, Vector2.zero);
        vh.AddVert(b - perp, color, Vector2.zero);

        vh.AddTriangle(start,     start + 1, start + 2);
        vh.AddTriangle(start,     start + 2, start + 3);
    }

    void AddCap(VertexHelper vh, Vector2 center, Vector2 outDir)
    {
        float r = lineWidth * 0.5f;
        float baseAngle = Mathf.Atan2(outDir.y, outDir.x) * Mathf.Rad2Deg;

        int centerIdx = vh.currentVertCount;
        vh.AddVert(center, color, Vector2.zero);

        for (int i = 0; i <= capSegments; i++)
        {
            float t = i / (float)capSegments;
            float angle = (baseAngle - 90f + t * 180f) * Mathf.Deg2Rad;
            Vector2 offset = new(Mathf.Cos(angle), Mathf.Sin(angle));
            vh.AddVert(center + offset * r, color, Vector2.zero);
        }

        for (int i = 0; i < capSegments; i++)
        {
            vh.AddTriangle(centerIdx, centerIdx + i + 1, centerIdx + i + 2);
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetVerticesDirty();
    }
#endif

    public void SetPoints(IEnumerable<Vector2> newPoints)
    {
        points = new List<Vector2>(newPoints);
        SetVerticesDirty();
    }

    public void AddPoint(Vector2 p)
    {
        points.Add(p);
        SetVerticesDirty();
    }

    public void ClearPoints()
    {
        points.Clear();
        SetVerticesDirty();
    }
}