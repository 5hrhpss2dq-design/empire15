using UnityEngine;

namespace Empire15.Strategy
{
    /// <summary>
    /// Defines player role (Leader or Soldier) and enables/disables appropriate systems
    /// </summary>
    public enum PlayerRole
    {
        Soldier,
        Leader
    }
    
    public class PlayerRoleManager : MonoBehaviour
    {
        [Header("Role Settings")]
        [SerializeField] private PlayerRole currentRole = PlayerRole.Soldier;
        
        // Component references
        private LeaderDrawing drawingSystem;
        private UI.HUDManager hud;
        
        private void Start()
        {
            // Get components
            drawingSystem = GetComponent<LeaderDrawing>();
            if (drawingSystem == null)
                drawingSystem = gameObject.AddComponent<LeaderDrawing>();
            
            // Find HUD
            hud = FindObjectOfType<UI.HUDManager>();
            
            // Apply role
            ApplyRole();
        }
        
        private void Update()
        {
            // Allow role switching for testing (R key)
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleRole();
            }
        }
        
        /// <summary>
        /// Applies the current role settings
        /// </summary>
        private void ApplyRole()
        {
            if (drawingSystem != null)
            {
                drawingSystem.enabled = (currentRole == PlayerRole.Leader);
            }
            
            if (hud != null)
            {
                hud.SetRole(currentRole.ToString().ToUpper());
            }
            
            Debug.Log($"Player role set to: {currentRole}");
        }
        
        /// <summary>
        /// Sets the player role
        /// </summary>
        public void SetRole(PlayerRole role)
        {
            currentRole = role;
            ApplyRole();
        }
        
        /// <summary>
        /// Toggles between Leader and Soldier roles
        /// </summary>
        public void ToggleRole()
        {
            currentRole = (currentRole == PlayerRole.Leader) ? PlayerRole.Soldier : PlayerRole.Leader;
            ApplyRole();
        }
        
        /// <summary>
        /// Gets the current player role
        /// </summary>
        public PlayerRole GetRole()
        {
            return currentRole;
        }
        
        /// <summary>
        /// Checks if player is a leader
        /// </summary>
        public bool IsLeader()
        {
            return currentRole == PlayerRole.Leader;
        }
    }
}
