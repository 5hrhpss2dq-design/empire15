using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2.2f, -4.5f);
    public float followSmoothTime = 0.08f;
    public float rotationSpeed = 120f;
    public float zoomSpeed = 2f;
    public float minDistance = 1.5f;
    public float maxDistance = 6f;

    Vector3 currentVelocity;
    float currentDistance;
    float targetDistance;

    void Start()
    {
        currentDistance = targetDistance = -offset.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Zoom
        float scroll = -Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            targetDistance = Mathf.Clamp(targetDistance + scroll * zoomSpeed, minDistance, maxDistance);
        }

        // Right-mouse to orbit horizontally
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }

        // Desired position based on target's forward
        Vector3 desiredOffset = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(0, offset.y, -targetDistance);
        Vector3 desiredPosition = target.position + desiredOffset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, followSmoothTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}