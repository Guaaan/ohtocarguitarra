using UnityEngine;

public class FloatingWater : MonoBehaviour
{
    [Header("Movimiento de Rotación")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Movimiento de Levitación")]
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatFrequency = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotación
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Movimiento de Levitación
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
