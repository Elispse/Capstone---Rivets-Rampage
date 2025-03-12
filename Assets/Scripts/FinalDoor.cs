using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private VoidEvent stageEnd;

    private bool roomComplete = false;
    
    public void DoorOpens()
    {
        animator.SetTrigger("DoorAnim");
        roomComplete = true;
    }

    public void ColliderOn()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" &&  roomComplete == true)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.gameWon, this.transform.position);
            stageEnd.RaiseEvent();
        }
    }
}
