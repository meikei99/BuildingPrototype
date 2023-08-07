using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;




public class PlacementSystem : MonoBehaviour
{
    [SerializeField] 
    private InputManager inputManager;
    
    [SerializeField] 
    private Grid grid;
    
    [SerializeField] 
    private ObjectsDatabaseSO database;
    
    [SerializeField] 
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;


    [SerializeField]
    private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField]
    private AudioSource source;

    
    public float gridHeight = 0.1f; 

    [SerializeField]
    private PreviewSystem preview;
    [SerializeField]
    private ObjectPlacer objectPlacer;
    [SerializeField]
    private SoundFeedback soundFeedback;
    public int currentMoney;
    public int startingMoney = 999999;
    public TextMeshProUGUI moneyText;



    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    IBuildingState buildingState;
    private UIManager uiManager; // Reference to the UIManager script
    private void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyUI();
        StopPlacement();
        gridVisualization.SetActive(false);
        floorData = new();
        furnitureData= new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer,
                                           soundFeedback,
                                           this);
      
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid,
                                          preview,
                                          floorData,
                                          furnitureData,
                                          objectPlacer,
                                          soundFeedback);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }


    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);  
        buildingState.OnAction(gridPosition);
     
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 4 ?
    //        floorData :
    //        furnitureData;

    //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

   
    private void Update()
    {
        if (buildingState == null)
        {
            return;
        }

        // Rest of the Update method remains the same
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3 cellWorldPosition = grid.CellToWorld(gridPosition) + grid.cellSize * 0.5f;
        cellWorldPosition.y = gridHeight;
        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition= gridPosition;
        }     
    }

    public void DeductMoney(int amount)
    {
        currentMoney -= amount;
        UpdateMoneyUI();
    }
    private void UpdateMoneyUI()
    {
        moneyText.text = currentMoney.ToString();
    }
}
