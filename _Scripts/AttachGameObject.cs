using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachGameObject: MonoBehaviour
{
    public Transform leftHand; // Mano izquierda del personaje
    public Transform rightHand; // Mano derecha del personaje
    public Transform grabPoint1; // Primer punto de agarre del objeto
    public Transform grabPoint2; // Segundo punto de agarre del objeto

    private Transform grabbedObject; // Objeto actualmente agarrado

    public void GrabObject(Transform objectToGrab)
    {
        // Asignar el objeto a la variable
        grabbedObject = objectToGrab;

        // Alinear el primer punto de agarre con la mano izquierda
        objectToGrab.position = leftHand.position;
        objectToGrab.rotation = leftHand.rotation;

        // Calcular la posición y rotación relativa al segundo punto de agarre
        Vector3 offset = grabPoint2.position - grabPoint1.position;
        objectToGrab.position += rightHand.position - (leftHand.position + offset);

        // Parentar el objeto al personaje para mantenerlo fijo
        objectToGrab.SetParent(transform);
    }

    public void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            // Desparentar el objeto
            grabbedObject.SetParent(null);
            grabbedObject = null;
        }
    }

    private void Update()
    {
        // Test para recoger un objeto con la tecla "E" (ajusta según tu necesidad)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedObject == null)
            {
                // Implementa un método para seleccionar el objeto cercano
                Transform nearbyObject = FindNearbyObject();
                if (nearbyObject != null)
                {
                    GrabObject(nearbyObject);
                }
            }
            else
            {
                ReleaseObject();
            }
        }
    }

    private Transform FindNearbyObject()
    {
        // Implementa la lógica para encontrar el objeto cercano
        // Por ejemplo, usando un collider o raycasting
        return null;
    }
}
 
