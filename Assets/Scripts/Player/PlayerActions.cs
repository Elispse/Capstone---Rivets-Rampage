using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour, IDamagable
{
    [SerializeField] WeaponBase weapon;
    [SerializeField] protected Animator animator;

    public void ApplyDamage(float damage)
    {

    }

    public void Fire(InputAction.CallbackContext context)
    {
        //weapon.Use(animator);
        weapon.Attack();
        //animator.SetBool("hasAttacked", true);
    }
}