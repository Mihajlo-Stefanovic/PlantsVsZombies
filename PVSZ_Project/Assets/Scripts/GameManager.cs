//#define DEBUG_GAMEMANAGER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum TurnType
{
    Tech,
    Alien
}

[System.Serializable]
public class AlienWithNum
{
    public Zombie alien;
    public int num;
}

public class GameManager : MonoBehaviour
{
    // NOTE(sftl): singleton
    public static GameManager Instance;

    public ResourceManager resourceManager;
    public GridManager gridManager;
    public PlayUI playUI;



    public Preview shooterPrevPrefab;
    public TechUnit shooterPrefab;

    public Preview removePrevPrefab;

    public Zombie alienPrefab; // TODO(sftl): change Class name
    public BaseZombie baseAlienPrefab;
    public SpecialZombie specialAlienPrefab;

    public List<SerializableList<AlienWithNum>> aliensPerLane;


    [SerializeField] public int unitCost;
    public int TurnNum = 1;
    public GameEvent turnIncrementedEvent;

    Preview currPreview;
    TurnType currTurn = TurnType.Tech;
    List<Zombie> aliens = new();
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        StartTechTurn();


    }

    void Update()
    {

        if (Input.GetMouseButtonUp(1)) // NOTE(sftl): right click
        {
            if (currPreview != null)
            {
#if DEBUG_GAMEMANAGER
                Debug.Log("Preview destroyed on right click.");
#endif
                Destroy(currPreview.gameObject);
                currPreview = null;
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
                            tile.Unit = techUnit;

                            //-remove TechPreview
                            //Destroy(currPreview.gameObject);
                            //currPreview = null;
                        }
                    }
                }
                else // NOTE(sftl): remove preview
                {
                    Tile tile = gridManager.GetSelectedTileIfOccupied();

                    if (tile != null)
                    {
                        //-remove TechUnit
                        Destroy(tile.Unit.gameObject);
                        tile.Unit = null;

                        //-remove RemovePreview
                        //Destroy(currPreview.gameObject);
                        //currPreview = null;
                    }
                }
            }
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

    public void StartTechTurn()
    {
        // TODO(sftl): generate next alien wave
        gridManager.SetAlienLaneIndicators();
    }

    public void EndTechTurn()
    {
        currTurn = TurnType.Alien;
        playUI.Dissable();
        SpawnAliens();
    }

    public void EndAlienTurn()
    {
        currTurn = TurnType.Tech;
        TurnNum++;
        StartTechTurn();
        playUI.Enable();
    }

    void SpawnAliens()
    {
        var availablePos = gridManager.GetAvailableSpawnPos();

        //-spawn aliens by specified aliensPerLane
        for (int i = 0; i < aliensPerLane.Count; i++)
        {
            foreach (var alienWithNum in aliensPerLane[i].data)
            {
                for (int i2 = 0; i2 < alienWithNum.num; i2++)
                {
                    var pos = availablePos[i];
                    var alien = Instantiate(alienWithNum.alien, pos, Quaternion.identity);
                    aliens.Add(alien);
                }
            }
        }
    }
    public void OnAlienDeath(Zombie alien)
    {
        aliens.Remove(alien);

        if (aliens.Count == 0)
        {
            EndAlienTurn();
            turnIncrementedEvent.Raise();
        }
    }

    public void DecreaseResources()
    {

    }

}
