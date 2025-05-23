using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : MonoBehaviour, IInteractable
{
    enum eType
    {
        Health,
        Coin,
        Weapon
    }

    [SerializeField] Action action;
    [SerializeField] eType type;
    [SerializeField] float value = 1;
    [SerializeField] bool valueOverTime = false;
    [SerializeField] bool destroyOnPickup = true;

    private void Start()
    {
        if (action != null)
        {
            action.onEnter += OnInteractStart;
            action.onStay += OnInteractActive;
        }
    }

    public void OnInteractStart(GameObject interactor)
    {
        if (valueOverTime) return;

        if (type == eType.Health)
        {
            if (interactor.TryGetComponent(out IHealable health))
            {
                health.Heal(value);
                if (destroyOnPickup) Destroy(gameObject);
            }
        }

        if (type == eType.Coin)
        {
            if (interactor.TryGetComponent(out IScoreable score))
            {
                score.AddScore((int)value);
                if (destroyOnPickup) Destroy(gameObject);
            }
        }

        if (type == eType.Weapon)
        {
            
        }
    }

    public void OnInteractActive(GameObject interactor)
    {
        if (!valueOverTime) return;

        if (interactor.TryGetComponent(out IHealable health))
        {
            health.Heal(value * Time.deltaTime);
        }
    }

    public void OnInteractEnd(GameObject interactor)
    {
        //
    }
}
