using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCursor : MonoBehaviour
{
    public GameObject hoveringGameObject;
    public bool hoverTrackable;
    public GameObject spawn;
    private void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Para evitar que la física afecte al objeto.
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"OnTriggerEnter llamado con {other.gameObject.name}, Tag: {other.gameObject.tag}");
        //HiglightSelectTarget(other.gameObject);

        if (other.CompareTag("trackable")|| other.CompareTag("muneco"))
        {
            Debug.Log("Esta tocando " + other.gameObject);
            hoverTrackable = true;
            hoveringGameObject = other.gameObject;
            return;
        }
        hoverTrackable = false;
        hoveringGameObject = spawn;
        return;
    }

  
}
