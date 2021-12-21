using UnityEngine;

public abstract class TechPrototype : MonoBehaviour
{
    [SerializeField] public float distanceToDetect;
    [SerializeField] public float attackDelay;
    [SerializeField] public Transform reyPos;
    [SerializeField] public GameObject shotPrefab;
    [SerializeField] public int health;
    
    public GameObject ShieldSprite;
    
    public bool HasShield;
    public float RemoveShieldTime;
    
    public void AddShieldForSec(float sec)
    {
        HasShield = true;
        ShieldSprite.SetActive(true);
        RemoveShieldTime = Time.time + sec;
    }
    
    // NOTE(sftl): call every update
    public void CheckShield()
    {
        if (Time.time > RemoveShieldTime) 
        {
            HasShield = false;
            ShieldSprite.SetActive(false);
        }
    }
}