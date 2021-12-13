using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Zombie : MonoBehaviour
{
    public int speed;
    private bool _lockDirection = false;
    public Tile _tile;
    protected GridManager gridManager;
    private int _random = 1;
    protected Vector3 _startVar;
    private int _lastmove = 1;
    [SerializeField] protected int health;
    void Start(){ } //override in child
    void Update(){ } //override in child
    
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= 30;
            Destroy(collision.gameObject);
            
            
        }
    }
    private void LeftOrDown()
    {
        int a = Random.Range(0, 10);
        if (a % 2 == 0)
            moveDown();
        else
            moveLeft();
    }
    private void LeftOrUp()
    {
        int a = Random.Range(0, 10);
        if (a % 2 == 0)
            moveUp();
        else
            moveLeft();
    }
    
    private void MoveIt(int move)
    {
        
        switch (move)
        {
            case 1:
            moveLeft();
            break;
            
            case 0:
              if(gridManager.canAlienMove(_startVar, true))
                moveUp();
            break;
            
            case 2:
              if (gridManager.canAlienMove(_startVar, false))
                moveDown();
            break;
            
            
        } 
    }
    
    protected void isDead(){

        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameManager.Instance.OnAlienDeath(this);
        }
    
    }

    protected void moonWalk(){
        if(!( _lockDirection))
            if(_lastmove==1)
            _random = Random.Range(0, 3);
        else if (_lastmove == 0)
            _random = Random.Range(0, 2);
        else if(_lastmove == 2)
            _random = Random.Range(1, 3);
        
        
        
        MoveIt(_random);
    }


    protected void moveLeft()
    {
        _lockDirection = true;
        
        
        
        if (Mathf.Abs(transform.position.x - _startVar.x) > _tile.GetComponent<Transform>().localScale.x) {
            
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
        
        if (Mathf.Abs(transform.position.y - _startVar.y) > _tile.GetComponent<Transform>().localScale.y)
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
        
        if (Mathf.Abs(_startVar.y - transform.position.y) > _tile.GetComponent<Transform>().localScale.y)
        {
            _lastmove = 2;
            _lockDirection = false;
            _startVar = transform.position;
            
        }
        else
            transform.position = transform.position + new Vector3(0f, -speed * Time.deltaTime, 0f);
    }
    
    
}
