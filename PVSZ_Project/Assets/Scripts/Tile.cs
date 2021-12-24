using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    
    [SerializeField] private SpriteRenderer _renderer;
    
    [SerializeField] private GameObject _highlight;
    
    public int Column;
    public int Row;
    
    public TechPrototype Unit;
    
    public void Init(bool isOffset)
    {
        _baseColor.a = 255;
        _offsetColor.a = 255;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
    
    public void OnPointerRayEnter()
    {
        GameManager.Instance.gridManager.TileHover(this);
    }
    
    public void OnPointerRayExit()
    {
        GameManager.Instance.gridManager.TileHoverExit(this);
    }
    
    public void SetHighlight(bool isHighlighted)
    {
        _highlight.SetActive(isHighlighted);
    }
}
