using UnityEngine;
using UnityEngine.UI;

namespace Empire15.UI
{
    /// <summary>
    /// HUD Manager displaying role name, cycle timer, and current zone status
    /// Clean, military-style interface with event-driven updates
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("HUD References")]
        [SerializeField] private Text roleNameText;
        [SerializeField] private Text cycleTimerText;
        [SerializeField] private Text zoneStatusText;
        [SerializeField] private Text cycleNumberText;
        
        [Header("HUD Settings")]
        [SerializeField] private string playerRole = "SOLDIER";
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color warningColor = Color.yellow;
        [SerializeField] private Color criticalColor = Color.red;
        
        private GameLoop.WarCycleManager gameManager;
        
        private void Start()
        {
            gameManager = GameLoop.WarCycleManager.Instance;
            
            // Subscribe to events
            if (gameManager != null)
            {
                gameManager.OnCycleStart += HandleCycleStart;
                gameManager.OnCycleEnd += HandleCycleEnd;
                gameManager.OnZoneOwnershipChanged += HandleZoneOwnershipChanged;
            }
            
            // Set role name
            if (roleNameText != null)
            {
                roleNameText.text = $"ROLE: {playerRole}";
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameManager != null)
            {
                gameManager.OnCycleStart -= HandleCycleStart;
                gameManager.OnCycleEnd -= HandleCycleEnd;
                gameManager.OnZoneOwnershipChanged -= HandleZoneOwnershipChanged;
            }
        }
        
        private void Update()
        {
            UpdateTimer();
            UpdateZoneStatus();
            UpdateCycleNumber();
        }
        
        /// <summary>
        /// Handles cycle start event
        /// </summary>
        private void HandleCycleStart()
        {
            Debug.Log("HUD: New cycle started");
        }
        
        /// <summary>
        /// Handles cycle end event
        /// </summary>
        private void HandleCycleEnd()
        {
            Debug.Log("HUD: Cycle ended");
        }
        
        /// <summary>
        /// Handles zone ownership changed event
        /// </summary>
        private void HandleZoneOwnershipChanged(GameLoop.CaptureZone zone)
        {
            Debug.Log($"HUD: Zone {zone.ZoneName} ownership changed");
        }
        
        /// <summary>
        /// Updates the cycle timer display
        /// </summary>
        private void UpdateTimer()
        {
            if (cycleTimerText == null || gameManager == null)
                return;
            
            string timeString = gameManager.GetFormattedTime();
            cycleTimerText.text = $"TIME: {timeString}";
            
            // Change color based on time remaining
            float timeRemaining = gameManager.GetTimeRemaining();
            if (timeRemaining < 10f)
                cycleTimerText.color = criticalColor;
            else if (timeRemaining < 30f)
                cycleTimerText.color = warningColor;
            else
                cycleTimerText.color = normalColor;
        }
        
        /// <summary>
        /// Updates the zone status display
        /// </summary>
        private void UpdateZoneStatus()
        {
            if (zoneStatusText == null || gameManager == null)
                return;
            
            var activeZone = gameManager.ActiveZone;
            if (activeZone != null)
            {
                string status = activeZone.IsCaptured ? "CAPTURED" : "ACTIVE";
                float progress = activeZone.CaptureProgress * 100f;
                
                zoneStatusText.text = $"ZONE: {activeZone.ZoneName} - {status}\nPROGRESS: {progress:F0}%";
                
                // Color based on capture progress
                if (activeZone.IsCaptured)
                    zoneStatusText.color = Color.green;
                else if (progress > 0)
                    zoneStatusText.color = warningColor;
                else
                    zoneStatusText.color = normalColor;
            }
            else
            {
                zoneStatusText.text = "ZONE: NO ACTIVE ZONE";
                zoneStatusText.color = normalColor;
            }
        }
        
        /// <summary>
        /// Updates the cycle number display
        /// </summary>
        private void UpdateCycleNumber()
        {
            if (cycleNumberText == null || gameManager == null)
                return;
            
            cycleNumberText.text = $"CYCLE: {gameManager.CurrentCycle}";
        }
        
        /// <summary>
        /// Sets the player role displayed in the HUD
        /// </summary>
        public void SetRole(string role)
        {
            playerRole = role;
            if (roleNameText != null)
            {
                roleNameText.text = $"ROLE: {playerRole}";
            }
        }
    }
}
