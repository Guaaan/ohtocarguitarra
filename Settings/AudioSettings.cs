using UnityEngine;

[CreateAssetMenu(menuName = "Settings/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambienceVolume = 1f;
    [Range(0f, 1f)] public float effectsVolume = 1f;

    public void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("AmbienceVolume", ambienceVolume);
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
    }

    public void Load()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", ambienceVolume);
        effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", effectsVolume);
    }
}
