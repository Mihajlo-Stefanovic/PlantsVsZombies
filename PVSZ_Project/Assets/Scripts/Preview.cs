using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PreviewType
{
    Tech,       // TODO(sftl): rename to shooter
    PowerScan,
    Remove,
    ResourceUnit,
    MachineGun
}

public class Preview : MonoBehaviour
{
    public float moveSpeed;
    public PreviewType type;

    Vector3 mousePosition;

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // NOTE(sftl): in front of camera

        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }
}