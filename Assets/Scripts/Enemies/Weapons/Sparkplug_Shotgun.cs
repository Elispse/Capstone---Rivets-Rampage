using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparkplug_Shotgun : WeaponBase
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField, Range(1, 50)] int fireForce = 20;
    private GameObject bullet;
    private float weaponSpeed;
    private bool isReload = false;

    private void Awake()
    {
        ammoCount = magCapacity;
    }

    public override void Attack()
    {
        Vector3 attackDirection = Vector3.zero;
    }

    public override bool Use()
    {
        WeaponAnimator.speed = weaponSpeed;
        bool used = false;
        if (ready && ammoCount != 0 && !isReload)
        {
            if (WeaponAnimator != null && animationTriggerName != "")
            {
                WeaponAnimator.speed = WeaponAnimator.speed / attackRate;
                WeaponAnimator.SetTrigger(animationTriggerName);
                ready = false;
                StartCoroutine(ResetAttackReadyCR(attackRate));
                Attack();
                used = true;
            }
        }
        else if (ammoCount == 0 && !isReload)
        {
            StartCoroutine(ReloadCR());
        }
        return used;
    }

    public override IEnumerator ReloadCR()
    {
        isReload = true;
        yield return new WaitForSeconds(reloadTime);
        ammoCount = magCapacity;
        isReload = false;
        weaponUI.UpdateAmmoCount(ammoCount);
    }

    IEnumerator ResetAttackReadyCR(float time)
    {
        yield return new WaitForSeconds(time);
        ready = true;
    }
}
