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

    // Este método lo llamarás desde CameraRig cuando haya click
    public void Press()
    {
        transform.position = originalPos + Vector3.down * 0.2f;

        if (buttonType == ButtonType.Play)
        {
            print("play pressed");
            drumMachine.Play();

        }
        else if (buttonType == ButtonType.Stop) { 
            print("play pressed");
        drumMachine.Stop();
    }
        // Simular que se levanta solo
        StartCoroutine(ReleaseAfterDelay(0.1f));
    }

    private IEnumerator ReleaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = originalPos;
    }
}
