using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    
    void Start()
    {
        // SetAspectRatio();
        
        var rEdge   = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));      // NOTE(sftl): middle position on the right edge of the screen
        var pos     = new Vector3(rEdge.x, rEdge.y, 0);
        Instantiate(zombiePrefab, pos, Quaternion.identity);
    }
    
    private void SetAspectRatio()
    {
        // NOTE(sftl): reference http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
        float targetAspect = 4f / 3f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        Camera camera = Camera.main;
        
        // if scaled height is less than current height, add letterbox
        if (scaleHeight < 1.0f)
        {  
            Rect rect = camera.rect;
            
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scaleWidth = 1.0f / scaleHeight;
            
            Rect rect = camera.rect;
            
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            
            camera.rect = rect;
        }
    }
    
    // TODO(sftl): should we add a second camera to render pillar/letterbox
}
