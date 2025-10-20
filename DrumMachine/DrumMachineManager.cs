using System.Collections;
using UnityEngine;
using UnityEngine.UI; // para el Slider
using TMPro;          // para TextMeshProUGUI

public class DrumMachineManager : MonoBehaviour
{
    public GameObject stepButtonPrefab;
    public int numSteps = 16; // 2 compases de corcheas
    public AudioClip[] samples; // cada sample es una fila

    [Header("Playback")]
    public float bpm = 120f;
    public float bpmMin = 40f;
    public float bpmMax = 240f;

    [Header("UI")]
    public Slider bpmSlider;            // asignar en el inspector
    public TextMeshProUGUI bpmText;     // asignar en el inspector

    private StepButton[,] grid;
    private AudioSource[] audioSources;

    private int currentStep = 0;
    private Coroutine playRoutine;

    [Header("Layout Settings")]
    public Vector3 stepButtonSize = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector2 stepSpacing = new Vector2(0.02f, 0.02f);

    void Start()
    {
        GenerateGrid();
        SetupAudioSources();

        // Configurar slider si está asignado
        if (bpmSlider != null)
        {
            bpmSlider.minValue = bpmMin;
            bpmSlider.maxValue = bpmMax;
            bpmSlider.wholeNumbers = true;
            bpmSlider.value = Mathf.Clamp(bpm, bpmMin, bpmMax);
            bpmSlider.onValueChanged.AddListener(SetBPM);
        }

        UpdateBPMText();
    }

    void OnDestroy()
    {
        if (bpmSlider != null)
            bpmSlider.onValueChanged.RemoveListener(SetBPM);
    }

    void GenerateGrid()
    {
        grid = new StepButton[samples.Length, numSteps];

        for (int row = 0; row < samples.Length; row++)
        {
            for (int col = 0; col < numSteps; col++)
            {
                Vector3 localPos = new Vector3(
                    col * (stepButtonSize.x + stepSpacing.x),
                    0,
                    row * (stepButtonSize.z + stepSpacing.y)
                );

                GameObject stepGO = Instantiate(stepButtonPrefab, transform);
                stepGO.transform.localPosition = localPos;
                stepGO.transform.localScale = stepButtonSize;

                StepButton step = stepGO.GetComponent<StepButton>();
                grid[row, col] = step;

                int r = row, c = col;
                stepGO.AddComponent<BoxCollider>();
                stepGO.AddComponent<StepClickHandler>().Init(() => ToggleStep(r, c));
            }
        }
    }

    void ToggleStep(int row, int col)
    {
        grid[row, col].Toggle();
    }

    void SetupAudioSources()
    {
        audioSources = new AudioSource[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            GameObject audioGO = new GameObject("AudioSource_" + i);
            audioGO.transform.parent = transform;
            AudioSource src = audioGO.AddComponent<AudioSource>();
            src.clip = samples[i];
            audioSources[i] = src;
        }
    }

    public void Play()
    {
        if (playRoutine == null)
            playRoutine = StartCoroutine(PlaySequence());
    }

    public void Stop()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
            ClearHighlights();
            currentStep = 0;
        }
    }

    IEnumerator PlaySequence()
    {
        while (true)
        {
            float interval = 60f / Mathf.Max(1f, bpm) / 2f; // recalcular cada ciclo

            for (int i = 0; i < samples.Length; i++)
            {
                if (grid[i, currentStep].isActive)
                    audioSources[i].PlayOneShot(samples[i]);

                grid[i, currentStep].Highlight();
            }

            yield return new WaitForSeconds(interval);

            for (int i = 0; i < samples.Length; i++)
            {
                grid[i, currentStep].Unhighlight();
            }

            currentStep = (currentStep + 1) % numSteps;
        }
    }

    void ClearHighlights()
    {
        foreach (var step in grid)
            step.Unhighlight();
    }

    public void SetBPM(float newBpm)
    {
        bpm = Mathf.Clamp(newBpm, bpmMin, bpmMax);
        UpdateBPMText();
    }

    void UpdateBPMText()
    {
        if (bpmText != null)
            bpmText.text = $"BPM: {Mathf.RoundToInt(bpm)}";
    }
}
