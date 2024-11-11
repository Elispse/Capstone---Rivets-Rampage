using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour, IDamagable, IHealable, IScoreable
{
    [SerializeField]
    private FloatVariable healthVar;
    [SerializeField]
    private IntVariable score;

    public void AddScore(int score)
    {
        this.score.value += score;
    }

    public void ApplyDamage(float damage)
    {
        healthVar.value -= damage;
        if (healthVar.value <= 0)
        {
            Debug.Log("I'm Dead");
        }
        Debug.Log("I've Been Hit");
    }

    public void Heal(float health)
    {
        healthVar.value += health;
    }
}