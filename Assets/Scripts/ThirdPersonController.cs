using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runMultiplier = 1.8f;
    [Range(0.01f, 0.5f)] public float moveSmoothTime = 0.08f;
    [Range(0.01f, 0.5f)] public float rotationSmoothTime = 0.08f;

    [Header("Jump/Gravity")]
    public float gravity = -20f;
    public float jumpSpeed = 7.5f;

    [Header("References")]
    public Transform cameraTransform;

    CharacterController cc;
    Vector3 currentVelocity;
    float currentSpeed;
    float speedVelocityRef;
    float rotationVelocityRef;
    float verticalVelocity;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 rawInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 input = rawInput.magnitude > 0.01f ? rawInput.normalized : Vector2.zero;

        // Camera relative movement
        Vector3 camForward = cameraTransform ? Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized : Vector3.forward;
        Vector3 camRight = cameraTransform ? cameraTransform.right : Vector3.right;
        Vector3 desiredMove = (camForward * input.y + camRight * input.x).normalized;

        // Smooth speed
        float targetSpeed = walkSpeed * (Input.GetKey(KeyCode.LeftShift) ? runMultiplier : 1f) * desiredMove.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocityRef, moveSmoothTime);

        // Apply horizontal movement
        Vector3 horizontalVel = desiredMove * currentSpeed;
        currentVelocity.x = horizontalVel.x;
        currentVelocity.z = horizontalVel.z;

        // Smooth rotation towards movement direction
        if (desiredMove.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(desiredMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-rotationSmoothTime * 60f * Time.deltaTime));
        }

        // Gravity & Jump
        if (cc.isGrounded)
        {
            if (verticalVelocity < 0) verticalVelocity = -2f;
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }
        verticalVelocity += gravity * Time.deltaTime;
        currentVelocity.y = verticalVelocity;

        // Move
        cc.Move(currentVelocity * Time.deltaTime);
    }
}