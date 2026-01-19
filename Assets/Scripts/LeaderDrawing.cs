using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LeaderDrawing : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public Camera mainCamera;

    // Event to notify others (LeaderCommandHandler)
    public Action<List<Vector3>> OnLineFinished;

    void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (mainCamera == null) mainCamera = Camera.main;
        lineRenderer.positionCount = 0;
        lineRenderer.widthMultiplier = 1.8f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginLine();
        }
        else if (Input.GetMouseButton(0))
        {
            AddPointUnderMouse();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndLine();
        }
    }

    void BeginLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    void AddPointUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
        {
            Vector3 pos = hit.point;
            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], pos) > 1f)
            {
                points.Add(pos);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
            }
        }
    }

    void EndLine()
    {
        Debug.Log($"LeaderDrawing: finished line with {points.Count} points.");
        OnLineFinished?.Invoke(new List<Vector3>(points));
    }
}