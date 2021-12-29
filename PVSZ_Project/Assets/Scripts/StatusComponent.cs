using System;
using UnityEngine;

public class StatusComponent : MonoBehaviour
{
    [SerializeField] private GameObject _shield;

    [SerializeField] private bool _hasShield;
    public bool HasShield
    {
        get { return _hasShield; }
        set { 
            _hasShield = value;
            _shield.SetActive(value);
        }
    }

    public void Clear()
    {
        HasShield = false;
    }
}