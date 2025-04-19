using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCursor : MonoBehaviour
{
    public CameraRig cameraRig;
    private void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Para evitar que la física afecte al objeto.
    }
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"OnTriggerEnter llamado con {other.gameObject.name}, Tag: {other.gameObject.tag}");
        //HiglightSelectTarget(other.gameObject);

        if (other.CompareTag("Ampli")|| other.CompareTag("muneco"))
        {
            Debug.Log("Esta tocando " + other.tag);
            HiglightSelectTarget(other.gameObject);
            return;
        }
        HiglightSelectTarget(null);
        return;
    }

    void HiglightSelectTarget(GameObject target)
    {
        cameraRig.targetselected = target;
    }
}
