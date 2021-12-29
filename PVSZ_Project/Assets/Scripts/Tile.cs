using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    
    [SerializeField] private SpriteRenderer _renderer;
    
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _blockedIndicator;
    
    public int Column;
    public int Row;
    
    private bool   _isBlocked;
    public bool     IsBlocked 
    {
        get { return _isBlocked; }
        set { 
            _isBlocked = value;
            _blockedIndicator.SetActive(value);
        }
    }
    
    public TechUnit Unit;
    
    public void Init(bool isOffset)
    {
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
