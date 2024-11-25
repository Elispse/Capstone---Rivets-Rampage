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
        }
    }

    public void RoomComplete(bool Complete)
    {
        roomComplete = Complete;
    }
}