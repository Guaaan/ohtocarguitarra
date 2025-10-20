using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCursor : MonoBehaviour
{
    public GameObject hoveringGameObject;
    public bool hoverTrackable;
    public GameObject spawn;
    public GameObject currentBtn = null;

    [Header("Cursor Visual")]
    [SerializeField] private Renderer cursorRenderer;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color buttonColor = Color.yellow;
    [SerializeField] private Color trackableColor = Color.green;
    [SerializeField] private Color focusable = Color.magenta;
    [SerializeField] private Color sliderColor = Color.cyan;

    private void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Para evitar que la física afecte al objeto.

        if (cursorRenderer == null)
        {
            cursorRenderer = GetComponent<Renderer>();
        }

        SetCursorColor(defaultColor);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("button"))
        {
            currentBtn = other.gameObject;
            SetCursorColor(buttonColor);
            return;
        }

        if (other.CompareTag("trackable") || other.CompareTag("muneco"))
        {
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            SetCursorColor(trackableColor);
            return;
        }

        if (other.CompareTag("focusable"))
        {
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            SetCursorColor(focusable);
            return;
        }

        if (other.CompareTag("sliderhandle"))
        {
            currentBtn = other.gameObject;
            Renderer r = other.GetComponent<Renderer>();
            if (r) r.material.color = Color.green;
            SetCursorColor(sliderColor);
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("button"))
        {
            currentBtn = other.gameObject;
            SetCursorColor(buttonColor);
            return;
        }

        if (other.CompareTag("trackable") || other.CompareTag("muneco"))
        {
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            SetCursorColor(trackableColor);
            return;
        }

        if (other.CompareTag("focusable"))
        {
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            SetCursorColor(focusable);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sliderhandle"))
        {
            Renderer r = other.GetComponent<Renderer>();
            if (r) r.material.color = Color.white;

            if (currentBtn == other.gameObject)
            {
                currentBtn = null;
            }

            SetCursorColor(defaultColor);
            return;
        }

        if (other.CompareTag("button") && currentBtn == other.gameObject)
        {
            currentBtn = null;
            SetCursorColor(defaultColor);
            return;
        }

        if ((other.CompareTag("trackable") || other.CompareTag("muneco") || other.CompareTag("focusable"))
            && hoveringGameObject == other.gameObject)
        {
            hoverTrackable = false;
            hoveringGameObject = spawn;
            SetCursorColor(defaultColor);
            return;
        }
    }

    private void SetCursorColor(Color color)
    {
        if (cursorRenderer != null)
        {
            cursorRenderer.material.color = color;
        }
    }
}
