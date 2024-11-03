using System.Collections;
using UnityEngine;

public class MeleeWeaponBase : WeaponBase
{
    public Vector3 finalPos;
    [SerializeField, Range(0, 5)] public float attackRadius = 1;
    public override void Attack()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector3 playerPos = transform.position;
        Vector3 playerToCursor = cursorPos - playerPos;
        Vector3 dir = playerToCursor.normalized;
        Vector3 cursorVector = dir * 1.25f;
        finalPos = transform.position + cursorVector;

        var colliders = Physics2D.OverlapCircleAll(finalPos, 1, layerMask);
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
