using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _camera;


    



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
                var isOffset = (i + j)/(width+height) % 2 == 1;
                spawnedTile.Init(isOffset);
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
}
