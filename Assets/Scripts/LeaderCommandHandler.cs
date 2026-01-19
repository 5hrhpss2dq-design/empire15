using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LeaderDrawing))]
public class LeaderCommandHandler : MonoBehaviour
{
    LeaderDrawing drawing;
    CaptureZone[] zones;

    void Start()
    {
        drawing = GetComponent<LeaderDrawing>();
        drawing.OnLineFinished += OnLineFinished;
        zones = FindObjectsOfType<CaptureZone>();
    }

    void OnDestroy()
    {
        if (drawing != null) drawing.OnLineFinished -= OnLineFinished;
    }

    void OnLineFinished(List<Vector3> points)
    {
        if (points == null || points.Count == 0) return;

        // Simple rule: pick the zone whose center is nearest to the last point
        Vector3 last = points[points.Count - 1];
        CaptureZone nearest = null;
        float best = float.MaxValue;
        foreach (var z in zones)
        {
            float d = Vector3.Distance(z.transform.position, last);
            if (d < best)
            {
                best = d;
                nearest = z;
            }
        }

        if (nearest != null)
        {
            // For prototype, force-activate the zone with leader id = current GameObject name
            nearest.ForceActivate(gameObject.name);
            Debug.Log($"LeaderCommand: activated zone {nearest.zoneName} from drawn line.");
        }
    }
}