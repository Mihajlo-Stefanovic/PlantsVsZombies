using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _camera;
    
    // TODO(sftl): probably should have them accessible quickly by row/column
    List<Tile> tiles = new();
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }
    
    void GenerateGrid()
    {
        int width = (int)_tilePrefab.GetComponent<Transform>().localScale.x;
        int height = (int)_tilePrefab.GetComponent<Transform>().localScale.y;
        
        for (int i = 0; i < _width; )
        {
            for (int j = 0; j < _height; )
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(i, j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";
                var isOffset = (i + j)/(width) % 2 == 1;
                spawnedTile.Init(isOffset);
                tiles.Add(spawnedTile);
                j += height;
            }
            i += width;
        }
        
        
        
        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        
        
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Tile GetSelectedTileIfAvailable()
    {
        // TODO(sftl): optimise
        foreach(var tile in tiles)
        {
            if (tile.IsSelected) 
            {
                if (tile.Unit == null)  return tile;
                else                    return null;
            }
        }
        return null;
    }
}
