using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _parent;
    
    public AlienLaneIndicator indicatorPrefab;
    List<AlienLaneIndicator> indicators = new();
    
    List<List<Tile>> tiles = new();
    public static GridManager Instance;
    
    public int NumOfLanes { get { return _height; } }
    
    
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
    
    // Start is called before the first frame update
    void Start()
    {
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Tile GetSelectedTileIfAvailable()
    {
        // TODO(sftl): optimise
        foreach(var row in tiles)
        {
            foreach(var tile in row)
            {
                if (tile.IsSelected) 
                {
                    if (tile.Unit == null)  return tile;
                    else                    return null;
                }
            }
        }
        return null;
    }
    
    public Tile GetSelectedTileIfOccupied()
    {
        // TODO(sftl): optimise
        foreach(var row in tiles)
        {
            foreach(var tile in row)
            {
                if (tile.IsSelected) 
                {
                    if (tile.Unit is TechUnit)  return tile;
                    else                        return null;
                }
            }
        }
        return null;
    }
    
    public void DeselectTile()
    {
        // TODO(sftl): optimise
        foreach(var row in tiles)
        {
            foreach(var tile in row)
            {
                if (tile.IsSelected) 
                {
                    tile.Deselect();
                }
            }
        }
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
        // TODO(sftl): optimise
        int tileIndex = -1;
        for (int i = 0; i < tiles.Count; i++)
        {   
            for (int j = 0; j < tiles[i].Count; j++)
            {
                if (tiles[i][j].IsSelected)
                {
                    tileIndex = j;
                }
            }
        }
        
        if (tileIndex == -1) return null;
        
        var start = tiles.First()[tileIndex].transform.position;
        var end = tiles.Last()[tileIndex].transform.position;
        return (start, end);
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
            indicators.Add(Instantiate(indicatorPrefab, tile.transform.position, Quaternion.identity));
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