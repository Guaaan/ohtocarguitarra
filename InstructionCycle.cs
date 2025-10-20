using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Instruccion
{
    public string titulo;
    public Sprite imagen;
    [TextArea] public string texto;
}

public class InstructionCycle : MonoBehaviour
{
    [Header("Instrucciones")]
    [SerializeField] private Instruccion[] instrucciones;

    [Header("UI Elements")]
    [SerializeField] private Text txtTitulo;
    [SerializeField] private Image imgInstruccion;
    [SerializeField] private TextMeshProUGUI txtCuerpo;
    [SerializeField] private Text txtBtnTerminal;

    [Header("Sonidos")]
    [SerializeField] private AudioSource typingSound;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource backSound;

    [Header("Configuración")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private string textoFinal = "¡Completado!";

    private int indiceActual = 0;
    private Coroutine typingCoroutine;
    private string textoCompleto;
    private bool escribiendo = false;

    void Start()
    {
        ActualizarUI();
    }

    public void Avanzar()
    {
        if (escribiendo)
        {
            TerminarEscrituraInstantanea();
            return;
        }

        if (indiceActual < instrucciones.Length - 1)
        {
            indiceActual++;
            clickSound?.Play();
            ActualizarUI();
        }
        else
        {
            txtBtnTerminal.text = textoFinal;
        }
    }

    public void Retroceder()
    {
        if (escribiendo)
        {
            TerminarEscrituraInstantanea();
            return;
        }

        if (indiceActual > 0)
        {
            indiceActual--;
            backSound?.Play();
            txtBtnTerminal.text = ""; // Limpiar si retrocedemos
            ActualizarUI();
        }
    }

    private void ActualizarUI()
    {
        if (instrucciones == null || instrucciones.Length == 0)
        {
            Debug.LogWarning("No hay instrucciones asignadas.");
            return;
        }

        Instruccion actual = instrucciones[indiceActual];

        if (txtTitulo != null)
            txtTitulo.text = actual.titulo;

        if (imgInstruccion != null)
            imgInstruccion.sprite = actual.imagen;

        // Iniciar efecto de máquina de escribir
        if (txtCuerpo != null)
            IniciarEscritura(actual.texto);
    }

    private void IniciarEscritura(string texto)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textoCompleto = texto;
        typingCoroutine = StartCoroutine(EscribirTexto(textoCompleto));
    }

    private void TerminarEscrituraInstantanea()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        txtCuerpo.text = textoCompleto;
        escribiendo = false;
    }

    private IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        txtCuerpo.text = "";

        foreach (char letra in texto)
        {
            txtCuerpo.text += letra;

            if (typingSound != null)
                typingSound.Play();

            yield return new WaitForSeconds(typingSpeed);
        }

        escribiendo = false;
    }
}
