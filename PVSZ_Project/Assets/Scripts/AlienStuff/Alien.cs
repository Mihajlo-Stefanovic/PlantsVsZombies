using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Animator animator;
    public int Difficulty;
    public int speed;
    public int damage;
    private bool _lockDirection = false;
    public Tile _tile;
    protected GridManager gridManager;
    private int _random = 1;
    protected Vector3 _startVar;
    private int _lastmove = 1;
    private bool _stopMoving = false;
    private GameObject _techUnitToDamage;
    private float nextAttack = 0f;
    [SerializeField] protected int health;
    void Start() { } //override in child
    void Update()
    {
        IsTechDead();
    }
    public void IsTechDead()
    {
        if (_techUnitToDamage == null)
        {   _stopMoving = false;
             
        }
        
    }
    
    public void KillOutside()
    {
        if (gridManager.isAlienOutside(gameObject.transform.position))
        {
            StartCoroutine(KillAfterSec(2f));
        }
    }
    
    public IEnumerator KillAfterSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(this.gameObject);
        GameManager.Instance.OnAlienDeath(this);
        
    }
    
    // NOTE(sftl): used when Power Block is casted
    public void MoveToNeightourLane()
    {
        var newY = GameManager.Instance.gridManager.GetNeighbourLaneY(this);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z); // TODO(sftl): move smoothly
    }
    
    protected void isDead()
    {
        
        if (health <= 0)
        {
            
            Destroy(this.gameObject);
            GameManager.Instance.OnAlienDeath(this);
        }
       
        
    }
    
    protected void CheckEverything()
    {
        KillOutside();
        IsTechDead();
        isDead();
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            
            damage = GameManager.Instance.shooterPrefab.damage;
            health -= damage;
            Destroy(col.gameObject); // TODO(sftl): destroy in bullet, not here
        }
        else if (col.gameObject.CompareTag("PowerScanBullet"))
        {
            health -= 50;
        }
        else if (col.gameObject.CompareTag("TechUnit"))
        {
            _stopMoving = true;
            _techUnitToDamage = col.gameObject;
        }
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        
        
        if (col.gameObject.CompareTag("TechUnit"))
        {
            if (Time.time > nextAttack)
            {
                
                AudioManager.Instance.Play_AlienMelee();
                nextAttack = Time.time + 0.5f;
                col.gameObject.GetComponent<ITechAbilities>().takeDamage(20);
               
            }
            
        
        }
    }
    
    protected void MoveIt(int move)
    {
        if (_stopMoving)
            return;
        
        switch (move)
        {
            case 1:
            moveLeft();
            break;
            
            case 0:
            if (gridManager.canAlienMove(_startVar, true))
                moveUp();
            break;
            
            case 2:
            if (gridManager.canAlienMove(_startVar, false))
                moveDown();
            break;
            
            
        }
    }
    
    protected void moonWalk()
    {
        if (!(_lockDirection))
            if (_lastmove == 1)
            _random = Random.Range(0, 3);
        else if (_lastmove == 0)
            _random = Random.Range(0, 2);
        else if (_lastmove == 2)
            _random = Random.Range(1, 3);
        
        MoveIt(_random);
    }
    
    protected void moveLeft()
    {
        
        if (_stopMoving)
            return;
        
        _lockDirection = true;
        
        
        if (Mathf.Abs(transform.position.x - _startVar.x) > _tile.GetComponent<Transform>().localScale.x)
        {
            
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
