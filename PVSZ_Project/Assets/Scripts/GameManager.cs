//#define DEBUG_GAMEMANAGER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TurnType
{
    Tech,
    Alien
}

public class GameManager : MonoBehaviour
{
    // NOTE(sftl): singleton
    public static GameManager Instance;
    
    public GridManager gridManager;
    public EndTurn endTurn;
    
    public Preview shooterPrevPrefab;
    public TechUnit shooterPrefab;
    
    public Preview removePrevPrefab;
    
    public Zombie alienPrefab; // TODO(sftl): change Class name
    
    Preview currPreview;
    TurnType currTurn = TurnType.Tech;
    
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
                        //-instantiate TechUnit
                        var pos = tile.transform.position;
                        var techUnit = Instantiate(shooterPrefab, pos, Quaternion.identity);
                        tile.Unit = techUnit;
                        
                        //-remove TechPreview
                        //Destroy(currPreview.gameObject);
                        //currPreview = null;
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
    
    public void EndTechTurn()
    {
        currTurn = TurnType.Alien;
        endTurn.Dissable();
        SpawnAliens();
        
        // NOTE(sftl): temp
        StartCoroutine(EndAlienTurnDelayed());
    }
    
    // NOTE(sftl): temp
    List<Zombie> aliens = new();
    
    IEnumerator EndAlienTurnDelayed()
    {
        yield return new WaitForSeconds(5f);
        
        foreach (var alien in aliens) Destroy(alien.gameObject);
        aliens.Clear();
        
        EndAlienTurn();
    }
    
    void EndAlienTurn()
    {
        currTurn = TurnType.Tech;
        endTurn.Enable();
    }
    
    void SpawnAliens()
    {
        var numOfAliens     = 5;
        var availablePos    = gridManager.GetAvailableSpawnPos();
        
        for (int i = 0; i < numOfAliens; i++)
        {
            var pos     = availablePos[Random.Range(0, availablePos.Count)]; // NOTE(sftl): +1 to include maxYPos in range
            var alien   = Instantiate(alienPrefab, pos, Quaternion.identity);
            aliens.Add(alien);
        }
    }
}
