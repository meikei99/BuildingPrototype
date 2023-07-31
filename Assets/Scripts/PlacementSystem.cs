using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    // The y-position where you want the grid to be placed above the plane layer
    public float gridHeight = 0.1f;

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Apply the custom cell size to the grid position
        Vector3 cellWorldPosition = grid.CellToWorld(gridPosition) + grid.cellSize * 0.5f;

        // Set the y-position of the grid above the plane layer
        cellWorldPosition.y = gridHeight;

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = cellWorldPosition;
    }
}
