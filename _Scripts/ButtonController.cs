using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Configuraci�n de Nota")]
    public GuitarController guitarController;
    public int noteNumber;

    [Header("Animaci�n del Bot�n")]
    public float pressDepth = 0.1f;         // Cu�nto se hunde el bot�n al presionar
    public float returnSpeed = 5f;          // Qu� tan r�pido regresa
    public float maxPressLimit = 0.2f;      // M�ximo que se puede presionar

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

        // Reproducir sonido de clic si est� asignado
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Iniciar animaci�n de movimiento si no se est� moviendo
        if (!isMoving)
        {
            StartCoroutine(AnimatePress());
        }
    }

    private IEnumerator AnimatePress()
    {
        isMoving = true;

        // Calcular nueva posici�n limitada
        Vector3 targetPos = transform.localPosition - new Vector3(0, pressDepth, 0);
        float maxY = initialPosition.y - maxPressLimit;
        targetPos.y = Mathf.Max(targetPos.y, maxY);
        isMoving = false;

        // Mover hacia abajo
        transform.localPosition = targetPos;

        // Esperar un peque�o momento antes de regresar
        yield return new WaitForSeconds(0.05f);

        // Regresar suavemente a la posici�n original
        while (Vector3.Distance(transform.localPosition, initialPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.localPosition = initialPosition;
        isMoving = false;
    }
}
