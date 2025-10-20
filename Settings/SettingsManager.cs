using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public GameSettings gameSettings;

    public AudioManager audioManager;
    public GraphicsSettings graphicsSettings;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    public void SaveSettings()
    {
        audioManager.SaveVolumes();
        graphicsSettings.Save();
        gameSettings.Save();
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        audioManager.LoadVolumes();
        graphicsSettings.Load();
        gameSettings.Load();
    }

}
