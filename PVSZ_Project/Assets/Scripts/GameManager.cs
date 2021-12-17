//#define DEBUG_GAMEMANAGER

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

using Random = System.Random;

enum TurnType
{
    Tech,
    Alien
}

enum GameState
{
    Playing,
    Paused
}

[System.Serializable]
public class AlienWithNum
{
    public Alien alien;
    public int num;
}

public class GameManager : MonoBehaviour
{
    // NOTE(sftl): singleton
    public static GameManager Instance;
    
    //- scene objects
    public ResourceManager  resourceManager;
    public GridManager      gridManager;
    public PlayUI           playUI;
    public PointerRaycaster raycaster;
    
    //- tech unit prefabs
    public Preview  shooterPrevPrefab;
    public TechUnit shooterPrefab;
    
    public Preview  removePrevPrefab;
    
    //- power prefabs
    public PowerScan    powerScanPrefab;
    public Preview      powerScanPrevPrefab;
    
    //- alien unit prefabs
    public AlienStandard    baseAlienPrefab;
    public AlienMoonwalker  specialAlienPrefab;
    
    public AlienTank        alienTankPrefab;
    
    //- unit costs
    public int unitCost;
    public int powerScanCost;
    
    //- turns
    public int  TurnNum = 1;
    TurnType    currTurn = TurnType.Tech;
    public      GameEvent turnIncrementedEvent;
    
    //- game state
    GameState   gameState = GameState.Playing;
    
    //- utils
    public List<SerializableList<AlienWithNum>> aliensPerLane;
    
    Preview                 currPreview;
    List<Alien>             aliens = new();
    List<TechPrototype>     techs = new();
    List<TechPrototype>     temp_techs = new();
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        StartTechTurn();
    }
    
    void Update()
    {
        if(gameState == GameState.Paused) return;
        
        if (Input.GetMouseButtonUp(1)) // NOTE(sftl): right click
        {
            if (currPreview != null)
            {
                ClearPreview();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currPreview != null)
            {
                if (currPreview.type == PreviewType.Tech)
                {
                    Tile tile = gridManager.GetSelectedTileIfAvailable();
                    
                    if (tile != null)
                    {
                        if (ResourceManager.getResources() >= unitCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(unitCost);
                            
                            //-instantiate TechUnit
                            var pos = tile.transform.position;
                            var techUnit = Instantiate(shooterPrefab, pos, Quaternion.identity);
                            techs.Add(techUnit);
                            tile.Unit = techUnit;
                        }
                    }
                }
                else if (currPreview.type == PreviewType.PowerScan) // NOTE(sftl): method?
                {
                    Tile tile = gridManager.GetSelectedTileIfAvailable();
                    
                    if (tile != null)
                    {
                        if (ResourceManager.getResources() >= powerScanCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(powerScanCost);
                            
                            //-instantiate TechUnit
                            var pos = tile.transform.position;
                            var techUnit = Instantiate(powerScanPrefab, pos, Quaternion.identity);
                            techs.Add(techUnit);
                            temp_techs.Add(techUnit);
                            tile.Unit = techUnit;
                        }
                    }
                }
                else // NOTE(sftl): RemovePreview
                {
                    Tile tile = gridManager.GetSelectedTileIfOccupied();
                    
                    if (tile != null)
                    {
                        //-remove TechUnit
                        Destroy(tile.Unit.gameObject);
                        techs.Remove(tile.Unit as TechUnit); // NOTE(sftl): player is not able to try to remove alien unit
                        tile.Unit = null;
                    }
                }
            }
        }
        
        //-handle unit front rendering
        // TODO(sftl): optimise, don't do this every frame, just when unit spawns or changes lane
        // TODO(sftl): when spawning, rendering position may not be accurate the first frame
        // TODO(sftl): handle infinite units
        
        float precision = 0.2f; // Real world width for which units are considered to be on the same rendering layer and rendering order is not defined
        int refOrder = 15000;   // SpriteRenderer.sortingOrder max is 32767
        
        if (aliens.Count > 0)
        {
            float refPos = aliens.First().transform.position.y;
            
            foreach (var alien in aliens)
            {
                var diffFromRefPos = refPos - alien.transform.position.y;
                int diffFromRefOrder = (int)(diffFromRefPos / precision);
                alien.GetComponent<SpriteRenderer>().sortingOrder = refOrder + diffFromRefOrder;
            }
        }
        
        if (techs.Count > 0)
        {
            float refPos = techs.First().transform.position.y;
            
            foreach (var tech in techs)
            {
                var diffFromRefPos = refPos - tech.transform.position.y;
                int diffFromRefOrder = (int)(diffFromRefPos / precision);
                tech.GetComponent<SpriteRenderer>().sortingOrder = refOrder + diffFromRefOrder;
            }
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale  = 0f;
        gameState       = GameState.Paused;
        
        raycaster.Deactivate();
        gridManager.DeselectTile();
        
        if (currPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview hidden on pause.");
#endif
            currPreview.gameObject.SetActive(false);
        }
    }
    
    public void ResumeGame()
    {
        Time.timeScale  = 1f;
        gameState       = GameState.Playing;
        
        raycaster.Activate();
        
        if (currPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview un-hidden on pause.");
#endif
            currPreview.gameObject.SetActive(true);
        }
    }
    
    public void TechCardClicked(TechCard card)
    {
        var prevPreview = currPreview;
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currPreview = Instantiate(shooterPrevPrefab, pos, Quaternion.identity); // TODO(sftl): use card type
        
        if (prevPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview destroyed since new one is initialized.");
#endif
            Destroy(prevPreview.gameObject);
        }
    }
    
    public void RemoveCardClicked()
    {
        var prevPreview = currPreview;
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currPreview = Instantiate(removePrevPrefab, pos, Quaternion.identity);
        
        if (prevPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview destroyed since new one is initialized.");
#endif
            Destroy(prevPreview.gameObject);
        }
    }
    
    public void PowerCardClicked(PowerCard card)
    {
        var prevPreview = currPreview;
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currPreview = Instantiate(powerScanPrevPrefab, pos, Quaternion.identity);
        
        if (prevPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview destroyed since new one is initialized.");
#endif
            Destroy(prevPreview.gameObject);
        }
    }
    
    void ClearPreview()
    {
#if DEBUG_GAMEMANAGER
        Debug.Log("Preview destroyed on right click.");
#endif
        Destroy(currPreview.gameObject);
        currPreview = null;
    }
    
    public void StartTechTurn()
    {
        // TODO(sftl): generate next alien wave
        gridManager.SetAlienLaneIndicators();
    }
    
    public void EndTechTurn()
    {
        currTurn = TurnType.Alien;
        playUI.OnAlienTurn();
        
        if (currPreview != null)
        {
            ClearPreview();
        }
        
        SpawnAliens();
    }
    
    public void EndAlienTurn()
    {
        currTurn = TurnType.Tech;
        TurnNum++;
        StartTechTurn();
        playUI.OnTechTurn();
        
        if (currPreview != null)
        {
            ClearPreview();
        }
        
        //-remove temp tech units
        foreach (var unit in temp_techs)
        {
            Destroy(unit.gameObject);
            techs.Remove(unit);
        }
        temp_techs.Clear();
    }
    
    public void OnTechDeath(TechPrototype unit)
    {
        techs.Remove(unit);
        temp_techs.Remove(unit);
    }
    
    void SpawnAliens()
    {
        var availablePos = gridManager.GetAvailableSpawnPos();
        
        //-spawn aliens by specified aliensPerLane
        var random = new Random();
        for (int i = 0; i < aliensPerLane.Count; i++)
        {
            foreach (var alienWithNum in aliensPerLane[i].data)
            {
                for (int i2 = 0; i2 < alienWithNum.num; i2++)
                {
                    var sec = (float)random.NextDouble();
                    var currIndex = i; // TODO(sftl): avoid this
                    
                    StartCoroutine(
                                   DoAfterSec(
                                              sec,
                                              () =>
                                              {
                                                  var pos = availablePos[currIndex];
                                                  var alien = Instantiate(alienWithNum.alien, pos, Quaternion.identity);
                                                  aliens.Add(alien);
                                              }
                                              )
                                   );
                }
            }
        }
    }
    
    public void OnAlienDeath(Alien alien)
    {
        aliens.Remove(alien);
        
        if (aliens.Count == 0)
        {
            EndAlienTurn();
            turnIncrementedEvent.Raise();
        }
    }
    
    IEnumerator DoAfterSec(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action();
    }
}
