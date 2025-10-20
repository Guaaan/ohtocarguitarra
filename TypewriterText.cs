using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;  // Referencia al componente TextMeshProUGUI
    public string[] texts;  // Arreglo de textos
    public float typingSpeed = 0.05f;  // Velocidad de escritura (en segundos entre cada letra)
    public AudioSource typingSound;  // Sonido que se reproducirá al escribir cada letra

    private int currentTextIndex = 0;  // Índice del texto actual en el arreglo
    private Coroutine typingCoroutine; // Referencia a la corrutina de escritura
    private bool isTyping = false;  // Para saber si está escribiendo una frase
    private string fullText;  // El texto completo que se está escribiendo actualmente

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI is not assigned.");
            return;
        }

        // Iniciar la escritura del primer texto
        StartTyping(texts[currentTextIndex]);
    }

    void Update()
    {
        // Si se hace clic izquierdo y la frase se está escribiendo, termina de escribirla inmediatamente
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Completar la escritura instantáneamente
                StopCoroutine(typingCoroutine);
                textMeshPro.text = fullText;  // Mostrar el texto completo
                isTyping = false;
            }
            else
            {
                // Avanzar al siguiente texto
                currentTextIndex++;
                if (currentTextIndex >= texts.Length) currentTextIndex = texts.Length - 1;  // Limitar al último texto
                StartTyping(texts[currentTextIndex]);
            }
        }

        // Si se hace clic derecho (retroceder al texto anterior)
        if (Input.GetMouseButtonDown(1))
        {
            if (isTyping)
            {
                // Completar la escritura instantáneamente
                StopCoroutine(typingCoroutine);
                textMeshPro.text = fullText;  // Mostrar el texto completo
                isTyping = false;
            }
            else
            {
                // Retroceder al texto anterior
                currentTextIndex--;
                if (currentTextIndex < 0) currentTextIndex = 0;  // Limitar al primer texto
                StartTyping(texts[currentTextIndex]);
            }
        }
    }

    // Iniciar la corrutina para escribir el texto letra por letra
    void StartTyping(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        fullText = text;  // Guardar el texto completo
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    // Corrutina que escribe el texto letra por letra
    IEnumerator TypeText(string text)
    {
        textMeshPro.text = "";  // Limpiar el texto actual
        isTyping = true;  // Indicar que se está escribiendo

        foreach (char letter in text.ToCharArray())
        {
            textMeshPro.text += letter;  // Agregar la siguiente letra

            // Reproducir el sonido al escribir cada letra, si el AudioSource está asignado
            if (typingSound != null)
            {
                typingSound.Play();
            }

            // Esperar antes de escribir la siguiente letra, a menos que el usuario haga clic
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;  // La frase ha terminado de escribirse
    }
}
