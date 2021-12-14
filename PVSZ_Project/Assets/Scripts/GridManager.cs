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
    
    public AlienLaneIndicator   indicatorPrefab;
    List<AlienLaneIndicator>    indicators = new();
    
    List<List<Tile>> tiles = new();
    public static GridManager Instance;
    
    
    
    private void Awake()
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
    
    public void SpawnAlienLaneIndicators()
    {
        var lastRow = tiles.Last();
        
        foreach (var tile in lastRow)
        {
            indicators.Add(Instantiate(indicatorPrefab, tile.transform.position, Quaternion.identity));
        }
    }
    
    public void SetAlienLaneIndicators()
    {
        // NOTE(sftl): temp
        var aliensPerLane = GameManager.Instance.aliensPerLane;
        
        for (int i = 0; i < indicators.Count; i++)
        {
            var laneDifficuly = aliensPerLane[i].data.Sum(item => item.num); // NOTE(sftl): num of aliens in lane
            indicators[i].SetDifficulty(laneDifficuly);
        }
    }
}