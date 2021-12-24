using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechMachineGun : TechPrototype, ITechAbilities
{
    //public Animator animator;
    public int damage;
    float nextAttack = 0f;
    
    float secondAttack = 0.0f;
    
    float miniDelay = 0.1f;
    // TO DO: [SerializeField] public float projectileSpeed;
    private void Start()
    {
        distanceToDetect = distanceToDetect == 0 ? 8 : distanceToDetect;
        
        
    }
    
    
    
    void Update() // TODO(sftl): raycast on fixed update
    {
        DetectEnemyShoot();
        CheckShield();
    }
    
    public void DetectEnemyShoot()
    {
        Debug.DrawRay(reyPos.position, reyPos.right * distanceToDetect, Color.red);
        Ray ray = new Ray(reyPos.position, reyPos.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distanceToDetect, LayerMask.GetMask("Alien"));
        
        if (hit)
        {
            if (Time.time > nextAttack)
            {
                // NOTE(sftl): shooting
                nextAttack = Time.time + attackDelay;
                Instantiate(shotPrefab, reyPos.position, Quaternion.identity);
                StartCoroutine(ShootAfterSec(0.05f));
                StartCoroutine(ShootAfterSec(0.15f));
                //animator.SetFloat("Shoot", nextAttack);
                AudioManager.Instance.Play_ShooterShoot();
            }
            // Drugi nacin bez korutine ali ne radi za prvi shot
            // if(Time.time > secondAttack + miniDelay){
            //     secondAttack=nextAttack;
            //     Instantiate(shotPrefab, reyPos.position, Quaternion.identity);
            // }
            
        }
        
    }
    
    
    public IEnumerator ShootAfterSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        Instantiate(shotPrefab, reyPos.position, Quaternion.identity);
        
        
    }
    
    public void takeDamage(int damage)
    {
        if (HasShield) return;
        health -= damage;
        if (health <= 0)
        {
            
            Destroy(this.gameObject);
            GameManager.Instance.OnTechDeath(this);
        }
        
    }
    
}