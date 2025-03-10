using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using System.Reflection;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CircleCollider2D radiusCollider; // The explosion radius
    [SerializeField] private int damage = 2;
    private bool isExploding = false;

    void Start()
    {
        animator.ResetTrigger("Explode");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits the barrel
        if (collision.gameObject.GetComponent<Projectile>())
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploding) return; // Prevent multiple explosions

        isExploding = true;
        animator.SetTrigger("Explode");

        // Play explosion sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.barrelExplosion, transform.position);

        // Destroy the capsule collider (assuming it's for the barrel body)
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());

        // Apply knockback to all objects in the explosion radius
        Collider2D[] objectsInside = Physics2D.OverlapCircleAll(radiusCollider.bounds.center, radiusCollider.radius);
        KnockBack(objectsInside);
    }

    private void KnockBack(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                PlayerMovement player = rb.GetComponent<PlayerMovement>();
                if (player != null) // If it's the player, use custom knockback handling
                {
                    Vector3 direction = (collider.transform.position - transform.position).normalized;
                    direction.z = 0;
                    player.ApplyKnockback(direction, 3f, 0.25f); // Apply knockback to player
                }
                else if (rb.CompareTag("Enemy"))
                {
                    GameObject enemy = rb.gameObject;
                    Vector3 direction = (collider.transform.position - transform.position).normalized;
                    direction.z = 0;
                    InvokeFunction(enemy, "ApplyKnockback", new object[] { direction, 10f, 0.5f });
                }
                if (collider.gameObject.TryGetComponent(out IDamagable damagable))
                {
                    damagable.ApplyDamage(damage);
                }
            }
        }
    }

    public static bool InvokeFunction(GameObject obj, string functionName, object[] parameters = null)
    {
        // Get all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component == null) continue;

            // Get the Type of the component
            System.Type type = component.GetType();

            // Search for a method with the given name
            MethodInfo method = type.GetMethod(functionName, BindingFlags.Public | BindingFlags.Instance);

            if (method != null)
            {
                // Invoke the method on the component
                method.Invoke(component, parameters);
                return true; // Method was found and invoked
            }
        }

        Debug.LogWarning($"Function '{functionName}' not found on {obj.name}");
        return false;
    }

    public void destroyBarrel()
    {
        Destroy(gameObject);
    }
}
