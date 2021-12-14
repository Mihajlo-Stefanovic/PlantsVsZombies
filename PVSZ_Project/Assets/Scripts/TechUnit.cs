using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUnit : FieldUnit
{

    [SerializeField] private float distanceToDetect;
    [SerializeField] private float attackSpeed = 1.5f;
    float nextAttack = 0.5f;
    [SerializeField] private Transform reyPos;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private int health = 50;
    
    




private void Start() {
    distanceToDetect= distanceToDetect==0 ? 8: distanceToDetect;
    
}



void Update() {
    
    if(health <= 0)
        {
            
            Destroy(this.gameObject);
        }

    Debug.DrawRay(reyPos.position, reyPos.right * distanceToDetect, Color.red);

    Ray ray = new Ray(reyPos.position, reyPos.right);

    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distanceToDetect, LayerMask.GetMask("Alien"));

    if(hit) {
        
        if(Time.time>nextAttack){

            nextAttack = Time.time+attackSpeed;
            Instantiate(shotPrefab,reyPos.position,Quaternion.identity);

        }
       
        }
}

    public void takeDamage(int damage)
    {
        health -= damage;

    }

}