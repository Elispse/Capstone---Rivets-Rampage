using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_NailGun : WeaponBase
{
    [SerializeField] int ammoCount;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField, Range(1, 50)] int fireForce = 20;
    private float weaponSpeed;
    

    private void Awake()
    {
        WeaponAnimator = GetComponent<Animator>();
        weaponSpeed = WeaponAnimator.speed;
    }

    public override void Attack()
    {
        Vector3 attackDirection = (PointerPosition - (Vector2)this.gameObject.transform.position).normalized;
        var finalPos = this.gameObject.transform.position + attackDirection;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().Damage = damage;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
        
    }

    public override bool Use()
    {
        WeaponAnimator.speed = weaponSpeed;
        bool used = false;
        if (ready && ammoCount != 0)
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

        return used;
    }

    IEnumerator ResetAttackReadyCR(float time)
    {
        yield return new WaitForSeconds(time);
        ready = true;
    }
}
