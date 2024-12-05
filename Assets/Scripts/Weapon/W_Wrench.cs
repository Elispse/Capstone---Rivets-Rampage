using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UIElements;

public class W_Wrench : WeaponBase
{
    [SerializeField, Range(0, 5)] float attackRadius = 1;
    [SerializeField, Range(0, 5)] float attackDistance = 1;
    private WeaponParent weaponParent;
    Vector3 finalPos;

    private void Awake()
    {
        WeaponAnimator = GetComponent<Animator>();
        weaponParent = this.gameObject.GetComponentInParent<WeaponParent>();
    }

    public override void Attack()
    {
        Vector3 p = transform.parent.parent.position;

        Vector3 attackDirection = (weaponParent.pointerPosition - (Vector2)p).normalized;
        Vector3 cursorVector = attackDirection * attackDistance;
        finalPos = p + cursorVector;

        var colliders = Physics2D.OverlapCircleAll(finalPos, attackRadius, layerMask);
        foreach (var collider in colliders)
        {
            if ((tagName == "" || collider.gameObject.CompareTag(tagName)) && collider.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(damage);
                collider.gameObject.GetComponent<Rigidbody2D>();
            }
        }
        AudioManager.instance.PlayOneShot(FMODEvents.instance.wrench, this.transform.position);
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

    public override IEnumerator ReloadCR()
    {
        yield return null;
    }
}