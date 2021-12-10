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

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(i, j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";

                var isOffset = (i + j) % 2 == 1;
                spawnedTile.Init(isOffset);
            }
        }



        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);


    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
