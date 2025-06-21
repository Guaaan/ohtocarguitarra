using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [Header("Indicators")]
    [SerializeField] private GameObject mouseIndicator;

    [Header("SphereCursor")]
    [SerializeField] private SphereCursor sCursor;

    [Header("Dependencies")]
    [SerializeField] private InputManager inputManager;

    [Header("Muneco")]
    [SerializeField] private GameObject ragdoll;

    [SerializeField] private IsometricCameraController cameraCtrl;

    [Header("Prefab Settings")]
    [SerializeField] public GameObject prefabToInstantiate;
    public GameObject targetselected;
    public Transform instantiatePosition;
    public float yOffset = 1f;

    [SerializeField] private GameObject gridVisualization;

    private void Start()
    {
        targetselected = ragdoll; // Cámara comienza siguiendo al objeto por defecto.
    }

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePosition;

        HandleInput(mousePosition);
    }

    private void HandleInput(Vector3 mousePosition)
    {
        // Instanciar objeto con Shift + clic
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            InstanciarPrefab(mousePosition);
            return; // No ejecutar más acciones este frame
        }

        // Si presionas click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            if (sCursor.currentBtn != null)
            {
                Debug.Log("Presionado: " + sCursor.currentBtn.name);
                sCursor.currentBtn.GetComponent<ButtonController>().PressButton();
            }
            else if (sCursor.hoverTrackable)
            {
                // Si está tocando un objeto trackable, seleccionarlo como target
                SelectTarget(sCursor.hoveringGameObject);
            }
            else
            {
                // Si no hay botón ni trackable, resetear al objeto por defecto
                ResetTarget();
            }
        }

        // Movimiento libre con Shift derecho
        if (Input.GetKey(KeyCode.RightShift))
        {
            cameraCtrl.MovePivotTo(mousePosition);
        }
        else
        {
            // La cámara siempre sigue el target seleccionado
            if (targetselected != null)
            {
                cameraCtrl.MovePivotTo(targetselected.transform.position);
            }
            else
            {
                cameraCtrl.MovePivotTo(ragdoll.transform.position);
            }
        }
    }

    private void SelectTarget(GameObject newTarget)
    {
        Debug.Log("Nuevo target seleccionado: " + newTarget);
        targetselected = newTarget;
    }

    private void ResetTarget()
    {
        Debug.Log("Reseteando target al objeto por defecto.");
        targetselected = ragdoll;
    }

    private void InstanciarPrefab(Vector3 position)
    {
        position.y += yOffset;

        if (prefabToInstantiate != null)
        {
            Instantiate(prefabToInstantiate, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("PrefabToInstantiate no está configurado en el Inspector.");
        }
    }
}
