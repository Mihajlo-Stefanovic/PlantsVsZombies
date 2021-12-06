using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject zombiePrefab;

    void Start()
    {
        var rEdge   = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));      // NOTE(sftl): middle position on the right edge of the screen
        var pos     = new Vector3(rEdge.x, rEdge.y, 0);
        Instantiate(zombiePrefab, pos, Quaternion.identity);
    }
}
