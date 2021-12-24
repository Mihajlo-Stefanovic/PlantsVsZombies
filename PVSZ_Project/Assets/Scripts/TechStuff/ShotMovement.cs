using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMovement : MonoBehaviour
{
    public float speed;
    float timer;
    
    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2) Destroy(this.gameObject);
    }
}
