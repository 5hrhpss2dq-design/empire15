using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Empire15.Strategy
{
    /// <summary>
    /// Handles leader commands and connects drawn paths to gameplay
    /// Subscribes to LeaderDrawing events and activates nearest zones
    /// </summary>
    public class LeaderCommandHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LeaderDrawing leaderDrawing;
        
        [Header("Settings")]
        [SerializeField] private string leaderId = "Leader1";
        [SerializeField] private bool showVisualFeedback = true;
        
        private void Start()
        {
            // Auto-find LeaderDrawing if not assigned
            if (leaderDrawing == null)
            {
                leaderDrawing = GetComponent<LeaderDrawing>();
                if (leaderDrawing == null)
                {
                    leaderDrawing = FindObjectOfType<LeaderDrawing>();
                }
            }
            
            // Subscribe to drawing events
            if (leaderDrawing != null)
            {
                leaderDrawing.OnLineFinished += HandleLineFinished;
                Debug.Log("LeaderCommandHandler subscribed to LeaderDrawing events");
            }
            else
            {
                Debug.LogWarning("LeaderCommandHandler: No LeaderDrawing component found!");
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (leaderDrawing != null)
            {
                leaderDrawing.OnLineFinished -= HandleLineFinished;
            }
        }
        
        /// <summary>
        /// Handles the OnLineFinished event from LeaderDrawing
        /// </summary>
        private void HandleLineFinished(List<Vector3> path)
        {
            if (path == null || path.Count < 2)
            {
                Debug.LogWarning("LeaderCommandHandler: Path too short to process");
                return;
            }
            
            Debug.Log($"LeaderCommandHandler: Processing path with {path.Count} points");
            
            // Get the endpoint of the path
            Vector3 endpoint = path[path.Count - 1];
            
            // Find the nearest capture zone to the endpoint
            GameLoop.CaptureZone nearestZone = FindNearestZone(endpoint);
            
            if (nearestZone != null)
            {
                // Force activate the zone with leader ID
                nearestZone.ForceActivate(leaderId);
                
                Debug.Log($"LeaderCommandHandler: Activated {nearestZone.ZoneName} via leader command");
                
                // Show visual feedback
                if (showVisualFeedback)
                {
                    ShowActivationFeedback(nearestZone.transform.position);
                }
            }
            else
            {
                Debug.LogWarning("LeaderCommandHandler: No capture zones found near path endpoint");
            }
        }
        
        /// <summary>
        /// Finds the nearest capture zone to a given position
        /// </summary>
        private GameLoop.CaptureZone FindNearestZone(Vector3 position)
        {
            var allZones = FindObjectsOfType<GameLoop.CaptureZone>();
            
            if (allZones == null || allZones.Length == 0)
                return null;
            
            // Find closest zone
            GameLoop.CaptureZone nearest = null;
            float minDistance = float.MaxValue;
            
            foreach (var zone in allZones)
            {
                float distance = Vector3.Distance(position, zone.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = zone;
                }
            }
            
            return nearest;
        }
        
        /// <summary>
        /// Shows visual feedback when a zone is activated
        /// </summary>
        private void ShowActivationFeedback(Vector3 position)
        {
            // Create a simple particle effect or visual indicator
            GameObject feedback = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            feedback.transform.position = position + Vector3.up * 2f;
            feedback.transform.localScale = Vector3.one * 0.5f;
            
            // Make it glow
            Renderer renderer = feedback.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = Color.cyan;
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.cyan * 2f);
                renderer.material = mat;
            }
            
            // Destroy after a short time
            Destroy(feedback, 2f);
            
            Debug.Log("LeaderCommandHandler: Showing activation feedback");
        }
        
        /// <summary>
        /// Sets the leader ID for this handler
        /// </summary>
        public void SetLeaderId(string id)
        {
            leaderId = id;
        }
    }
}
