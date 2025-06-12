using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3D : MonoBehaviour
{
    public enum ButtonType { Play, Stop }
    public ButtonType buttonType;

    public DrumMachineManager drumMachine;

    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    void OnMouseDown()
    {
        transform.position = originalPos + Vector3.down * 0.2f;

        if (buttonType == ButtonType.Play)
            drumMachine.Play();
        else if (buttonType == ButtonType.Stop)
            drumMachine.Stop();
    }

    void OnMouseUp()
    {
        transform.position = originalPos;
    }
}
// This script defines a 3D button that can be used to control the DrumMachineManager.
