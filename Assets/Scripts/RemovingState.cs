using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedback soundFeedback;
    private PlacementSystem placementSystem; // Add this variable

    public RemovingState(int iD,
                         Grid grid,
                         PreviewSystem previewSystem,
                         ObjectsDatabaseSO database,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer,
                         SoundFeedback soundFeedback,
                         PlacementSystem placementSystem) // Add this parameter in the constructor
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;
        this.placementSystem = placementSystem; // Assign the parameter to the variable
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        // Check if the selected grid position contains any object
        PlacementData placementData = furnitureData.GetPlacementData(gridPosition);
        if (placementData != null)
        {
            selectedData = furnitureData;
        }
        else
        {
            placementData = floorData.GetPlacementData(gridPosition);
            if (placementData != null)
            {
                selectedData = floorData;
            }
        }

        if (selectedData == null)
        {
            // sound
            soundFeedback.PlaySound(SoundType.wrongPlacement);
        }
        else
        {
            soundFeedback.PlaySound(SoundType.Remove);
            int gameObjectIndex = placementData.PlacedObjectIndex;

            // Get the cost of the object to be removed
            int removedObjectCost = database.objectsData[placementData.ID].Cost;

            // Remove the object from the grid and object placer
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);

            // Add the cost back to the player's money
            placementSystem.AddMoney(removedObjectCost);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObejctAt(gridPosition, Vector2Int.one) &&
                 floorData.CanPlaceObejctAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
