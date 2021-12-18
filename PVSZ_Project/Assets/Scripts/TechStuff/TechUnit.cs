using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUnit : TechPrototype, ITechAbilities
{
    public Animator animator;
    public int damage;
    float nextAttack = 0f;
    // TO DO: [SerializeField] public float projectileSpeed;
    private void Start()
    {
        distanceToDetect = distanceToDetect == 0 ? 8 : distanceToDetect;

    }

    void Update() // TODO(sftl): raycast on fixed update
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
                animator.SetFloat("Shoot", 1);
                AudioManager.Instance.Play_ShooterShoot();
            }

        }
            else    animator.SetFloat("Shoot", 0);
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