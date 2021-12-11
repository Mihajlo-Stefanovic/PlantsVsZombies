using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechPreview : MonoBehaviour
{
    public float moveSpeed;
    
    Vector3 mousePosition;
    
    void Update () {
        mousePosition   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // NOTE(sftl): in front of camera
        
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }
}