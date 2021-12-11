using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioManager : MonoBehaviour
{
    private float targetAspect = 16f / 9f;
    
    private int lastW = 0;
    private int lastH = 0;
    
    void Update()
    {
        int currW = Screen.width;
        int currH = Screen.height;
        
        // TODO(sftl): optimise, don't check this every fame if possible
        if(currW != lastW || currH != lastH)
        {
            Debug.Log("Screen size changed: current width " + currW.ToString() + " current height " + currH.ToString());
            
            SetAspectRatio();
            lastW = currW;
            lastH = currH;
        }
    }
    
    private void SetAspectRatio()
    {
        // NOTE(sftl): reference http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
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
}
