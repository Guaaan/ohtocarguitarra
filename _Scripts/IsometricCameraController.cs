using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float moveSpeed = 10f; // Velocidad de desplazamiento de la cámara
    public float smoothSpeed = 5f; // Velocidad de suavizado del movimiento
    public Vector2 minLimits = new Vector2(-50f, -50f); // Límites mínimos en el plano XZ
    public Vector2 maxLimits = new Vector2(50f, 50f);   // Límites máximos en el plano XZ

    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomSmoothSpeed = 5f;

    [Header("Pivot Settings")]
    public GameObject pivot; // Pivote de la cámara

    private Vector3 targetPosition; // Posición deseada del pivote
    private float targetZoom;       // Zoom deseado de la cámara
    private Camera cam;

    void Start()
    {
        if (pivot == null)
        {
            Debug.LogError("Pivot is not assigned.");
            enabled = false;
            return;
        }

        cam = GetComponent<Camera>();
        ValidateCamera();

        targetPosition = pivot.transform.position;
        targetZoom = cam.orthographicSize;

        // Establecer el ángulo isométrico inicial
        //transform.rotation = Quaternion.Euler(45f, 45f, 0f);
    }

    void Update()
    {
        //HandleMovement();
        HandleCamZoom();

        // Suavizar el movimiento del pivote
        pivot.transform.position = Vector3.Lerp(pivot.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void HandleCamMovement()
    {
        // Obtener la entrada del usuario (WASD)
        float horizontal = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float vertical = Input.GetAxis("Vertical");     // W/S o flechas arriba/abajo

        // Calcular el desplazamiento en el plano isométrico (XZ)
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Convertir a espacio isométrico (rotación 45°)
        Vector3 isometricDirection = Quaternion.Euler(0f, 45f, 0f) * direction;

        // Actualizar la posición deseada del pivote
        targetPosition += isometricDirection * moveSpeed * Time.deltaTime;

        // Limitar la posición dentro de los límites
        targetPosition.x = Mathf.Clamp(targetPosition.x, minLimits.x, maxLimits.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minLimits.y, maxLimits.y);
    }

    private void HandleCamZoom()
    {
        // Control del zoom con la rueda del mouse
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > Mathf.Epsilon)
        {
            targetZoom = Mathf.Clamp(targetZoom - scrollInput * zoomSpeed, minZoom, maxZoom);
        }

        // Suavizar el zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSmoothSpeed * Time.deltaTime);
    }

    private void ValidateCamera()
    {
        if (cam == null || !cam.orthographic)
        {
            Debug.LogError("This script requires an orthographic camera.");
            enabled = false;
        }
    }

    public void MovePivotTo(Vector3 newPosition)
    {
        // Mueve el pivote a una posición específica dentro de los límites
        targetPosition.x = Mathf.Clamp(newPosition.x, minLimits.x, maxLimits.x);
        targetPosition.z = Mathf.Clamp(newPosition.z, minLimits.y, maxLimits.y);
        targetPosition.y = pivot.transform.position.y; // Mantener la altura actual del pivote
    }
}
