using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons;
    private WeaponBase weapon;
    [SerializeField]
    private GameObject weaponParent;
    private bool held;

    private void Start()
    {
        foreach (Transform child in weaponParent.transform)
        {
            weapons.Add(child.gameObject);
        }
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapon = weapons[0].GetComponent<WeaponBase>();
        weapon.gameObject.SetActive(true);
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
        }
        if (context.canceled)
        {
            held = false;
        }
    }

    public void SwapWeapon()
    {

    }
}