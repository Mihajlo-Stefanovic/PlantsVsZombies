using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUnit : TechUnit
{
    [SerializeField] private int _resourcesToGet;
    private int currResources;

    public void IncreaseRescources()
    {
        currResources = ResourceManager.getResources();
        currResources += _resourcesToGet;
        // Debug.Log("ovoliko resursa" + currResources);
        GameManager.Instance.resourceManager.setResources(currResources);
    }
}
