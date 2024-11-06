using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float attackRate = 1;
    [SerializeField] protected Animator WeaponAnimator;
    [SerializeField] protected string animationTriggerName;
    [SerializeField] protected string tagName;
    [SerializeField] protected LayerMask layerMask = Physics.AllLayers;
    [SerializeField] protected enum WeaponType
    {
        Melee,
        Ranged
    }

    public Vector2 PointerPosition { get; set; }

    protected bool ready = true;

    public abstract bool Use();
    public abstract void Attack();
}
