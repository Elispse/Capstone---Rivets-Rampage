using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_NailGun : WeaponBase
{
    [SerializeField] int ammoCount;

    private void Awake()
    {
        WeaponAnimator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        //throw new System.NotImplementedException();
    }

    public override bool Use()
    {
        bool used = false;
        if (ready)
        {
            if (WeaponAnimator != null && animationTriggerName != "")
            {
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
