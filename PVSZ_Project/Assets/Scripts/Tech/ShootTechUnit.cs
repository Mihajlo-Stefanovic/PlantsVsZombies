using UnityEngine;

public class ShootTechUnit : TechUnit
{
    [SerializeField] protected ShootComponent _shootComponent;
    
    protected override void Awake()
    {
        base.Awake();
        _shootComponent.Unit = this;
    }
}