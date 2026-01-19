using UnityEngine;
using System.Collections.Generic;
using System;

namespace Empire15.GameLoop
{
    /// <summary>
    /// Represents a capturable zone that players can control
    /// Changes color based on capture status
    /// Exposes events and API for game loop integration
    /// </summary>
    public class CaptureZone : MonoBehaviour
    {
        [Header("Zone Settings")]
        [SerializeField] private string zoneName = "Zone A";
        [SerializeField] private float captureRadius = 5f;
        [SerializeField] private float captureTime = 5f;
        
        [Header("Visual Settings")]
        [SerializeField] private Color neutralColor = Color.gray;
        [SerializeField] private Color capturingColor = Color.yellow;
        [SerializeField] private Color capturedColor = Color.green;
        [SerializeField] private Renderer zoneRenderer;
        
        // Zone state
        private bool isActive = false;
        private bool isCaptured = false;
        private float captureProgress = 0f;
        private List<GameObject> playersInZone = new List<GameObject>();
        private Material zoneMaterial;
        private string ownerId = "";
        
        // Events
        public event Action<CaptureZone> OnCaptured;
        
        public bool IsActive => isActive;
        public bool IsCaptured => isCaptured;
        public bool IsOwned => isCaptured && !string.IsNullOrEmpty(ownerId);
        public float CaptureProgress => captureProgress;
        public string ZoneName => zoneName;
        public string OwnerId => ownerId;
        
        private void Awake()
        {
            // Create instance material to avoid modifying shared material
            if (zoneRenderer != null)
            {
                zoneMaterial = zoneRenderer.material;
                UpdateZoneColor();
            }
        }
        
        private void Update()
        {
            if (!isActive || isCaptured)
                return;
            
            // Check if players are in zone
            CheckPlayersInZone();
            
            // Update capture progress
            if (playersInZone.Count > 0)
            {
                // Players in zone - increase progress
                captureProgress += Time.deltaTime / captureTime;
                captureProgress = Mathf.Clamp01(captureProgress);
                
                UpdateZoneColor();
                
                // Zone captured!
                if (captureProgress >= 1f)
                {
                    CompleteCapture();
                }
            }
            else if (captureProgress > 0 && !isCaptured)
            {
                // No players in zone - regress progress slowly
                captureProgress -= Time.deltaTime / (captureTime * 2f); // Regress at half speed
                captureProgress = Mathf.Max(0, captureProgress);
                UpdateZoneColor();
            }
        }
        
        private void CheckPlayersInZone()
        {
            playersInZone.Clear();
            
            // Find all player objects in range
            Collider[] colliders = Physics.OverlapSphere(transform.position, captureRadius);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    playersInZone.Add(col.gameObject);
                }
            }
        }
        
        private void UpdateZoneColor()
        {
            if (zoneMaterial == null)
                return;
            
            Color targetColor;
            
            if (isCaptured)
            {
                targetColor = capturedColor;
            }
            else if (captureProgress > 0)
            {
                targetColor = Color.Lerp(neutralColor, capturingColor, captureProgress);
            }
            else
            {
                targetColor = neutralColor;
            }
            
            zoneMaterial.color = targetColor;
        }
        
        private void CompleteCapture()
        {
            isCaptured = true;
            UpdateZoneColor();
            
            Debug.Log($"{zoneName} has been captured!");
            
            // Notify via event
            OnCaptured?.Invoke(this);
            
            // Notify game manager
            WarCycleManager.Instance?.OnZoneCaptured(this);
        }
        
        /// <summary>
        /// Activates this zone for capture
        /// </summary>
        public void Activate()
        {
            isActive = true;
            captureProgress = 0f;
            isCaptured = false;
            ownerId = "";
            UpdateZoneColor();
            Debug.Log($"{zoneName} is now active!");
        }
        
        /// <summary>
        /// Deactivates this zone
        /// </summary>
        public void Deactivate()
        {
            isActive = false;
            captureProgress = 0f;
            UpdateZoneColor();
        }
        
        /// <summary>
        /// Resets the zone to neutral state
        /// </summary>
        public void Reset()
        {
            isActive = false;
            isCaptured = false;
            captureProgress = 0f;
            ownerId = "";
            playersInZone.Clear();
            UpdateZoneColor();
        }
        
        /// <summary>
        /// Force activates this zone with a specific leader ID (leader command)
        /// </summary>
        /// <param name="leaderId">ID of the leader forcing activation</param>
        public void ForceActivate(string leaderId)
        {
            this.ownerId = leaderId;
            this.isCaptured = true;
            this.captureProgress = 1f;
            this.isActive = true;
            
            UpdateZoneColor();
            
            Debug.Log($"{zoneName} force-activated by leader {leaderId}!");
            
            // Notify via event
            OnCaptured?.Invoke(this);
            
            // Notify game manager
            WarCycleManager.Instance?.OnZoneCaptured(this);
        }
        
        /// <summary>
        /// Gets the capture progress as a value between 0 and 1
        /// </summary>
        public float GetProgress01()
        {
            return captureProgress;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Visualize capture radius in editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, captureRadius);
        }
    }
}
