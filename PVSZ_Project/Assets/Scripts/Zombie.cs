using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(-speed * Time.deltaTime, 0f, 0f); // NOTE(sftl): moving left
    }
}
