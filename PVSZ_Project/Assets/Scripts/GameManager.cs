//#define DEBUG_GAMEMANAGER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // NOTE(sftl): singleton
    public static GameManager Instance;
    
    public GridManager gridManager;
    
    public TechPreview  shooterPrevPrefab;
    public TechUnit     shooterPrefab;
    
    TechPreview currPreview;
    
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
                Debug.Log("Tech Preview destroyed on right click.");
#endif
                Destroy(currPreview.gameObject);
                currPreview = null;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currPreview != null) 
            {
                Tile tile = gridManager.GetSelectedTileIfAvailable();
                
                if (tile != null)
                {
                    //-instantiate TechUnit
                    var pos = tile.transform.position;
                    var techUnit = Instantiate(shooterPrefab, pos, Quaternion.identity);
                    tile.Unit = techUnit;
                    
                    //-remove TechPreview
                    Destroy(currPreview.gameObject);
                    currPreview = null;
                }
            }
        }
    }
    
    public void TechCardClicked(TechCard card)
    {
        var prevPreview = currPreview;
        
        var pos         = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currPreview     = Instantiate(shooterPrevPrefab, pos, Quaternion.identity);
        
        if (prevPreview != null) 
        {
#if DEBUG_GAMEMANAGER
            Debug.Log("Tech Preview destroyed since new one is initialized.");
#endif
            Destroy(prevPreview.gameObject);
        }
    }
}
