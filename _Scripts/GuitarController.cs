using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importar TextMeshPro

public class GuitarController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip noteF;
    public AudioClip noteG;
    public AudioClip noteAb;
    public AudioClip noteBb;
    public AudioClip noteC;
    public AudioClip noteDb;
    public AudioClip noteEb;

    private AudioSource audioSource;

    [Header("Audio Control")]
    public Slider startSlider;
    private float startPercentage = 0f;

    public RagdollBalancer ragdollCtrl;

    [Header("Octave Control")]
    private int currentOctave = 0;
    private const int MIN_OCTAVE = -3;
    private const int MAX_OCTAVE = 3;

    [Header("UI Elements")]
    public TextMeshProUGUI octaveText; // Referencia al texto de la octava

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (startSlider != null)
        {
            startSlider.minValue = 0f;
            startSlider.maxValue = 1f;
            startSlider.value = 0f;
            startSlider.onValueChanged.AddListener(UpdateStartPercentage);
        }

        // Actualizar el texto al iniciar
        UpdateOctaveText();
    }

    void Update()
    {
        // Octave Control
        if (Input.GetKeyDown(KeyCode.Q))
            AdjustOctave(-1);
        else if (Input.GetKeyDown(KeyCode.E))
            AdjustOctave(1);

        // Reproducir sonidos
        if (Input.GetKeyDown(KeyCode.Z))
            PlaySound(noteF);
        else if (Input.GetKeyDown(KeyCode.X))
            PlaySound(noteG);
        else if (Input.GetKeyDown(KeyCode.C))
            PlaySound(noteAb);
        else if (Input.GetKeyDown(KeyCode.V))
            PlaySound(noteBb);
        else if (Input.GetKeyDown(KeyCode.B))
            PlaySound(noteC);
        else if (Input.GetKeyDown(KeyCode.N))
            PlaySound(noteDb);
        else if (Input.GetKeyDown(KeyCode.M))
            PlaySound(noteEb);
    }

    /// <summary>
    /// Reproduce un AudioClip aplicando la octava actual.
    /// </summary>
    /// <param name="clip">El AudioClip a reproducir.</param>
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;

            float maxStartTime = clip.length * 0.4f;
            float startTime = startPercentage * maxStartTime;
            startTime = Mathf.Clamp(startTime, 0, maxStartTime);

            audioSource.time = startTime;
            audioSource.pitch = Mathf.Pow(2f, currentOctave);
            audioSource.Play();

            ragdollCtrl.ApplySpasmForce();

            Debug.Log($"🎵 Reproduciendo '{clip.name}' en octava {currentOctave} (Pitch: {audioSource.pitch:F2})");
        }
        else
        {
            Debug.LogWarning("⚠️ Clip no asignado para esta tecla.");
        }
    }

    /// <summary>
    /// Ajusta la octava actual y actualiza el texto.
    /// </summary>
    /// <param name="change">Incremento o decremento de la octava.</param>
    private void AdjustOctave(int change)
    {
        currentOctave = Mathf.Clamp(currentOctave + change, MIN_OCTAVE, MAX_OCTAVE);
        Debug.Log($"🔄 Octava ajustada a: {currentOctave}");
        UpdateOctaveText();
    }
    /// <summary>
    /// Reproduce una nota específica según un número del 1 al 7.
    /// </summary>
    /// <param name="number">Número de nota (1 a 7)</param>
    public void PlayNoteByNumber(int number)
    {
        AudioClip selectedClip = null;

        switch (number)
        {
            case 1:
                selectedClip = noteF;
                break;
            case 2:
                selectedClip = noteG;
                break;
            case 3:
                selectedClip = noteAb;
                break;
            case 4:
                selectedClip = noteBb;
                break;
            case 5:
                selectedClip = noteC;
                break;
            case 6:
                selectedClip = noteDb;
                break;
            case 7:
                selectedClip = noteEb;
                break;
            default:
                Debug.LogWarning("⚠️ Número de nota fuera de rango (1-7).");
                return;
        }

        PlaySound(selectedClip);
    }

    /// <summary>
    /// Actualiza el texto de la octava actual.
    /// </summary>
    private void UpdateOctaveText()
    {
        if (octaveText != null)
        {
            octaveText.text = $"Octava: {currentOctave}";
        }
        else
        {
            Debug.LogWarning("⚠️ No se ha asignado un TextMeshProUGUI para la octava.");
        }
    }

    /// <summary>
    /// Actualiza el porcentaje de inicio del AudioClip.
    /// </summary>
    /// <param name="value">Nuevo valor del slider.</param>
    private void UpdateStartPercentage(float value)
    {
        startPercentage = value;
    }
}
