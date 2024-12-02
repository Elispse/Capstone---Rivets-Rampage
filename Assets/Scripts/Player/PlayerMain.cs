using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour, IDamagable, IHealable, IScoreable
{
    [SerializeField]
    private FloatVariable healthVar;
    [SerializeField]
    private IntVariable score;
    [SerializeField] private VoidEvent hurtEvent;
    [SerializeField] private VoidEvent deadEvent;

    private bool isHit = false;

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
            hurtEvent.RaiseEvent();
            StartCoroutine(damageCooldownCR());
        }
    }

    public void Heal(float health)
    {
        healthVar.value += health;
    }

    private IEnumerator damageCooldownCR()
    {
        yield return new WaitForSeconds(1);
        isHit = false;
    }
}