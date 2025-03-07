using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool roomComplete = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && roomComplete)
        {
            gameObject.SetActive(false);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.doorOpen, this.transform.position);
        }
    }

    public void RoomComplete(bool Complete)
    {
        roomComplete = Complete;
    }
}