using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMovement : MonoBehaviour
{

    float timer;
    // TO DO:public float projectileSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * 5);

        timer += Time.deltaTime;
        if (timer > 2)
        {
            Destroy(this.gameObject);
        }
    }

    // public void SetProjectileSpeed(float projectileSpeed)
    // {

    //     this.projectileSpeed = projectileSpeed;

    // } TO DO!
}