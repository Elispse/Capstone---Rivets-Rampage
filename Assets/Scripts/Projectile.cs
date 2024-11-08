using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage { get; set; }
    private void Awake()
    {
        StartCoroutine(DestroyOverTime(10));
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.ApplyDamage(Damage);
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyOverTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
