using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUnit : FieldUnit
{

    float distanceToDetect;
    float attackSpeed = 0.5f;
    float nextAttack = 0.5f;
    [SerializeField] private Transform reyPos;
    [SerializeField] private GameObject shotPrefab;
    
    

private void Start() {
    distanceToDetect = 8;
    
}


void Update() {
    
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

}