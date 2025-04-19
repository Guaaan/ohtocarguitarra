    using UnityEngine;

public class CameraMouseMovement : MonoBehaviour
{
    [Header("Camera Settings")]
    public float moveToCursorSpeed = 10f; // Velocidad de desplazamiento de la cámara
    public float smoothCursorSpeed = 5f; // Velocidad de suavizado del movimiento
    public Vector2 minMoveLimits = new Vector2(-50f, -50f); // Límites mínimos en el plano XZ
    public Vector2 maxMoveLimits = new Vector2(50f, 50f);   // Límites máximos en el plano XZ

    [Header("Zoom Settings")]
    public float zoomToCursorSpeed = 2f;
    public float minCameraZoom = 5f;
    public float maxCameraZoom = 20f;
    public float zoomSmooth = 5f;

    private Vector3 targetPosition; // Posición deseada de la cámara
    private float targetZoom;       // Zoom deseado de la cámara
    private Camera cursorCam;

    void Start()
    {
        cursorCam = GetComponent<Camera>();
        ValidateIsometricCamera();

        targetPosition = transform.position;
        targetZoom = cursorCam.orthographicSize;

        // Establecer el ángulo isométrico inicial
        //transform.rotation = Quaternion.Euler(45f, 45f, 0f);
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        // Obtener la entrada del usuario (WASD)
        float horizontal = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float vertical = Input.GetAxis("Vertical");     // W/S o flechas arriba/abajo

        // Calcular el desplazamiento en el plano isométrico (XZ)
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Convertir a espacio isométrico (rotación 45°)
        Vector3 isometricDirection = Quaternion.Euler(0f, 45f, 0f) * direction;

        // Actualizar la posición deseada
        targetPosition += isometricDirection * moveToCursorSpeed * Time.deltaTime;

        // Limitar la posición dentro de los límites
        targetPosition.x = Mathf.Clamp(targetPosition.x, minMoveLimits.x, maxMoveLimits.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minMoveLimits.y, maxMoveLimits.y);

        // Suavizar el movimiento de la cámara
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothCursorSpeed * Time.deltaTime);
    }

    private void HandleZoom()
    {
        // Control del zoom con la rueda del mouse
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > Mathf.Epsilon)
        {
            targetZoom = Mathf.Clamp(targetZoom - scrollInput * zoomToCursorSpeed, minCameraZoom, maxCameraZoom);
        }

        // Suavizar el zoom
        cursorCam.orthographicSize = Mathf.Lerp(cursorCam.orthographicSize, targetZoom, zoomSmooth * Time.deltaTime);
    }

    private void ValidateIsometricCamera()
    {
        if (cursorCam == null || !cursorCam.orthographic)
        {
            Debug.LogError("This script requires an orthographic camera.");
            enabled = false;
        }
    }
}