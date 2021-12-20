using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameObject _parent;
    
    public AlienLaneIndicator indicatorPrefab;
    List<AlienLaneIndicator> indicators = new();
    public GameObject indicatorParent;
    
    public int NumOfLanes { get { return _height; } }
    
    List<List<Tile>> tiles = new();
    Tile selected = null;
    
    private void Awake()
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
        
        GenerateGrid();
    }
    
    void GenerateGrid()
    {
        int width = (int)_tilePrefab.GetComponent<Transform>().localScale.x;
        int height = (int)_tilePrefab.GetComponent<Transform>().localScale.y;
        
        for (int i = 0; i < _width; )
        {
            tiles.Add(new List<Tile>());
            
            for (int j = 0; j < _height; )
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(i, j), Quaternion.identity);
                spawnedTile.Row = j;
                spawnedTile.Column = i;
                spawnedTile.name = $"Tile {i} {j}";
                spawnedTile.transform.parent = _parent.transform;
                var isOffset = (i + j)/(width) % 2 == 1;
                spawnedTile.Init(isOffset);
                tiles[i].Add(spawnedTile);
                j += height;
            }
            i += width;
        }
        
        _parent.transform.position = (new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10))*-1;
        
        SpawnAlienLaneIndicators();
    }
    
    public void TileHover(Tile tile) {
        //-select
        selected = tile;
        
        //-highlight
        var preview = GameManager.Instance.CurrentPreview;
        
        if (preview == null) 
        {
            tile.SetHighlight(true);
        }
        else if (preview.Type != PreviewType.PowerBlock) 
        {
            tile.SetHighlight(true);
        }
        else // NOTE(sftl): highlight lane
        {
            foreach (var column in tiles)
            {
                column[tile.Row].SetHighlight(true);
            }
        }
    }
    
    public void TileHoverExit(Tile tile) {
        // NOTE(sftl): when moving cursor from one tile to the other, it is not defined wheter TileHoverExit or TileHover will be called first?
        if (tile != selected) return;
        
        //-remove highlight
        var preview = GameManager.Instance.CurrentPreview;
        
        if (preview == null) 
        {
            tile.SetHighlight(false);
        }
        else if (preview.Type != PreviewType.PowerBlock) 
        {
            tile.SetHighlight(false);
        }
        else // NOTE(sftl): highlight lane
        {
            foreach (var column in tiles)
            {
                column[tile.Row].SetHighlight(false);
            }
        }
        
        //-deselect
        selected = null;
    }
    
    // NOTE(sftl): player can clear preview while inside grid
    public void PreviewCleared(Preview oldPreview)
    {
        if (selected == null) return; // NOTE(sftl): change doesn't concern grid
        
        if (oldPreview?.Type == PreviewType.PowerBlock)
        {
            //-remove highligh from lane
            foreach (var column in tiles)
            {
                column[selected.Row].SetHighlight(false);
            }
        }
    }
    
    public bool canAlienMove(Vector3 startP,bool isUp)
    {
        float up = startP.y + _tilePrefab.transform.localScale.y;
        float down = startP.y - _tilePrefab.transform.localScale.y;
        var firstTile = tiles[0][0];
        var lastTile = tiles[0].Last();
        var bukizila = _tilePrefab.transform.localScale.y / 2;
        
        if (down < firstTile.transform.position.y-bukizila && !(isUp))
            return false;
        else if (up > lastTile.transform.position.y + bukizila && isUp)
            return false;
        
        return true;
    }
    
    public bool isAlienOutside(Vector3 pos)
    {
        var bukizila = _tilePrefab.transform.localScale.y / 2;
        var firstTile = tiles[0][0];
        if (pos.x < firstTile.transform.position.x - bukizila)
            return true;
        
        return false;
    }
    
    // NOTE(sftl): returns null if none is selected
    public Tile GetSelectedTileIfAvailable()
    {
        return selected;
    }
    
    // NOTE(sftl): returns null if none is selected or tile is not occupied
    public Tile GetSelectedTileIfOccupied()
    {
        return (selected?.Unit is TechUnit) ? selected : null;
    }
    
    public List<Vector3> GetAvailableSpawnPos()
    {
        var r = new List<Vector3>();
        
        // NOTE(sftl): get first row y positions
        var lastRow = tiles.Last();
        foreach (var tile in lastRow)
        {
            r.Add(tile.transform.position);
        }
        
        return r;
    }
    
    // NOTE(sftl): return null if no lane is selected
    public (Vector3, Vector3)? GetSelectedLaneStartEndPos()
    {
        if (selected == null) return null;
        return (tiles.First()[selected.Row].transform.position, tiles.Last()[selected.Row].transform.position);
    }
    
    public float GetNeighbourLaneY(Alien alien)
    {
        var tileSize = _tilePrefab.transform.localScale.y;
        var alienPos = alien.transform.position;
        
        var possibilities = new List<float>();
        if (canAlienMove(alienPos, isUp: true)) possibilities.Add(alienPos.y + tileSize);   // TODO(sftl): local scale is used frequently, should we have a field?
        if (canAlienMove(alienPos, isUp: false)) possibilities.Add(alienPos.y - tileSize);  // NOTE(sftl): if can't move up, Alien must be able to move down
        
        return possibilities[UnityEngine.Random.Range(0, possibilities.Count)];
    }
    
    public void SpawnAlienLaneIndicators()
    {
        var lastRow = tiles.Last();
        
        foreach (var tile in lastRow)
        {
            var curr = Instantiate(indicatorPrefab, tile.transform.position, Quaternion.identity);
            curr.transform.parent = indicatorParent.transform;
            indicators.Add(curr);
        }
    }
    
    public void SetAlienLaneIndicators(List<List<Alien>> nextWave)
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            var laneDifficuly = nextWave[i].Count;
            indicators[i].SetDifficulty(laneDifficuly);
        }
    }
}