using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] public  List<GameObject> weapons;
    [SerializeField] private GameObject weaponParent;
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private BoolEvent fireWeaponEvent;
    [SerializeField] private WeaponUI weaponUI;
    [SerializeField] private PlayerStateVariable playerStateVariable;
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
        if (held & Time.timeScale != 0)
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

    public void GameLoaded()
    {
        // Clear existing weapons
        foreach (Transform child in weaponParent.transform)
        {
            Destroy(child.gameObject);
        }
        weapons.Clear();

        // Load saved weapons
        foreach (string weaponName in playerStateVariable.value.weapons)
        {
            GameObject weaponPrefab = Resources.Load<GameObject>(weaponName);
            if (weaponPrefab != null)
            {
                GameObject weaponInstance = Instantiate(weaponPrefab, weaponParent.transform);
                weapons.Add(weaponInstance);
            }
        }

        // Update UI and current weapon
        if (weapons.Count > 0)
        {
            weapon = weapons[0].GetComponent<WeaponBase>();
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