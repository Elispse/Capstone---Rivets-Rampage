using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float Damage { get; set; }
    private void Awake()
    {
        StartCoroutine(DestroyOverTime(5));
        gameObject.layer = 8;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Collider2D>().isTrigger)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable damagable) !& collision.gameObject.CompareTag("Enemy"))
        {
            damagable.ApplyDamage(Damage);
        }
    }

    IEnumerator DestroyOverTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}