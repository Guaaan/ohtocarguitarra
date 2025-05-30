using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCursor : MonoBehaviour
{
    public GameObject hoveringGameObject;
    public bool hoverTrackable;
    public GameObject spawn;
    public GameObject currentBtn = null;
    private void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Para evitar que la física afecte al objeto.
    }
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && currentBtn)
        {
        Console.WriteLine("presionado" + currentBtn.name);
            currentBtn.GetComponent<ButtonController>().PressButton();
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"OnTriggerEnter llamado con {other.gameObject.name}, Tag: {other.gameObject.tag}");
        //HiglightSelectTarget(other.gameObject);

        if (other.CompareTag("button"))
        {
            currentBtn = other.gameObject;
            return;
        }
        if (other.CompareTag("trackable")|| other.CompareTag("muneco"))
        {
            Debug.Log("Esta tocando " + other.gameObject);
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            return;
        }
        if (other.CompareTag("sliderhandle"))
        {
            currentBtn = other.gameObject;
            // Puedes cambiar color del handle, por ejemplo:
            Renderer r = other.GetComponent<Renderer>();
            if (r) r.material.color = Color.green;
            return;
        }
        hoverTrackable = false;
        hoveringGameObject = spawn;
        currentBtn = null;
        return;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("button"))
        {
            currentBtn = other.gameObject;
            return;
        }
        if (other.CompareTag("trackable") || other.CompareTag("muneco"))
        {
            Debug.Log("Esta tocando " + other.gameObject);
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            return;
        }
        hoverTrackable = false;
        hoveringGameObject = spawn;
        currentBtn = null;
        return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sliderhandle"))
        {
            // Restaurar color original
            Renderer r = other.GetComponent<Renderer>();
            if (r) r.material.color = Color.white;

            if (currentBtn == other.gameObject)
            {
                currentBtn = null;
            }
        }
    }
}
