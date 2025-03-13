using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour, IDamagable, IHealable, IScoreable
{
    [SerializeField] private FloatVariable healthVar;
    [SerializeField] private IntVariable score;
    [SerializeField] private VoidEvent hurtEvent;
    [SerializeField] private VoidEvent healEvent;
    [SerializeField] private VoidEvent deadEvent;

    public static PlayerMain Instance { get; private set; }

    private DamageFlash damageFlash;

    private bool isHit = false;

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }
        Instance = this;
    }
    public void AddScore(int score)
    {
        this.score.value += score;
    }

    public void ApplyDamage(float damage)
    {
        if (!isHit)
        {
            isHit = true;
            healthVar.value -= damage;
            if (healthVar.value <= 0)
            {
                deadEvent.RaiseEvent();
            }
            damageFlash.CallDamageFlash();
            hurtEvent.RaiseEvent();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerHurt, this.transform.position);
            StartCoroutine(damageCooldownCR(6, 1));
        }
    }

    public void Heal(float health)
    {
        healthVar.value += health;
        if (healthVar.value >= 6)
        {
            healthVar.value = 6;
        }
        healEvent.RaiseEvent();
    }

    private IEnumerator damageCooldownCR(int numberOfFlashes, float time)
    {
        yield return new WaitForSeconds(time);
        isHit = false;
    }
}