using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private FloatVariable playerHealth;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IHealable healable) && collision.gameObject.CompareTag("Player") && playerHealth.value != 6)
        {
            healable.Heal(health);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerHeal, this.transform.position);
            Destroy(gameObject);
        }
    }
}