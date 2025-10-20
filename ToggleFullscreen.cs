using TMPro;
using UnityEngine;

public class ToggleFullscreen : MonoBehaviour
{
    public TMP_Text buttonText;

    public void Toggle()
    {
        if (Screen.fullScreen)
        {
            // Pasa a modo ventana con una resoluci�n (ej: 1280x720)
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else
        {
            // Pasa a pantalla completa (exclusive/fullscreen window seg�n necesidad)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        UpdateButtonText();
    }

    void Start()
    {
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = Screen.fullScreen ? "Modo Ventana" : "Pantalla Completa";
        }
    }
}
