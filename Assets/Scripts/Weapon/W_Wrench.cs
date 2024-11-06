using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Wrench : WeaponBase
{
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override bool Use(Animator animator)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ResetAttackReadyCR(float time)
    {
        yield return new WaitForSeconds(time);
        ready = true;
    }
}
