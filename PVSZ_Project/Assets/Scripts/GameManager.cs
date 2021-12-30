//#define DEBUG_GAMEMANAGER
//#define NO_END_GAME

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

public enum TurnType
{
    Tech,
    Alien
}

public enum GameState
{
    Playing,
    Paused,
    End,
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
    public WaveGenerator    waveGen;
    
    public EndScreen        endScreen;
    
    //- tech unit prefabs
    public Preview  guardianPrevPrefab;
    public GuardianUnit guardianPrefab;
    public Preview resourceCollectorPrevPrefab;
    public ResourceUnit resourceCollectorPrefab;
    public Preview machineGunPrevPrefab;
    public MachineGunUnit machineGunPrefab;
    public Preview wallPrevPrefab;
    public WallUnit wallPrefab;
    
    public Preview  removePrevPrefab;
    
    //- power prefabs
    public ScanUnit    powerScanPrefab;
    
    public Preview      powerScanPrevPrefab;
    public Preview      powerBlockPrevPrefab;
    public Preview      powerSlowPrevPrefab;
    public Preview      powerShieldPrevPrefab; 
    
    //- unit costs
    public int guardianCost;
    public int machineGunCost;
    public int resourceCollectorCost;
    public int wallCost;
    
    public int powerScanCost;
    public int powerBlockCost;
    public int powerSlowCost;
    public int powerShieldCost;
    
    //- unit cards
    public TechCard guardianCard;
    public TechCard machineGunCard;
    public TechCard resourceCollectorCard;
    public TechCard wallCard;
    
    public PowerCard scanCard;
    public PowerCard blockCard;
    public PowerCard slowCard;
    public PowerCard shieldCard;
    
    //- turns
    public const int    EndTurnNum = 5;
    public int          TurnNum = 1;
    
    public      TurnType  TurnType = TurnType.Tech;
    public      GameEvent turnIncrementedEvent;
    
    //- game state
    GameState   gameState = GameState.Playing;
    
    //- utils
    public readonly Vector3 DrawOrderRefPos = Vector3.zero;
    public readonly int     DrawOrderRefNum = 15000;    // SpriteRenderer.sortingOrder max is 32767
    public readonly float   DrawOrderPrecision = 0.2f;  // Real world width for which units are considered to be on the same rendering layer and rendering order is not defined
    
    public Preview          CurrentPreview;
    
    List<Alien>             aliens = new();
    List<List<Alien>>       nextWaveAliens = new();
    
    List<TechUnit>     techs = new();
    List<TechUnit>     temp_techs = new();
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Debug.Log("Tried to create mutliple GameManager instances.");
        }
        
        Instance = this;
    }
    
    void Start()
    {
        GenWaveAndShow();
        gridManager.SetTilesIsBlocked();
    }
    
    void Update()
    {
        if(gameState == GameState.Paused || gameState == GameState.End) return;
        
        if (Input.GetMouseButtonUp(1)) // NOTE(sftl): right click
        {
            if (CurrentPreview != null)
            {
                DestroyPreviewIfNotNull();
                SetPreview(null);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (CurrentPreview != null)
            {
                if (CurrentPreview.Type == PreviewType.Guardian)
                {
                    Tile tile = gridManager.GetSelectedTileIfAvailable();
                    
                    if (tile != null)
                    {
                        if (ResourceManager.getResources() >= guardianCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(guardianCost);
                            
                            //-instantiate TechUnit
                            var pos = tile.transform.position;
                            var techUnit = Instantiate(guardianPrefab, pos, Quaternion.identity);
                            techs.Add(techUnit);
                            tile.Unit = techUnit;
                        }
                    }
                }
                else if (CurrentPreview.Type == PreviewType.PowerScan) // NOTE(sftl): method?
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
                else if (CurrentPreview.Type == PreviewType.ResourceUnit) // NOTE(sftl): method?
                {
                    Tile tile = gridManager.GetSelectedTileIfAvailable();
                    
                    if (tile != null)
                    {
                        if (ResourceManager.getResources() >= resourceCollectorCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(resourceCollectorCost);
                            
                            //-instantiate TechUnit
                            var pos = tile.transform.position;
                            var techUnit = Instantiate(resourceCollectorPrefab, pos, Quaternion.identity);
                            techs.Add(techUnit);
                            tile.Unit = techUnit;
                        }
                    }
                }
                else if (CurrentPreview.Type == PreviewType.MachineGun) // NOTE(sftl): method?
                {
                    Tile tile = gridManager.GetSelectedTileIfAvailable();
                    
                    if (tile != null)
                    {
                        if (ResourceManager.getResources() >= machineGunCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(machineGunCost);
                            
                            //-instantiate TechUnit
                            var pos = tile.transform.position;
                            var techUnit = Instantiate(machineGunPrefab, pos, Quaternion.identity);
                            techs.Add(techUnit);
                            tile.Unit = techUnit;
                        }
                    }
                }
                else if (CurrentPreview.Type == PreviewType.Wall)
                {
                    Tile tile = gridManager.GetSelectedTileIfAvailable();
                    
                    if (tile != null)
                    {
                        if (ResourceManager.getResources() >= wallCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(wallCost);
                            
                            //-instantiate TechUnit
                            var pos = tile.transform.position;
                            var techUnit = Instantiate(wallPrefab, pos, Quaternion.identity);
                            techs.Add(techUnit);
                            tile.Unit = techUnit;
                        }
                    }
                }
                else if (CurrentPreview.Type == PreviewType.PowerBlock) // NOTE(sftl): method?
                {
                    var laneData = gridManager.GetSelectedLaneStartEndPos();
                    
                    if (laneData != null)
                    {
                        if (ResourceManager.getResources() >= powerBlockCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(powerBlockCost);
                            
                            var (laneStartPos, laneEndPos) = laneData.Value;
                            var dir = Vector3.Normalize(laneEndPos - laneStartPos);
                            var hits = Physics2D.RaycastAll(laneStartPos, dir, Mathf.Infinity, LayerMask.GetMask("Alien"));
                            
                            foreach (var hit in hits)
                            {
                                var alien = hit.collider.gameObject.GetComponent<Alien>();
                                //if(!alien.IsChangingLanes) 
                                alien.MoveToNeightourLane();
                            }
                            
                            DestroyPreviewIfNotNull();
                            SetPreview(null);
                        }
                    }
                }
                else if (CurrentPreview.Type == PreviewType.PowerSlow)
                {
                    if (gridManager.SelectedTile != null)
                    {
                        if (ResourceManager.getResources() >= powerSlowCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(powerSlowCost);
                            
                            aliens.ForEach(action: (Alien a) => { a.SlowForSec(5); });
                            
                            DestroyPreviewIfNotNull();
                            SetPreview(null);
                        }
                    }
                }
                else if (CurrentPreview.Type == PreviewType.PowerShield)
                {
                    if (gridManager.SelectedTile != null)
                    {
                        if (ResourceManager.getResources() >= powerShieldCost)
                        { // =checking if u have enough reosources to pay for the unit
                            //-decreasing resources
                            
                            resourceManager.payForUnit(powerShieldCost);
                            
                            techs.ForEach(action: (TechUnit a) => { a.AddShieldForSec(5); });
                            
                            DestroyPreviewIfNotNull();
                            SetPreview(null);
                        }
                    }
                }
                else // NOTE(sftl): RemovePreview
                {
                    Tile tile = gridManager.GetSelectedTileIfOccupied();
                    
                    if (tile != null)
                    {
                        // TODO(sftl): give reseources back for units that are placed this turn or maybe even previous turns
                        //-remove TechUnit
                        Destroy(tile.Unit.gameObject);
                        techs.Remove(tile.Unit as TechUnit); // NOTE(sftl): player is not able to try to remove alien unit
                        tile.Unit = null;
                    }
                }
            }
        }
        
        //-alien loop
        List<Alien> aliensInBase = new();
        
        if (aliens.Count > 0)
        {
            foreach (var alien in aliens)
            {
                // handle aliens in base
                if (gridManager.IsAlienInPlayerBase(alien.transform.position))
                {
                    aliensInBase.Add(alien);
                }
                
                //-handle unit front rendering
                // TODO(sftl): optimise, don't do this every frame, just when unit spawns or changes lane
                // TODO(sftl): when spawning, rendering position may not be accurate the first frame
                // TODO(sftl): handle infinite units
                var diffFromRefPos = DrawOrderRefPos.y - alien.transform.position.y;
                int diffFromRefOrder = (int)(diffFromRefPos / DrawOrderPrecision);
                alien.GetComponent<SpriteRenderer>().sortingOrder = DrawOrderRefNum + diffFromRefOrder;
            }
        }
        
#if NO_END_GAME
        aliensInBase.ForEach(alien => {
                                 OnAlienDeath(alien);
                                 Destroy(alien.gameObject);
                             });
#else
        if (aliensInBase.Count > 0) PlayerLost();
#endif
    }
    
    public void PauseGame()
    {
        Time.timeScale  = 0f;
        gameState       = GameState.Paused;
        
        raycaster.Deactivate();
        
        if (CurrentPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview hidden on pause.");
#endif
            CurrentPreview.gameObject.SetActive(false);
        }
    }
    
    public void ResumeGame()
    {
        Time.timeScale  = 1f;
        gameState       = GameState.Playing;
        
        raycaster.Activate();
        
        if (CurrentPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview un-hidden on pause.");
#endif
            CurrentPreview.gameObject.SetActive(true);
        }
    }
    
    public void TechCardClicked(TechCard card)
    {
#if DEBUG_GAMEMANAGER
        Debug.Log("Preview destroyed since new one is initialized.");
#endif
        DestroyPreviewIfNotNull();
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (card.type == CardType.Guardian)
            SetPreview(Instantiate(guardianPrevPrefab, pos, Quaternion.identity)); // TODO(sftl): use card type
        
        if (card.type == CardType.Collector)
            SetPreview(Instantiate(resourceCollectorPrevPrefab, pos, Quaternion.identity));
        
        if (card.type == CardType.MachineGun)
            SetPreview(Instantiate(machineGunPrevPrefab, pos, Quaternion.identity));
        
        if (card.type == CardType.Wall)
            SetPreview(Instantiate(wallPrevPrefab, pos, Quaternion.identity));
    }
    
    public void RemoveCardClicked()
    {
#if DEBUG_GAMEMANAGER
        Debug.Log("Preview destroyed since new one is initialized.");
#endif
        DestroyPreviewIfNotNull();
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SetPreview(Instantiate(removePrevPrefab, pos, Quaternion.identity));
    }
    
    public void PowerCardClicked(PowerCard card)
    {
#if DEBUG_GAMEMANAGER
        Debug.Log("Preview destroyed since new one is initialized.");
#endif
        DestroyPreviewIfNotNull();
        
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if(card.Type == PowerCardType.Scan) 
        {
            SetPreview(Instantiate(powerScanPrevPrefab, pos, Quaternion.identity));
        }
        else if(card.Type == PowerCardType.Block)
        {
            SetPreview(Instantiate(powerBlockPrevPrefab, pos, Quaternion.identity));
        }
        else if(card.Type == PowerCardType.Slow)
        {
            SetPreview(Instantiate(powerSlowPrevPrefab, pos, Quaternion.identity));
        }
        else if(card.Type == PowerCardType.Shield)
        {
            SetPreview(Instantiate(powerShieldPrevPrefab, pos, Quaternion.identity));
        }
    }
    
    public void GenWaveAndShow()
    {
        nextWaveAliens = waveGen.GetWave(TurnNum);
        gridManager.SetAlienLaneIndicators(nextWaveAliens);
    }
    
    public void EndTechTurn()
    {
        TurnType = TurnType.Alien;
        playUI.OnAlienTurn();
        
        if (CurrentPreview != null)
        {
            DestroyPreviewIfNotNull();
            SetPreview(null);
        }
        
        SpawnAliens();
    }
    
    public void EndAlienTurn()
    {
        Assert.IsTrue(aliens.Count == 0);
        
        if (CurrentPreview != null)
        {
            DestroyPreviewIfNotNull();
            SetPreview(null);
        }
        
        //-remove temp tech units
        foreach (var unit in temp_techs)
        {
            Destroy(unit.gameObject);
            techs.Remove(unit);
        }
        
        //check if unit is of certain type
        foreach (TechUnit unit in techs)
        {
            if (unit.GetType() == typeof(ResourceUnit))
            {
                ResourceUnit resourceUnit = (ResourceUnit) unit;
                resourceUnit.IncreaseRescources();
            }
        }
        temp_techs.Clear();
        
        //-remove status effects
        foreach (var unit in techs)
        {
            unit.RemoveStatusEffects();
        }
        
        TurnType = TurnType.Tech;
        TurnNum++;
        playUI.OnTechTurn();
        GenWaveAndShow();
        
        Time.timeScale = 1f; // NOTE(sftl): if fast forwarded, reset to normal speed
    }
    
    public void FastForward()
    {
        Time.timeScale = 10f;
    }
    
    void DestroyPreviewIfNotNull()
    {
        if (CurrentPreview == null) return;
        Destroy(CurrentPreview.gameObject);
    }
    
    void SetPreview(Preview newPreview)
    {
        // TODO(sftl): we shouldn't even change preview objects if they are of same type?
        var oldPreview = CurrentPreview;
        
        CurrentPreview = newPreview;
        
        if(oldPreview != null && newPreview == null) gridManager.PreviewCleared(oldPreview);
    }
    
    public void OnTechDeath(TechUnit unit)
    {
        techs.Remove(unit);
        temp_techs.Remove(unit);
    }
    
    void SpawnAliens()
    {
        var availablePos = gridManager.GetAvailableSpawnPos();
        
        // NOTE(sftl): spawning with small random time differences
        var random = new Random();
        for (int i = 0; i < nextWaveAliens.Count; i++)
        {
            foreach (var alienPrefab in nextWaveAliens[i])
            {
                var sec = (float)random.NextDouble(); // NOTE(sftl): range [0, 1)
                var pos = availablePos[i];
                
                StartCoroutine(
                               DoAfterSec(
                                          sec,
                                          () =>
                                          {
                                              var alien = Instantiate(alienPrefab, pos, Quaternion.identity);
                                              aliens.Add(alien);
                                          }
                                          )
                               );
            }
        }
    }
    
    public void OnAlienDeath(Alien alien)
    {
        aliens.Remove(alien);
        
        if (aliens.Count == 0)
        {
            //-handle win
#if !NO_END_GAME
            if (TurnNum >= EndTurnNum){
                PlayerWon();
                return;
            }
#endif
            
            EndAlienTurn();
            turnIncrementedEvent.Raise();
        }
    }
    
    public void PlayerLost()
    {
        Time.timeScale  = 0f;
        gameState       = GameState.End;
        
        raycaster.Deactivate();
        
        if (CurrentPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview hidden on end game.");
#endif
            CurrentPreview.gameObject.SetActive(false);
        }
        
        endScreen.gameObject.SetActive(true);
        endScreen.SetWinState(won: false);
    }
    
    public void PlayerWon()
    {
        Time.timeScale  = 0f;
        gameState       = GameState.End;
        
        raycaster.Deactivate();
        
        if (CurrentPreview != null)
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Preview hidden on end game.");
#endif
            CurrentPreview.gameObject.SetActive(false);
        }
        
        endScreen.gameObject.SetActive(true);
        endScreen.SetWinState(won: true);
    }
    
    public void UpdateCardsForResources(int res)
    {
        var turnType = GameManager.Instance.TurnType;
        
        //-tech cards 
        if (res < guardianCost)             guardianCard.Disable();
        if (res < machineGunCost)           machineGunCard.Disable();
        if (res < resourceCollectorCost)    resourceCollectorCard.Disable();
        
        if (res >= guardianCost)            guardianCard.Enable();
        if (res >= machineGunCost)          machineGunCard.Enable();
        if (res >= resourceCollectorCost)   resourceCollectorCard.Enable();
        if (res >= wallCost)                wallCard.Enable();
        
        //-power cards
        if (res < powerScanCost)    scanCard.Disable();
        if (res < powerBlockCost)   blockCard.Disable();
        if (res < powerSlowCost)    slowCard.Disable();
        if (res < powerShieldCost)  shieldCard.Disable();
        
        if (res >= powerScanCost)    scanCard.Enable();
        if (res >= powerBlockCost)   blockCard.Enable();
        if (res >= powerSlowCost)    slowCard.Enable();
        if (res >= powerShieldCost)  shieldCard.Enable();
    }
    
    IEnumerator DoAfterSec(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action();
    }
}
