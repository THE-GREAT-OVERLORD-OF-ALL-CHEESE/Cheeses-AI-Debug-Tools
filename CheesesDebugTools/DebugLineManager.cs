using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DebugLineManager
{
    public DebugLineManager()
    {
        lineInfos = new List<DebugLineInfo>();
        lineRenderers = new List<LineRenderer>();

        shader = Shader.Find("Standard");
        material = new Material(shader);
    }

    public class DebugLineInfo
    {
        public DebugLineInfo(Vector3[] points, float width, Color colour)
        {
            this.points = points;
            this.width = width;
            this.colour = colour;
        }

        public Vector3[] points;
        public float width;
        public Color colour;
    }

    public List<DebugLineInfo> lineInfos;
    public List<LineRenderer> lineRenderers;

    public Shader shader;
    public Material material;

    public void AddLine(DebugLineInfo info)
    {
        lineInfos.Add(info);
    }

    public void AddCircle(DebugLineInfo info, Vector3 center, float radius, int segments)
    {
        Vector3[] pointList = new Vector3[segments + 1];
        for (int i = 0; i < pointList.Length; i++)
        {
            pointList[i] = center + Quaternion.Euler(0, (360f / (float)segments) * (float)i, 0) * Vector3.forward * radius;
        }
        info.points = pointList;

        lineInfos.Add(info);
    }

    public void UpdateLines()
    {
        if (lineInfos.Count != lineRenderers.Count) {
            DestroyAllLineRenderers();
            SpawnLineRenderers();
        }

        for (int i = 0; i < lineInfos.Count; i++)
        {
            lineRenderers[i].positionCount = lineInfos[i].points.Length;
            lineRenderers[i].SetPositions(lineInfos[i].points);
            lineRenderers[i].widthMultiplier = lineInfos[i].width;
            lineRenderers[i].material.color = lineInfos[i].colour;
        }

        lineInfos = new List<DebugLineInfo>();
    }

    public void DestroyAllLineRenderers()
    {
        while (lineRenderers.Count > 0)
        {
            if (lineRenderers[0] == null)
            {
                lineRenderers.RemoveAt(0);
            }
            else
            {
                GameObject.Destroy(lineRenderers[0].gameObject);
                lineRenderers.RemoveAt(0);
            }
        }
    }

    public void SpawnLineRenderers()
    {
        for (int i = 0; i < lineInfos.Count; i++)
        {
            GameObject linObj = new GameObject();
            LineRenderer lineRenderer = linObj.AddComponent<LineRenderer>();
            lineRenderers.Add(lineRenderer);
            lineRenderer.material = material;
        }
    }
}
