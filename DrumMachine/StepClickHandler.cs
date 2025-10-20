using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepClickHandler : MonoBehaviour
{
    private System.Action onClick;

    public void Init(System.Action clickAction)
    {
        onClick = clickAction;
    }

    void OnMouseDown()
    {
        onClick?.Invoke();
    }
}