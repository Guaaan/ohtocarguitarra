using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("Canales de Audio")]
    [SerializeField] private string masterChannel = "MasterVolume";
    [SerializeField] private string instrumentChannel = "InstrumentVolume";
    [SerializeField] private string drumsChannel = "DrumsVolume";
    [SerializeField] private string sfxChannel = "SFXVolume";

    [Header("Sliders UI")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider instrumentSlider;
    [SerializeField] private Slider drumsSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        InitializeSlider(masterSlider, masterChannel);
        InitializeSlider(instrumentSlider, instrumentChannel);
        InitializeSlider(drumsSlider, drumsChannel);
        InitializeSlider(sfxSlider, sfxChannel);
    }

    private void InitializeSlider(Slider slider, string channel)
    {
        if (slider != null)
        {
            // Obtener valor actual del mixer y convertir a valor lineal
            if (audioMixer.GetFloat(channel, out float dbVolume))
            {
                float linearValue = ConvertDBToLinear(dbVolume);
                slider.value = linearValue;
            }

            // Configurar listener para cambios en el slider
            slider.onValueChanged.AddListener((value) => SetChannelVolume(channel, value));
        }
    }

    public void SetMasterVolume(float volume) => SetChannelVolume(masterChannel, volume);
    public void SetInstrumentVolume(float volume) => SetChannelVolume(instrumentChannel, volume);
    public void SetDrumsVolume(float volume) => SetChannelVolume(drumsChannel, volume);
    public void SetSFXVolume(float volume) => SetChannelVolume(sfxChannel, volume);

    private void SetChannelVolume(string channel, float linearVolume)
    {
        float dbVolume = ConvertLinearToDB(linearVolume);
        audioMixer.SetFloat(channel, dbVolume);
    }

    private float ConvertLinearToDB(float linear)
    {
        // Convertir valor lineal (0-1) a decibelios (-80dB a 0dB)
        return linear <= 0.0001f ? -80f : Mathf.Log10(linear) * 20f;
    }

    private float ConvertDBToLinear(float db)
    {
        // Convertir decibelios a valor lineal (0-1)
        return Mathf.Pow(10f, db / 20f);
    }
}