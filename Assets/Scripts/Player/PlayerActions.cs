using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour, IDamagable, IScoreable
{
    [SerializeField] float Health;
    [SerializeField] WeaponBase weapon;
    [SerializeField] public WeaponBase[] weapons;
    [SerializeField] protected Animator animator;
    private int currentWeapon = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Health = 6;
    }

    public void ApplyDamage(float damage)
    {
        Health -= damage;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        //weapon.Use(animator);
        weapon.Attack();
        //animator.SetBool("hasAttacked", true);
    }

    public void SwapWeapons(InputAction.CallbackContext context)
    {
        if (currentWeapon == 0)
        {
            currentWeapon = 1;
        }
        else
        {
            currentWeapon = 0;
        }
        weapon = weapons[currentWeapon];
    }

    public void AddScore(int score)
    {
        //
    }
}