using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambienceSlider;
    public Slider effectsSlider;

    [Header("Graphics Settings")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider uiScaleSlider;

    [Header("Game Settings")]
    public TMP_Dropdown languageDropdown;
    public Slider mouseSensitivitySlider;
    public TMP_Dropdown defaultLevelDropdown;

    [Header("Buttons")]
    public Button applyButton;
    public Button backToMenuButton;
    public Button quitButton;

    [Header("Feedback")]
    public GameObject savedMessage; // Un Text/TMP que diga: "Settings Saved!"

    Resolution[] availableResolutions;

    void Start()
    {
        savedMessage?.SetActive(false);

        var audioManager = SettingsManager.Instance.audioManager;
        var graphics = SettingsManager.Instance.graphicsSettings;
        var game = SettingsManager.Instance.gameSettings;

        // ====== AUDIO ======
        // Inicializar sliders con valores actuales de PlayerPrefs o 1f por defecto
        musicSlider.value = PlayerPrefs.GetFloat("InstrumentVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 1f);
        effectsSlider.value = PlayerPrefs.GetFloat("DrumMachineVolume", 1f);

        // Listeners: actualizan el volumen y guardan el valor usando AudioManager
        musicSlider.onValueChanged.AddListener(v => {
            audioManager.SetChannelVolume("InstrumentVolume", v);
            PlayerPrefs.SetFloat("InstrumentVolume", v);
            PlayerPrefs.Save();
        });
        sfxSlider.onValueChanged.AddListener(v => {
            audioManager.SetChannelVolume("SFXVolume", v);
            PlayerPrefs.SetFloat("SFXVolume", v);
            PlayerPrefs.Save();
        });
        ambienceSlider.onValueChanged.AddListener(v => {
            audioManager.SetChannelVolume("AmbienceVolume", v);
            PlayerPrefs.SetFloat("AmbienceVolume", v);
            PlayerPrefs.Save();
        });
        effectsSlider.onValueChanged.AddListener(v => {
            audioManager.SetChannelVolume("DrumMachineVolume", v);
            PlayerPrefs.SetFloat("DrumMachineVolume", v);
            PlayerPrefs.Save();
        });

        // ====== GRAPHICS ======
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();
        foreach (var res in availableResolutions)
            options.Add($"{res.width}x{res.height}");
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = graphics.resolutionIndex;
        fullscreenToggle.isOn = graphics.isFullscreen;
        uiScaleSlider.value = graphics.uiScale;

        resolutionDropdown.onValueChanged.AddListener(i => graphics.resolutionIndex = i);
        fullscreenToggle.onValueChanged.AddListener(b => graphics.isFullscreen = b);
        uiScaleSlider.onValueChanged.AddListener(v => graphics.uiScale = v);

        // ====== GAME ======
        languageDropdown.value = LanguageToIndex(game.languageCode);
        mouseSensitivitySlider.value = game.mouseSensitivity;

        // Poblar niveles reales desde build settings
        defaultLevelDropdown.ClearOptions();
        var sceneOptions = new System.Collections.Generic.List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            sceneOptions.Add(sceneName);
        }
        defaultLevelDropdown.AddOptions(sceneOptions);
        defaultLevelDropdown.value = game.defaultLevelIndex;

        languageDropdown.onValueChanged.AddListener(i => game.languageCode = IndexToLanguage(i));
        mouseSensitivitySlider.onValueChanged.AddListener(v => game.mouseSensitivity = v);
        defaultLevelDropdown.onValueChanged.AddListener(i => game.defaultLevelIndex = i);

        // ====== BOTONES ======
        applyButton.onClick.AddListener(OnApply);
        backToMenuButton.onClick.AddListener(OnBackToMenu);
        quitButton.onClick.AddListener(OnQuit);
    }

    void OnApply()
    {
        SettingsManager.Instance.graphicsSettings.Apply();
        SettingsManager.Instance.SaveSettings();

        if (savedMessage != null)
            StartCoroutine(ShowSavedMessage());
    }

    IEnumerator ShowSavedMessage()
    {
        savedMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        savedMessage.SetActive(false);
    }

    void OnBackToMenu()
    {
        SceneLoader.LoadMenu();
    }

    void OnQuit()
    {
        SceneLoader.QuitGame();
    }

    int LanguageToIndex(string code)
    {
        switch (code)
        {
            case "en": return 0;
            case "es": return 1;
            case "fr": return 2;
            default: return 0;
        }
    }

    string IndexToLanguage(int index)
    {
        switch (index)
        {
            case 0: return "en";
            case 1: return "es";
            case 2: return "fr";
            default: return "en";
        }
    }
}
