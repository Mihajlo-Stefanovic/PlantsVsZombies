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
    
    public Tile SelectedTile = null;
    List<List<Tile>> tiles = new();
    
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
        SelectedTile = tile;
        
        //-highlight
        var preview = GameManager.Instance.CurrentPreview;
        
        if (preview == null) 
        {
            tile.SetHighlight(true);
        }
        else if (preview.Type == PreviewType.PowerBlock) // NOTE(sftl): highlight lane 
        {
            foreach (var column in tiles)
            {
                column[tile.Row].SetHighlight(true);
            }
        }
        else if (preview.Type == PreviewType.PowerSlow) // NOTE(sftl): highligh whole grid
        {
            foreach (var column in tiles)
            {
                column.ForEach(action: (Tile t) => { t.SetHighlight(true); });
            }
        }
        else if (preview.Type == PreviewType.PowerShield) // NOTE(sftl): highligh whole grid
        {
            foreach (var column in tiles)
            {
                column.ForEach(action: (Tile t) => { t.SetHighlight(true); });
            }
        }
        else
        {
            tile.SetHighlight(true);
        }
    }
    
    public void TileHoverExit(Tile tile) {
        // NOTE(sftl): when moving cursor from one tile to the other, it is not defined wheter TileHoverExit or TileHover will be called first?
        if (tile != SelectedTile) return;
        
        //-remove highlight
        var preview = GameManager.Instance.CurrentPreview;
        
        if (preview == null) 
        {
            tile.SetHighlight(false);
        }
        else if (preview.Type == PreviewType.PowerBlock)
        {
            foreach (var column in tiles)
            {
                column[tile.Row].SetHighlight(false);
            }
        }
        else if (preview.Type == PreviewType.PowerSlow)
        {
            foreach (var column in tiles)
            {
                column.ForEach(action: (Tile t) => { t.SetHighlight(false); });
            }
        }
        else if (preview.Type == PreviewType.PowerShield)
        {
            foreach (var column in tiles)
            {
                column.ForEach(action: (Tile t) => { t.SetHighlight(false); });
            }
        }
        else
        {
            tile.SetHighlight(false);
        }
        
        //-deselect
        SelectedTile = null;
    }
    
    // NOTE(sftl): player can clear preview while inside grid
    public void PreviewCleared(Preview oldPreview)
    {
        if (SelectedTile == null) return; // NOTE(sftl): change doesn't concern grid
        
        if (oldPreview?.Type == PreviewType.PowerBlock)
        {
            //-remove highligh from lane
            foreach (var column in tiles)
            {
                column[SelectedTile.Row].SetHighlight(false);
            }
        }
        else if (oldPreview?.Type == PreviewType.PowerSlow)
        {
            foreach (var column in tiles)
            {
                column.ForEach(action: (Tile t) => { t.SetHighlight(false); });
            }
        }
        else if (oldPreview?.Type == PreviewType.PowerShield)
        {
            foreach (var column in tiles)
            {
                column.ForEach(action: (Tile t) => { t.SetHighlight(false); });
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
    
    public bool IsAlienInPlayerBase(Vector3 pos)
    {
        var tileW = _tilePrefab.transform.localScale.y;
        var firstTile = tiles[0][0];
        
        if (pos.x < firstTile.transform.position.x - tileW) return true;
        return false;
    }
    
    // NOTE(sftl): returns null if none is selected
    public Tile GetSelectedTileIfAvailable()
    {
        return SelectedTile;
    }
    
    // NOTE(sftl): returns null if none is selected or tile is not occupied
    public Tile GetSelectedTileIfOccupied()
    {
        return (SelectedTile?.Unit is TechPrototype) ? SelectedTile : null;
    }
    
    public List<Vector3> GetAvailableSpawnPos()
    {
        var r = new List<Vector3>();
        var offset = new Vector3(_tilePrefab.transform.localScale.x * 2f, 0f, 0f);
        
        // NOTE(sftl): get first row y positions
        var lastRow = tiles.Last();
        foreach (var tile in lastRow)
        {
            r.Add(tile.transform.position + offset);
        }
        
        return r;
    }
    
    // NOTE(sftl): return null if no lane is selected
    public (Vector3, Vector3)? GetSelectedLaneStartEndPos()
    {
        if (SelectedTile == null) return null;
        return (tiles.First()[SelectedTile.Row].transform.position, tiles.Last()[SelectedTile.Row].transform.position);
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
            curr.transform.SetParent(indicatorParent.transform);
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