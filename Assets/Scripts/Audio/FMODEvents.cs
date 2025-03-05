using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerWalk { get; private set; }
    [field: SerializeField] public EventReference playerHurt { get; private set; }
    [field: SerializeField] public EventReference playerHeal { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference enemyWalk { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference gameMusic { get; private set; }
    [field: SerializeField] public EventReference menuMusic { get; private set; }
    [field: SerializeField] public EventReference gameWon { get; private set; }
    [field: SerializeField] public EventReference roomComplete { get; private set; }

    [field: Header("Weapon SFX")]
    [field: SerializeField] public EventReference nailGun { get; private set; }
    [field: SerializeField] public EventReference wrench { get; private set; }

    [field: Header("World SFX")]
    [field: SerializeField] public EventReference doorOpen { get; private set; }
    [field: SerializeField] public EventReference barrelExplosion { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference UIClick { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene");
        }
        instance = this;
    }
}
