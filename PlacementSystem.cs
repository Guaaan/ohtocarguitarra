using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("Indicators")]
    [SerializeField]
    private GameObject mouseIndicator; // Indicador para la posici�n del mouse
    [SerializeField]
    private GameObject cellIndicator;  // Indicador para la celda del grid

    [Header("Dependencies")]
    [SerializeField]
    private InputManager inputManager; // Referencia al administrador de entradas
    [SerializeField]
    private Grid grid;                 // Referencia al grid

    [Header("Prefab Settings")]
    [SerializeField]
    public GameObject prefabToInstantiate; // Prefab que se instanciar�
    public Transform instantiatePosition;  // Posici�n base para la instanciaci�n
    public float yOffset = 1f;             // Desplazamiento en el eje Y

    [Header("Object Database")]
    [SerializeField]
    private ObjectsDatabaseSO databaseSO;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
    }
    public void StartPlacement(int ID)
    {
        selectedObjectIndex = databaseSO.objectsData.FindIndex(data=> data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No id found {ID}");
            return;
        }
        gridVisualization.SetActive(true); 
        cellIndicator.SetActive(true);
        //inputManager.OnClicked += PlaceStructure;
    }

    private void PlaceStructure()
    {
        throw new NotImplementedException();
    }

    private void StopPlacement()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        // Actualizar posici�n de los indicadores
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        // Verificar si se debe instanciar un prefab
        HandlePrefabInstantiation(grid.CellToWorld(gridPosition));
    }

    private void HandlePrefabInstantiation(Vector3 position)
    {
        // Verificar Shift + clic izquierdo
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            InstanciarPrefab(position);
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
