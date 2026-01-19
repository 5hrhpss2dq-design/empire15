using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CaptureZone : MonoBehaviour
{
    public string zoneName = "Zone";
    public float captureTime = 5f; // time required to capture
    public Color neutralColor = Color.gray;
    public Color contestedColor = Color.yellow;
    public Color ownedColor = Color.green;

    public event Action<CaptureZone> OnCaptured;
    public bool IsOwned { get; private set; }
    public string OwnerId { get; private set; } = "";

    HashSet<GameObject> occupants = new HashSet<GameObject>();
    float progress = 0f;
    Renderer rend;

    void Start()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
        rend = GetComponent<Renderer>();
        if (rend) rend.material.color = neutralColor;
    }

    void Update()
    {
        // Simple occupancy-based capture progress
        if (occupants.Count > 0)
        {
            progress += Time.deltaTime;
            if (rend) rend.material.color = contestedColor;
            if (!IsOwned && progress >= captureTime)
            {
                // For prototype: owner is string of first occupant's name
                foreach (var go in occupants) { OwnerId = go.name; break; }
                IsOwned = true;
                progress = captureTime;
                if (rend) rend.material.color = ownedColor;
                OnCaptured?.Invoke(this);
            }
        }
        else
        {
            // regress capture progress slowly
            if (progress > 0f) progress = Mathf.Max(0f, progress - Time.deltaTime * 0.5f);
            if (!IsOwned && rend) rend.material.color = neutralColor;
        }
    }

    public float GetProgress01() => Mathf.Clamp01(progress / captureTime);

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        occupants.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        occupants.Remove(other.gameObject);
    }

    // For leader assignment: force capture/activation
    public void ForceActivate(string leaderId)
    {
        OwnerId = leaderId;
        IsOwned = true;
        progress = captureTime;
        if (rend) rend.material.color = ownedColor;
        OnCaptured?.Invoke(this);
    }
}