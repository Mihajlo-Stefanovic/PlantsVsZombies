using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PreviewType
{
    Tech,       // TODO(sftl): rename to shooter
    PowerScan,
    Remove,
    ResourceUnit,
    MachineGun,
    PowerBlock,
    PowerSlow,
    PowerShield
}

public class Preview : MonoBehaviour
{
    float moveSpeed = 50f;
    public PreviewType Type;
    
    Vector3 mousePosition;
    
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // NOTE(sftl): in front of camera
        
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }
}