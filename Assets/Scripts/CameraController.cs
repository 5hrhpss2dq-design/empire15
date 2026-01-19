using UnityEngine;

namespace Empire15.Camera
{
    /// <summary>
    /// PUBG-like smooth third-person camera system
    /// Handles camera follow, rotation, and zoom with smooth interpolation
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 targetOffset = new Vector3(0, 1.7f, 0);
        
        [Header("Camera Distance")]
        [SerializeField] private float defaultDistance = 3.5f;
        [SerializeField] private float minDistance = 1.5f;
        [SerializeField] private float maxDistance = 6f;
        [SerializeField] private float zoomSpeed = 2f;
        
        [Header("Rotation Settings")]
        [SerializeField] private float mouseSensitivityX = 3f;
        [SerializeField] private float mouseSensitivityY = 2f;
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float minVerticalAngle = -35f;
        [SerializeField] private float maxVerticalAngle = 70f;
        
        [Header("Follow Settings")]
        [SerializeField] private float followSmoothTime = 0.15f;
        
        [Header("Collision Settings")]
        [SerializeField] private float collisionRadius = 0.3f;
        [SerializeField] private LayerMask collisionLayers = -1;
        
        // Camera state
        private float currentDistance;
        private float targetDistance;
        private Vector3 currentPosition;
        private Vector3 positionVelocity;
        
        // Rotation state
        private float currentYaw;
        private float currentPitch;
        private float yawVelocity;
        private float pitchVelocity;
        private float targetYaw;
        private float targetPitch;
        
        private void Start()
        {
            // Initialize camera position and rotation
            if (target != null)
            {
                Vector3 angles = transform.eulerAngles;
                currentYaw = targetYaw = angles.y;
                currentPitch = targetPitch = angles.x;
                currentDistance = targetDistance = defaultDistance;
                
                // Lock cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
        private void LateUpdate()
        {
            if (target == null)
                return;
            
            HandleInput();
            HandleRotation();
            HandlePosition();
        }
        
        private void HandleInput()
        {
            // Mouse input for rotation - RMB to orbit
            bool orbitMode = Input.GetMouseButton(1); // Right mouse button
            
            if (orbitMode || Cursor.lockState == CursorLockMode.Locked)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                
                targetYaw += mouseX * mouseSensitivityX;
                targetPitch -= mouseY * mouseSensitivityY;
                
                // Clamp vertical rotation
                targetPitch = Mathf.Clamp(targetPitch, minVerticalAngle, maxVerticalAngle);
            }
            
            // Mouse scroll for zoom
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                targetDistance -= scrollInput * zoomSpeed;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }
            
            // Toggle cursor lock
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
        
        private void HandleRotation()
        {
            // Smooth rotation interpolation
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, rotationSmoothTime);
            currentPitch = Mathf.SmoothDampAngle(currentPitch, targetPitch, ref pitchVelocity, rotationSmoothTime);
            
            // Apply rotation
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            transform.rotation = rotation;
        }
        
        private void HandlePosition()
        {
            // Calculate target position
            Vector3 targetPosition = target.position + targetOffset;
            
            // Calculate desired camera position
            Vector3 direction = transform.rotation * -Vector3.forward;
            Vector3 desiredPosition = targetPosition + direction * targetDistance;
            
            // Check for collisions
            float actualDistance = targetDistance;
            RaycastHit hit;
            if (Physics.SphereCast(targetPosition, collisionRadius, direction, out hit, targetDistance, collisionLayers))
            {
                actualDistance = Mathf.Max(hit.distance - collisionRadius, minDistance);
            }
            
            // Smooth distance interpolation
            currentDistance = Mathf.Lerp(currentDistance, actualDistance, Time.deltaTime * 10f);
            
            // Final camera position with smooth follow
            Vector3 finalPosition = targetPosition + direction * currentDistance;
            currentPosition = Vector3.SmoothDamp(currentPosition, finalPosition, ref positionVelocity, followSmoothTime);
            
            transform.position = currentPosition;
        }
        
        // Public API
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        public void SetDistance(float distance)
        {
            targetDistance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
        
        public float GetCurrentYaw()
        {
            return currentYaw;
        }
    }
}
