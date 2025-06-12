using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepButton : MonoBehaviour
{
    public bool isActive = false;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateColor();
    }

    public void Toggle()
    {
        isActive = !isActive;
        UpdateColor();
    }

    public void Highlight()
    {
        rend.material.color = isActive ? Color.yellow : Color.gray;
    }

    public void Unhighlight()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        rend.material.color = isActive ? Color.green : Color.white;
    }
}