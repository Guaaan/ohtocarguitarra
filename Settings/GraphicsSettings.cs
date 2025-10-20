using UnityEngine;

[CreateAssetMenu(menuName = "Settings/GraphicsSettings")]
public class GraphicsSettings : ScriptableObject
{
private int _resolutionIndex = 0;
public int resolutionIndex {
    get => _resolutionIndex;
    set { _resolutionIndex = value; Save(); }
}
private bool _isFullscreen = true;
public bool isFullscreen {
    get => _isFullscreen;
    set { _isFullscreen = value; Save(); }
}
private float _uiScale = 1f;
public float uiScale {
    get => _uiScale;
    set { _uiScale = value; Save(); }
}

    Resolution[] resolutions;

    public void Apply()
    {
        resolutions = Screen.resolutions;

        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
            resolutionIndex = resolutions.Length - 1;

        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, isFullscreen);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.SetFloat("UIScale", uiScale);
    }

    public void Load()
    {
        resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutionIndex);
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", isFullscreen ? 1 : 0) == 1;
        uiScale = PlayerPrefs.GetFloat("UIScale", uiScale);
        Apply();
    }
}
