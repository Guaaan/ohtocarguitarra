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

    public enum CameraMode { Isometric, Perspective }
    public CameraMode currentMode = CameraMode.Isometric;


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

                // StepButton
                var step = sCursor.currentBtn.GetComponent<StepButton>()
                          ?? sCursor.currentBtn.GetComponentInParent<StepButton>()
                          ?? sCursor.currentBtn.GetComponentInChildren<StepButton>();
                if (step != null)
                {
                    step.Toggle();
                    return;
                }

                // ButtonController
                var buttonCtrl = sCursor.currentBtn.GetComponent<ButtonController>()
                               ?? sCursor.currentBtn.GetComponentInParent<ButtonController>()
                               ?? sCursor.currentBtn.GetComponentInChildren<ButtonController>();
                if (buttonCtrl != null)
                {
                    buttonCtrl.PressButton();
                    return;
                }

                // Button3D
                var button3D = sCursor.currentBtn.GetComponent<Button3D>()
                              ?? sCursor.currentBtn.GetComponentInParent<Button3D>()
                              ?? sCursor.currentBtn.GetComponentInChildren<Button3D>();
                if (button3D != null)
                {
                    if (button3D.drumMachine == null)
                        Debug.LogError($"El botón {button3D.name} no tiene asignada la referencia a DrumMachineManager!");

                    if (button3D.buttonType == Button3D.ButtonType.Play)
                        button3D.drumMachine.Play();
                    else if (button3D.buttonType == Button3D.ButtonType.Stop)
                        button3D.drumMachine.Stop();

                    return;
                }

                Debug.LogWarning(
                    $"'{sCursor.currentBtn.name}' tiene tag 'button' pero no encontré StepButton, ButtonController ni Button3D.");
            }
            else if (sCursor.hoverTrackable && sCursor.hoveringGameObject != null)
            {
                SelectTarget(sCursor.hoveringGameObject);
            }

            else if (currentMode == CameraMode.Isometric)
            {
                if (targetselected != null)
                    cameraCtrl.MovePivotTo(targetselected.transform.position);
                else
                    cameraCtrl.MovePivotTo(ragdoll.transform.position);
            }
            else
            {
                // Si no hay botón ni trackable, resetear al objeto por defecto
                ResetTarget();
            }
        }

        // Movimiento libre con Shift derecho
        if (Input.GetKey(KeyCode.LeftShift))
        {
            cameraCtrl.MovePivotTo(mousePosition);
        }
        else
        {
            if (currentMode == CameraMode.Isometric)
            {
                if (targetselected != null)
                    cameraCtrl.MovePivotTo(targetselected.transform.position);
                else
                    cameraCtrl.MovePivotTo(ragdoll.transform.position);
            }
            // en perspectiva no mover el pivot automáticamente,
            // la cámara se queda fija donde SwitchToPerspective la dejó
        }
    }

    private void SelectTarget(GameObject newTarget)
    {
        Debug.Log("Nuevo target seleccionado: " + newTarget);
        targetselected = newTarget;

        if (newTarget.CompareTag("focusable"))
        {
            currentMode = CameraMode.Perspective;
            cameraCtrl.SwitchToPerspective(newTarget.transform);
        }
        else
        {
            currentMode = CameraMode.Isometric;
            cameraCtrl.SwitchToIsometric(newTarget.transform);
        }
    }

    private void ResetTarget()
    {
        Debug.Log("Reseteando target al objeto por defecto.");
        targetselected = ragdoll;
        // volver a cámara isométrica
        cameraCtrl.SetIsometricMode();
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
