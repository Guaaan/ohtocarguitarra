using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Canales de Audio")]
    [SerializeField] private string masterChannel = "MasterVolume";
    [SerializeField] private string instrumentChannel = "InstrumentVolume";
    [SerializeField] private string sfxChannel = "SFXVolume";
    [SerializeField] private string ambienceChannel = "AmbienceVolume";
    [SerializeField] private string DrumMachineChannel = "DrumMachineVolume";

    [Header("Sliders UI")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider instrumentSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider DrumMachineSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadVolumes();
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        InitSlider(masterSlider, masterChannel, "MasterVolume");
        InitSlider(instrumentSlider, instrumentChannel, "InstrumentVolume");
        InitSlider(sfxSlider, sfxChannel, "SFXVolume");
        InitSlider(ambienceSlider, ambienceChannel, "AmbienceVolume");
        InitSlider(DrumMachineSlider, DrumMachineChannel, "DrumMachineVolume");
    }

    private void InitSlider(Slider slider, string channel, string prefKey)
    {
        if (slider != null)
        {
            float value = PlayerPrefs.GetFloat(prefKey, 1f);
            slider.value = value;
            SetChannelVolume(channel, value);
            slider.onValueChanged.AddListener((v) => {
                SetChannelVolume(channel, v);
                PlayerPrefs.SetFloat(prefKey, v);
                PlayerPrefs.Save();
                SaveVolumes(); // Asegura que todos los valores se guarden tras cualquier cambio
            });
        }
    }

    public void SetChannelVolume(string channel, float linearVolume)
    {
        float dbVolume = ConvertLinearToDB(linearVolume);
        audioMixer.SetFloat(channel, dbVolume);
    }

    private float ConvertLinearToDB(float linear)
    {
        return linear <= 0.0001f ? -80f : Mathf.Log10(linear) * 20f;
    }

    private float ConvertDBToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }

    public void SaveVolumes()
    {
        if (masterSlider != null) PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        if (instrumentSlider != null) PlayerPrefs.SetFloat("InstrumentVolume", instrumentSlider.value);
        if (sfxSlider != null) PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        if (ambienceSlider != null) PlayerPrefs.SetFloat("AmbienceVolume", ambienceSlider.value);
        if (DrumMachineSlider != null) PlayerPrefs.SetFloat("DrumMachineVolume", DrumMachineSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadVolumes()
    {
        SetChannelVolume(masterChannel, PlayerPrefs.GetFloat("MasterVolume", 1f));
        SetChannelVolume(instrumentChannel, PlayerPrefs.GetFloat("InstrumentVolume", 1f));
        SetChannelVolume(sfxChannel, PlayerPrefs.GetFloat("SFXVolume", 1f));
        SetChannelVolume(ambienceChannel, PlayerPrefs.GetFloat("AmbienceVolume", 1f));
        SetChannelVolume(DrumMachineChannel, PlayerPrefs.GetFloat("DrumMachineVolume", 1f));
    }
}
