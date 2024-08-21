using UnityEngine;

public class CameraRTSController : MonoBehaviour
{
    public float panSpeed = 25f;             // Base speed for panning
    public float panAcceleration = 50f;      // Acceleration for panning
    public float panDamping = 5f;            // Damping for panning (smooth stop)
    public Vector2 panLimit = new Vector2(100, 100);                 // Limit of panning on x and z axes
    public float scrollSpeed = 40f;          // Speed of camera zooming
    public float zoomDamping = 10f;          // Damping for zooming (smooth stop)
    public float minY = 1f;                 // Minimum zoom level
    public float maxY = 100f;                // Maximum zoom level
    public float rotationSpeed = 300f;       // Speed of camera rotation
    public float rotationDamping = 10f;      // Damping for rotation (smooth stop)
    public float minXRotation = 10f;         // Minimum vertical rotation angle
    public float maxXRotation = 80f;         // Maximum vertical rotation angle

    private Vector3 panVelocity;             // Current velocity of panning
    private float targetZoom;                // Target zoom level
    private float targetRotationY;           // Target horizontal rotation angle
    private float targetRotationX;           // Target vertical rotation angle

    private void Start()
    {
        targetZoom = transform.position.y;
        targetRotationY = transform.eulerAngles.y;
        targetRotationX = transform.eulerAngles.x;
    }

    private void Update()
    {
        HandlePanning();
        HandleZooming();
        HandleRotation();
    }

    private void HandlePanning()
    {
        Vector3 panDirection = Vector3.zero;

        if (Input.GetKey("w"))// || Input.mousePosition.y >= Screen.height - 10f)
        {
            panDirection += transform.forward; // Move forward in the camera's local space
        }
        if (Input.GetKey("s"))// || Input.mousePosition.y <= 10f)
        {
            panDirection -= transform.forward; // Move backward in the camera's local space
        }
        if (Input.GetKey("d"))// || Input.mousePosition.x >= Screen.width - 10f)
        {
            panDirection += transform.right; // Move right in the camera's local space
        }
        if (Input.GetKey("a"))// || Input.mousePosition.x <= 10f)
        {
            panDirection -= transform.right; // Move left in the camera's local space
        }

        panDirection.y = 0f; // Flatten the direction to prevent unwanted vertical movement
        panDirection.Normalize();  // Keep consistent speed when moving diagonally

        // Acceleration
        panVelocity += panDirection * panAcceleration * Time.deltaTime;
        // Damping
        panVelocity = Vector3.Lerp(panVelocity, Vector3.zero, panDamping * Time.deltaTime);

        Vector3 pos = transform.position + panVelocity * panSpeed * Time.deltaTime;

        // Clamping the position
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }

    private void HandleZooming()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scroll * scrollSpeed * 100f * Time.deltaTime;
        targetZoom = Mathf.Clamp(targetZoom, minY, maxY);

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, targetZoom, zoomDamping * Time.deltaTime);
        transform.position = pos;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            // Horizontal rotation (Y-axis)
            targetRotationY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            // Vertical rotation (X-axis)
            targetRotationX -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            targetRotationX = Mathf.Clamp(targetRotationX, minXRotation, maxXRotation); // Clamp the vertical rotation
        }

        Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotationX, targetRotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationQuaternion, rotationDamping * Time.deltaTime);
    }
}
