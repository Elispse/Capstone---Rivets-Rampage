using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float attackRate = 1;
    [SerializeField] protected string tagName;

    public abstract bool Use();
    public abstract void Attack(Animator animator);
}
