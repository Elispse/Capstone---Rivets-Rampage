using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float attackRate = 1;
    [SerializeField] protected string animationTriggerName;
    [SerializeField] protected string tagName;
    [SerializeField] protected LayerMask layerMask = Physics.AllLayers;

    protected bool ready = true;

    public abstract bool Use(Animator animator);
    public abstract void Attack();
}
