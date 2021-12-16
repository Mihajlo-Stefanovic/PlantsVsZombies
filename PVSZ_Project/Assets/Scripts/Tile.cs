using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    
    [SerializeField] private SpriteRenderer _renderer;
    
    [SerializeField] private GameObject _highlight;
    
    public FieldUnit Unit;
    public bool IsSelected;
    
    public void Init(bool isOffset)
    {
        _baseColor.a = 255;
        _offsetColor.a = 255;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        
        
        
        
    }
    
    
    public void OnPointerRayEnter()
    {
        _highlight.SetActive(true);
        IsSelected = true;
    }
    
    public void OnPointerRayExit()
    {
        Deselect();
    }
    
    public void Deselect()
    {
        _highlight.SetActive(false);
        IsSelected = false;
    }
}
