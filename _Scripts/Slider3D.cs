using UnityEngine;

public class Slider3DHandle : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float value; // 0 a 1
    public bool isBeingDragged = false;

    private SphereCursor cursor;

    void Start()
    {
        // Busca el cursor en escena
        cursor = FindObjectOfType<SphereCursor>();
    }

    void Update()
    {
        if (cursor == null) return;

        if (cursor.currentBtn == gameObject && Input.GetMouseButton(0))
        {
            Debug.Log("Arrastrando el handle...");
            isBeingDragged = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isBeingDragged = false;
        }

        if (isBeingDragged)
        {
            DragHandle(cursor.transform.position);
        }
    }

    void DragHandle(Vector3 cursorPosition)
    {
        Debug.Log("Cursor Pos: " + cursorPosition);
        // Calcular la dirección del slider
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float length = Vector3.Distance(startPoint.position, endPoint.position);

        // Proyectar la posición del cursor sobre la línea del slider
        Vector3 toCursor = cursorPosition - startPoint.position;
        float dot = Vector3.Dot(toCursor, direction);
        dot = Mathf.Clamp(dot, 0, length);

        // Mover el handle
        transform.position = startPoint.position + direction * dot;

        // Actualizar el valor del slider (0 a 1)
        value = dot / length;
    }

    public float GetSliderValue01()
    {
        return value;
    }
}
