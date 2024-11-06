using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class W_Wrench : WeaponBase
{
    [SerializeField, Range(0, 5)] float attackRadius = 1;
    [SerializeField, Range(0, 5)] float attackDistance = 1;
    Vector3 finalPos;
    private void Awake()
    {
        WeaponAnimator = GetComponent<Animator>();
    }
    public override void Attack()
    {
        Vector3 attackDirection = (PointerPosition - (Vector2)this.gameObject.transform.position).normalized;
        Vector3 cursorVector = attackDirection * attackDistance;
        finalPos = this.gameObject.transform.position + cursorVector;

        var colliders = Physics2D.OverlapCircleAll(finalPos, attackRadius, layerMask);
        foreach (var collider in colliders)
        {
            if ((tagName == "" || collider.gameObject.CompareTag(tagName)) && collider.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(damage);
            }
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(finalPos, attackRadius);
    }
}