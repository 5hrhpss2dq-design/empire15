using UnityEngine;

namespace Empire15.Utilities
{
    /// <summary>
    /// Helper component for quick scene setup and testing
    /// Attach to an empty GameObject in the scene
    /// </summary>
    public class SceneSetupHelper : MonoBehaviour
    {
        [Header("Auto-Setup Options")]
        [SerializeField] private bool autoFindPlayer = true;
        [SerializeField] private bool autoFindCamera = true;
        [SerializeField] private bool autoFindManagers = true;
        
        [Header("Debug Options")]
        [SerializeField] private bool showDebugInfo = true;
        [SerializeField] private bool showZoneGizmos = true;
        
        private void Start()
        {
            if (autoFindPlayer)
                ValidatePlayer();
            
            if (autoFindCamera)
                ValidateCamera();
            
            if (autoFindManagers)
                ValidateManagers();
            
            if (showDebugInfo)
                LogSceneSetup();
        }
        
        private void ValidatePlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No player found with 'Player' tag!");
                return;
            }
            
            var controller = player.GetComponent<Movement.ThirdPersonController>();
            if (controller == null)
                Debug.LogWarning("Player missing ThirdPersonController component!");
            
            var roleManager = player.GetComponent<Strategy.PlayerRoleManager>();
            if (roleManager == null)
                Debug.LogWarning("Player missing PlayerRoleManager component!");
            
            var characterController = player.GetComponent<CharacterController>();
            if (characterController == null)
                Debug.LogWarning("Player missing CharacterController component!");
        }
        
        private void ValidateCamera()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                Debug.LogWarning("No main camera found!");
                return;
            }
            
            var camController = cam.GetComponent<Camera.CameraController>();
            if (camController == null)
                Debug.LogWarning("Camera missing CameraController component!");
        }
        
        private void ValidateManagers()
        {
            var gameManager = FindObjectOfType<GameLoop.WarCycleManager>();
            if (gameManager == null)
                Debug.LogWarning("No WarCycleManager found in scene!");
            
            var spawnManager = FindObjectOfType<GameLoop.SpawnManager>();
            if (spawnManager == null)
                Debug.LogWarning("No SpawnManager found in scene!");
            
            var hud = FindObjectOfType<UI.HUDManager>();
            if (hud == null)
                Debug.LogWarning("No HUDManager found in scene!");
        }
        
        private void LogSceneSetup()
        {
            Debug.Log("=== Empire 15 Scene Setup ===");
            
            var zones = FindObjectsOfType<GameLoop.CaptureZone>();
            Debug.Log($"Capture Zones: {zones.Length}");
            
            var player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log($"Player: {(player != null ? "Found" : "Missing")}");
            
            var cam = Camera.main;
            Debug.Log($"Main Camera: {(cam != null ? "Found" : "Missing")}");
            
            Debug.Log("=========================");
        }
        
        private void OnDrawGizmos()
        {
            if (!showZoneGizmos)
                return;
            
            var zones = FindObjectsOfType<GameLoop.CaptureZone>();
            foreach (var zone in zones)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(zone.transform.position, 5f);
            }
        }
    }
}
