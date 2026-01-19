using UnityEngine;
using System.Collections.Generic;

namespace Empire15.GameLoop
{
    /// <summary>
    /// Represents a capturable zone that players can control
    /// Changes color based on capture status
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
        
        public bool IsActive => isActive;
        public bool IsCaptured => isCaptured;
        public float CaptureProgress => captureProgress;
        public string ZoneName => zoneName;
        
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
                captureProgress += Time.deltaTime / captureTime;
                captureProgress = Mathf.Clamp01(captureProgress);
                
                UpdateZoneColor();
                
                // Zone captured!
                if (captureProgress >= 1f)
                {
                    CompleteCapture();
                }
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
            
            // Notify game manager
            GameLoopManager.Instance?.OnZoneCaptured(this);
        }
        
        /// <summary>
        /// Activates this zone for capture
        /// </summary>
        public void Activate()
        {
            isActive = true;
            captureProgress = 0f;
            isCaptured = false;
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
            playersInZone.Clear();
            UpdateZoneColor();
        }
        
        private void OnDrawGizmosSelected()
        {
            // Visualize capture radius in editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, captureRadius);
        }
    }
}
