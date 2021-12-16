using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUnit : TechPrototype, ITechAbilities
{
    float nextAttack = 0.5f;
    // TO DO: [SerializeField] public float projectileSpeed;
    private void Start()
    {
        distanceToDetect = distanceToDetect == 0 ? 8 : distanceToDetect;


    }



    void Update()
    {
        DetectEnemyShoot();

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
                AudioManager.Instance.Play_ShooterShoot();
            }

        }

    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {

            Destroy(this.gameObject);
            GameManager.Instance.OnTechDeath(this);
        }

    }

}