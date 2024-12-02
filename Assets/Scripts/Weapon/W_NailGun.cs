using System.Collections;
using UnityEngine;

public class W_NailGun : WeaponBase
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField, Range(1, 50)] int fireForce = 20;
    private WeaponParent weaponParent;
    private GameObject bullet;
    private float weaponSpeed;
    private bool isReload = false;
    

    private void Awake()
    {
        WeaponAnimator = GetComponent<Animator>();
        weaponParent = this.gameObject.GetComponentInParent<WeaponParent>();
        weaponSpeed = WeaponAnimator.speed;
        ammoCount = magCapacity;
        weaponUI = GetComponentInParent<WeaponUI>();

        //update WeaponUI
        weaponUI.UpdateInfo(GetComponent<SpriteRenderer>().sprite, magCapacity, ammoCount);
    }

    public override void Attack()
    {
        Vector3 attackDirection = (weaponParent.pointerPosition - (Vector2)this.gameObject.transform.position).normalized;
        var finalPos = this.gameObject.transform.position + attackDirection;
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().Damage = damage;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
        ammoCount--;
        weaponUI.UpdateAmmoCount(ammoCount);
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