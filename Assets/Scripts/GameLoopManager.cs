using UnityEngine;

namespace Empire15.GameLoop
{
    /// <summary>
    /// Manages the core game loop: Spawn → Move → Capture → Zone Changes → Timer Updates → Repeat
    /// Controls zone activation cycle and game state
    /// </summary>
    public class GameLoopManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private float cycleDuration = 60f; // 60 seconds per cycle
        [SerializeField] private CaptureZone[] captureZones;
        
        // Singleton
        private static GameLoopManager instance;
        public static GameLoopManager Instance => instance;
        
        // Game state
        private float cycleTimer = 0f;
        private int currentCycleNumber = 1;
        private CaptureZone activeZone;
        private int activeZoneIndex = 0;
        
        public float CycleTimer => cycleTimer;
        public float CycleDuration => cycleDuration;
        public int CurrentCycle => currentCycleNumber;
        public CaptureZone ActiveZone => activeZone;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        
        private void Start()
        {
            InitializeGameLoop();
        }
        
        private void Update()
        {
            UpdateCycleTimer();
        }
        
        /// <summary>
        /// Initializes the game loop with the first active zone
        /// </summary>
        private void InitializeGameLoop()
        {
            if (captureZones == null || captureZones.Length == 0)
            {
                Debug.LogError("No capture zones assigned to GameLoopManager!");
                return;
            }
            
            // Reset all zones
            foreach (var zone in captureZones)
            {
                zone.Reset();
            }
            
            // Activate first zone
            ActivateNextZone();
            
            Debug.Log($"Game Loop Started - Cycle {currentCycleNumber}");
        }
        
        /// <summary>
        /// Updates the cycle timer and triggers zone changes
        /// </summary>
        private void UpdateCycleTimer()
        {
            cycleTimer += Time.deltaTime;
            
            // Check if cycle is complete
            if (cycleTimer >= cycleDuration)
            {
                EndCycle();
            }
        }
        
        /// <summary>
        /// Called when a zone is captured
        /// </summary>
        public void OnZoneCaptured(CaptureZone zone)
        {
            if (zone == activeZone)
            {
                Debug.Log($"Active zone captured! Moving to next zone...");
                
                // Small delay before activating next zone
                Invoke(nameof(ActivateNextZone), 2f);
            }
        }
        
        /// <summary>
        /// Activates the next zone in sequence
        /// </summary>
        private void ActivateNextZone()
        {
            // Deactivate current zone
            if (activeZone != null)
            {
                activeZone.Deactivate();
            }
            
            // Move to next zone (cycle through all zones)
            activeZoneIndex = (activeZoneIndex + 1) % captureZones.Length;
            activeZone = captureZones[activeZoneIndex];
            activeZone.Activate();
            
            Debug.Log($"Zone activated: {activeZone.ZoneName}");
        }
        
        /// <summary>
        /// Ends the current cycle and starts a new one
        /// </summary>
        private void EndCycle()
        {
            Debug.Log($"Cycle {currentCycleNumber} complete!");
            
            // Reset timer
            cycleTimer = 0f;
            currentCycleNumber++;
            
            // Reset all zones
            foreach (var zone in captureZones)
            {
                zone.Reset();
            }
            
            // Start new cycle with first zone
            activeZoneIndex = -1; // Will be 0 after increment
            ActivateNextZone();
            
            Debug.Log($"Starting Cycle {currentCycleNumber}");
        }
        
        /// <summary>
        /// Gets time remaining in current cycle
        /// </summary>
        public float GetTimeRemaining()
        {
            return Mathf.Max(0, cycleDuration - cycleTimer);
        }
        
        /// <summary>
        /// Gets formatted time string (MM:SS)
        /// </summary>
        public string GetFormattedTime()
        {
            float timeRemaining = GetTimeRemaining();
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            return $"{minutes:00}:{seconds:00}";
        }
    }
}
