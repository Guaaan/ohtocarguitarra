using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para usar Slider

public class GuitarController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip noteF; // Fa
    public AudioClip noteG; // Sol
    public AudioClip noteAb; // Lab
    public AudioClip noteBb; // Sib
    public AudioClip noteC; // Do
    public AudioClip noteDb; // Reb
    public AudioClip noteEb; // Mib

    private AudioSource audioSource;

    [Header("Audio Control")]
    public Slider startSlider; // Slider para modificar el inicio del audio
    private float startPercentage = 0f; // Representa el porcentaje del inicio (0.0 a 1.0)

    public RagdollBalancer ragdollCtrl;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (startSlider != null)
        {
            startSlider.minValue = 0f;
            startSlider.maxValue = 1f;
            startSlider.value = 0f; // Iniciar desde el principio del audio
            startSlider.onValueChanged.AddListener(UpdateStartPercentage);
        }
    }

    void Update()
    {
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

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            // Asegurar que el AudioSource se detiene antes de asignar un nuevo clip
            audioSource.Stop();
            audioSource.clip = clip;

            // 🔹 Limitar el inicio máximo al 80% del clip
            float maxStartTime = clip.length * 0.4f;
            float startTime = startPercentage * maxStartTime;

            // Asegurar que startTime esté dentro de los límites
            startTime = Mathf.Clamp(startTime, 0, maxStartTime);

            // Asignar el tiempo y reproducir
            audioSource.time = startTime;
            audioSource.Play();
            ragdollCtrl.ApplySpasmForce();

            // 📌 Imprimir información en consola
            float percentagePlayed = (startTime / clip.length) * 100f;
            Debug.Log($"🎵 Reproduciendo '{clip.name}' desde {startTime:F2} segundos ({percentagePlayed:F1}% del clip). Duración total: {clip.length:F2} segundos.");
        }
        else
        {
            Debug.LogWarning("⚠️ Clip no asignado para esta tecla.");
        }
    }

    private void UpdateStartPercentage(float value)
    {
        startPercentage = value;
    }
}
