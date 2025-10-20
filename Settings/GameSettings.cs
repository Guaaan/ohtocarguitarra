using UnityEngine;

[CreateAssetMenu(menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
private int _defaultLevelIndex = 0;  // Índice del nivel predeterminado
public int defaultLevelIndex {
    get => _defaultLevelIndex;
    set { _defaultLevelIndex = value; Save(); }
}
private string _languageCode = "en"; // idioma por defecto
public string languageCode {
    get => _languageCode;
    set { _languageCode = value; Save(); }
}
private float _mouseSensitivity = 1.0f;
public float mouseSensitivity {
    get => _mouseSensitivity;
    set { _mouseSensitivity = value; Save(); }
}

    public void Save()
    {
        PlayerPrefs.SetInt("DefaultLevelIndex", defaultLevelIndex);
        PlayerPrefs.SetString("LanguageCode", languageCode);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
    }

    public void Load()
    {
        defaultLevelIndex = PlayerPrefs.GetInt("DefaultLevelIndex", defaultLevelIndex);
        languageCode = PlayerPrefs.GetString("LanguageCode", languageCode);
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
    }
}
