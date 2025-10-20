using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Configuración de Nota")]
    public GuitarController guitarController;
    public int noteNumber;

    [Header("Animación del Botón")]
    public float pressDepth = 0.1f;         // Cuánto se hunde el botón al presionar
    public float returnSpeed = 5f;          // Qué tan rápido regresa
    public float maxPressLimit = 0.2f;      // Máximo que se puede presionar

    [Header("Sonido")]
    public AudioClip clickSound;
    private AudioSource audioSource;

    private Vector3 initialPosition;
    private bool isMoving = false;

    private void Start()
    {
        initialPosition = transform.localPosition;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PressButton()
    {
        // Reproducir nota musical
        guitarController.PlayNoteByNumber(noteNumber);

        // Reproducir sonido de clic si está asignado
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Iniciar animación de movimiento si no se está moviendo
        if (!isMoving)
        {
            StartCoroutine(AnimatePress());
        }
    }

    private IEnumerator AnimatePress()
    {
        isMoving = true;

        // Calcular nueva posición limitada
        Vector3 targetPos = transform.localPosition - new Vector3(0, pressDepth, 0);
        float maxY = initialPosition.y - maxPressLimit;
        targetPos.y = Mathf.Max(targetPos.y, maxY);
        isMoving = false;

        // Mover hacia abajo
        transform.localPosition = targetPos;

        // Esperar un pequeño momento antes de regresar
        yield return new WaitForSeconds(0.05f);

        // Regresar suavemente a la posición original
        while (Vector3.Distance(transform.localPosition, initialPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.localPosition = initialPosition;
        isMoving = false;
    }
}
