using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int speed;
    private bool _lockDirection = false;
    private bool _upDown = false;
    public Tile _tile;
    public GameObject gridManager;
    private int _random = 1;
    private Vector3 _startVar;
    private int _lastmove = 1;
    void Start()
    {
        _startVar = transform.position;
        Debug.Log(_startVar);
    }

    void Update()
    {
        if(!( _lockDirection))
            if(_lastmove==1)
                _random = Random.Range(0, 3);
            else if (_lastmove == 0)
                _random = Random.Range(0, 2);
            else if(_lastmove == 2)
                _random = Random.Range(1, 3);



        MoveIt(_random);
    }



    private void MoveIt(int move)
    {
        switch (move)
        {
           case 1:
                moveLeft();
                break;

           case 0:
                moveUp();
                break;

            case 2:
                moveDown();
                break;

        } 
    }

    private void moveLeft()
    {
        _lockDirection = true;
        
       
        
        if ((int)Mathf.Abs(transform.position.x - _startVar.x) == _tile.GetComponent<Transform>().localScale.x) {

            _lastmove = 1;
            _lockDirection = false;
            _startVar = transform.position;
            
        }
        else
            transform.position = transform.position + new Vector3(-speed * Time.deltaTime, 0f, 0f);
    }
    private void moveUp()
    {
        _lockDirection = true;
        
        if ((int)Mathf.Abs(transform.position.y - _startVar.y) == _tile.GetComponent<Transform>().localScale.y)
        {
            _lastmove = 0;
            _lockDirection = false;
            _startVar = transform.position;
            
        }
        else
            transform.position = transform.position + new Vector3(0f, speed * Time.deltaTime, 0f);


    }
    private void moveDown()
    {
        
        _lockDirection = true;
        
        if ((int)Mathf.Abs(_startVar.y - transform.position.y) == _tile.GetComponent<Transform>().localScale.y)
        {
            _lastmove = 2;
            _lockDirection = false;
            _startVar = transform.position;

        }
        else
            transform.position = transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
    }
}
