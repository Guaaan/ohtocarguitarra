using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DrumMachineManager : MonoBehaviour
{
    public GameObject stepButtonPrefab;
    public int numSteps = 16; // 2 compases de corcheas
    public AudioClip[] samples; // cada sample es una fila
    public float bpm = 120f;

    private StepButton[,] grid;
    private AudioSource[] audioSources;

    private int currentStep = 0;
    private Coroutine playRoutine;

    void Start()
    {
        GenerateGrid();
        SetupAudioSources();
    }

    void GenerateGrid()
    {
        grid = new StepButton[samples.Length, numSteps];

        for (int row = 0; row < samples.Length; row++)
        {
            for (int col = 0; col < numSteps; col++)
            {
                Vector3 pos = new Vector3(col * 1.2f, 0, row * 1.2f);
                GameObject stepGO = Instantiate(stepButtonPrefab, pos, Quaternion.identity, this.transform);
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
        float interval = 60f / bpm / 2f; // corcheas = negra/2

        while (true)
        {
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
}
