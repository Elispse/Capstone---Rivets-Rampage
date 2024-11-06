using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    private WeaponBase weapon;
    private WeaponParent weaponParent;

    private void Start()
    {
        weapons[0] = GetComponentInChildren<WeaponBase>().gameObject;
        weapon = weapons[0].GetComponent<WeaponBase>();
    }
    public void Attack()
    {
        weapon.Use();
    }

    public void SwapWeapon()
    {

    }
}