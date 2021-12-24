using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechResourceUnit : TechPrototype, ITechAbilities
{
    [SerializeField] private int resourcesToGet;
    private int currResources = ResourceManager.getResources();
    
    void Update()
    {
        CheckShield();
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
    
    public void IncreaseRescources()
    {
        currResources = ResourceManager.getResources();
        currResources += resourcesToGet;
        //Debug.Log("ovoliko resursa" + currResources);
        GameManager.Instance.resourceManager.setResources(currResources);
    }
}
