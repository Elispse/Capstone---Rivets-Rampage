using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons;
    [SerializeField] private GameObject weaponParent;
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private BoolEvent fireWeaponEvent;
    [SerializeField] private WeaponUI weaponUI;
    public bool held { get; set; }
    private int weaponNum = 0;
    private GameObject weaponMagSize;
    private GameObject weaponCountSize;

    private void Start()
    {
        foreach (Transform child in weaponParent.transform)
        {
            weapons.Add(child.gameObject);
        }
        foreach (GameObject weapon in weapons)
        {
            weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        weapon = weapons[0].GetComponent<WeaponBase>();
        weapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;

        //update WeaponUI
        weaponUI.UpdateInfo(weapon.GetComponent<SpriteRenderer>().sprite, weapon.GetComponent<WeaponBase>().magCapacity, weapon.GetComponent<WeaponBase>().ammoCount);
    }

    private void Update()
    {
        if (held)
        {
            weapon.Use();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            held = true;
            fireWeaponEvent.RaiseEvent(true);
        }
        if (context.canceled)
        {
            held = false;
        }
    }

    public void SwapWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            weaponNum = weaponNum + 1;
            if (weaponNum > weapons.Count - 1)
            {
                weaponNum = 0;
            }
            weapon = weapons[weaponNum].GetComponent<WeaponBase>();
            weaponParent.GetComponent<WeaponParent>().weaponRenderer = weapon.gameObject.GetComponent<SpriteRenderer>();
            weapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;

            //update WeaponUI
            weaponUI.UpdateInfo(weapon.GetComponent<SpriteRenderer>().sprite, weapon.GetComponent<WeaponBase>().magCapacity, weapon.GetComponent<WeaponBase>().ammoCount);
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weapon.StartCoroutine(weapon.ReloadCR());
        }
    }
}