using UnityEngine;

public class PowerScan : TechPrototype, ITechAbilities
{
    public PowerScanBullet bulletPrefab;
    
    public float rotationSpeed;
    public float maxSideAngle;
    public float minDistance; // TODO(sftl): handle shooting zombies that are right in front, this was used as a quick fix and should maybe be removed
    
    bool isRotatingRight = true;
    float angle = 0f;
    
    Vector2 target;
    bool hasTarget = false;
    
    float nextAttack = 0f;
    
    void FixedUpdate()
    {
        Debug.DrawRay(reyPos.position, reyPos.right * distanceToDetect, Color.red);
        if (Time.time > nextAttack)
        {
            Ray ray = new Ray(reyPos.position, reyPos.right); // NOTE(sftl): this object is rotating, rey position is its child
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distanceToDetect, LayerMask.GetMask("Alien"));
            
            if (hit && hit.distance > minDistance)
            {
                target = hit.point;
                hasTarget = true;
            }
        }
    }
    
    void Update()
    {
        //-shoot
        if (hasTarget == true && Time.time > nextAttack)
        {
            var bullet = Instantiate(bulletPrefab, reyPos.transform.position, Quaternion.identity);
            bullet.GoTowards(target);
            
            hasTarget = false;
            nextAttack = Time.time + attackDelay;
        }
        
        //-calc rotation
        if (isRotatingRight)
        {
            angle += rotationSpeed * Time.deltaTime;
            if (angle > maxSideAngle)
            {
                isRotatingRight = false;
            }
        }
        else 
        {
            // NOTE(sftl): how do negative angles work? is this correct?
            angle -= rotationSpeed * Time.deltaTime;
            if (angle < -maxSideAngle)
            {
                isRotatingRight = true;
            }
        }
        
        //-rotate
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Destroy(gameObject);
            GameManager.Instance.OnTechDeath(this);
        }
    }
}