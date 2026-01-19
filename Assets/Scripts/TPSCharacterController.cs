using UnityEngine;

namespace Empire15.Movement
{
    /// <summary>
    /// PUBG-like smooth third-person character controller
    /// Handles movement, rotation, and player input
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class TPSCharacterController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 2.5f;
        [SerializeField] private float sprintSpeed = 5.5f;
        [SerializeField] private float crouchSpeed = 1.5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 15f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -19.62f;
        
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSmoothTime = 0.1f;
        
        // Components
        private CharacterController controller;
        private Transform cameraTransform;
        
        // Movement state
        private Vector3 velocity;
        private Vector3 currentMovement;
        private float currentSpeed;
        private float targetSpeed;
        private bool isGrounded;
        private float rotationVelocity;
        
        // Input
        private Vector2 moveInput;
        private bool sprintInput;
        private bool crouchInput;
        private bool jumpInput;
        
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            cameraTransform = Camera.main?.transform;
        }
        
        private void Update()
        {
            HandleInput();
            HandleMovement();
            HandleRotation();
            HandleGravity();
        }
        
        private void HandleInput()
        {
            // Get movement input
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;
            
            // Get action inputs
            sprintInput = Input.GetKey(KeyCode.LeftShift);
            crouchInput = Input.GetKey(KeyCode.LeftControl);
            jumpInput = Input.GetButtonDown("Jump");
        }
        
        private void HandleMovement()
        {
            // Check if grounded
            isGrounded = controller.isGrounded;
            
            // Determine target speed based on state
            if (crouchInput)
                targetSpeed = crouchSpeed;
            else if (sprintInput && moveInput.y > 0)
                targetSpeed = sprintSpeed;
            else
                targetSpeed = walkSpeed;
            
            // If no input, target speed is zero
            if (moveInput.magnitude == 0)
                targetSpeed = 0;
            
            // Smooth acceleration/deceleration
            float speedChange = (targetSpeed > currentSpeed) ? acceleration : deceleration;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChange * Time.deltaTime);
            
            // Calculate movement direction relative to camera
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            
            // Flatten directions (no vertical component)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            // Calculate target movement direction
            Vector3 targetDirection = (forward * moveInput.y + right * moveInput.x).normalized;
            currentMovement = targetDirection * currentSpeed;
            
            // Apply movement
            controller.Move(currentMovement * Time.deltaTime);
            
            // Handle jump
            if (jumpInput && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        
        private void HandleRotation()
        {
            // Only rotate when moving
            if (moveInput.magnitude > 0.1f)
            {
                // Calculate target rotation based on movement direction
                Vector3 forward = cameraTransform.forward;
                Vector3 right = cameraTransform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                
                Vector3 targetDirection = (forward * moveInput.y + right * moveInput.x).normalized;
                
                if (targetDirection.magnitude > 0.1f)
                {
                    float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
                    float smoothAngle = Mathf.SmoothDampAngle(
                        transform.eulerAngles.y, 
                        targetAngle, 
                        ref rotationVelocity, 
                        rotationSmoothTime
                    );
                    
                    transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
                }
            }
        }
        
        private void HandleGravity()
        {
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Small downward force to keep grounded
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
            
            controller.Move(velocity * Time.deltaTime);
        }
        
        // Public getters for other systems
        public bool IsMoving => currentSpeed > 0.1f;
        public bool IsSprinting => sprintInput && currentSpeed > walkSpeed;
        public bool IsCrouching => crouchInput;
        public Vector3 Velocity => currentMovement;
    }
}
