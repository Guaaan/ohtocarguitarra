using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [Header("Indicators")]
    [SerializeField]
    private GameObject mouseIndicator; // Indicador para la posici�n del mouse

    [Header("Dependencies")]
    [SerializeField]
    private InputManager inputManager; // Referencia al administrador de entradas
    [SerializeField]
    private IsometricCameraController cameraCtrl;
    //private Grid grid;                 // Referencia al grid

    [Header("Prefab Settings")]
    [SerializeField]
    public GameObject prefabToInstantiate; // Prefab que se instanciar�
    public GameObject targetselected; // el objetivo al que se acercará la camara
    public Transform instantiatePosition;  // Posici�n base para la instanciaci�n
    public float yOffset = 1f;             // Desplazamiento en el eje Y

    [SerializeField]
    private GameObject gridVisualization;
    [SerializeField]

    private void Start()
    {
        //StopPlacement();
    }

    private void Update()
    {
        // Actualizar posici�n de los indicadores
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();


        mouseIndicator.transform.position = mousePosition;


        // Verificar si se debe instanciar un prefab
        HandlePrefabInstantiation(mousePosition);

        MoveToCursor(mousePosition);
       
    }


    private void HandlePrefabInstantiation(Vector3 position)
    {
        // Verificar Shift + clic izquierdo
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            InstanciarPrefab(position);
        }
    }
    private void MoveToCursor(Vector3 position)
    {
        //que vaya al seleccionado
        if (Input.GetMouseButtonDown(0) && targetselected)
        {
            Debug.Log(targetselected);
            cameraCtrl.MovePivotTo(targetselected.transform.position);
            //cameraCtrl.MovePivotTo(position);
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            Debug.Log("clickeado " + position); ;
            //muee el pivote hasta pos 
            cameraCtrl.MovePivotTo(position);
        }

    }
    private void InstanciarPrefab(Vector3 position)
    {
        // Ajustar la posici�n en el eje Y
        position.y += yOffset;

        // Validar que el prefab sea v�lido
        if (prefabToInstantiate != null)
        {
            // Instanciar el prefab en la posici�n ajustada
            Instantiate(prefabToInstantiate, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("PrefabToInstantiate no est� configurado en el Inspector.");
        }
    }
}
