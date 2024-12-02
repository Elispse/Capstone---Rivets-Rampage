using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public enum WeaponType
    {
        Melee,
        Ranged
    }
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float attackRate = 1;
    [SerializeField] protected Animator WeaponAnimator;
    [SerializeField] protected string animationTriggerName;
    [SerializeField] protected string tagName;
    [SerializeField] protected LayerMask layerMask = Physics.AllLayers;
    [SerializeField] protected int reloadTime;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public int magCapacity;
    [SerializeField] public int ammoCount;

    public Vector2 PointerPosition { get; set; }
    protected WeaponUI weaponUI;

    protected bool ready = true;
    public abstract bool Use();
    public abstract void Attack();
    public abstract IEnumerator ReloadCR();
}