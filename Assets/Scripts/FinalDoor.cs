using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private VoidEvent gameEnd;
    
    public void DoorOpens()
    {
        animator.SetTrigger("DoorAnim");
    }

    public void ColliderOn()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameEnd.RaiseEvent();
    }
}
