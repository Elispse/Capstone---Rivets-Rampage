using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBase : WeaponBase
{
    public Vector3 finalPos;
    [SerializeField, Range(0, 5)] public float attackRadius = 1;
    public override void Attack()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 dir = (Quaternion.Euler(mousePos.x, mousePos.y, 0) * transform.up);
        Vector3 cursorVector = dir;
        finalPos = transform.position + cursorVector;

        var colliders = Physics2D.OverlapCircleAll(finalPos, 2, layerMask);
        foreach (var collider in colliders)
        {
            if ((tagName == "" || collider.gameObject.CompareTag(tagName)) && collider.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(damage);

            }
        }
    }

    public override bool Use(Animator animator)
    {
        bool used = false;
        if (ready)
        {
            if (animator != null && animationTriggerName != "")
            {
                animator.SetTrigger(animationTriggerName);
                ready = false;
                StartCoroutine(ResetAttackReadyCR(attackRate));

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(finalPos, attackRadius);
    }
}
