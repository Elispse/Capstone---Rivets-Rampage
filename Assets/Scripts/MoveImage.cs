using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveImage : MonoBehaviour
{
    [SerializeField] private float startPosition;
    [SerializeField] private float endPosition;
    [SerializeField] private float speed;

    private Vector3 startVector;
    private Vector3 endVector;
    private bool ready = false;

    private void Awake()
    {
        GameObject parent = this.gameObject.transform.parent.gameObject;
        startVector = new Vector3(startPosition, 0, 0);
        endVector = new Vector3(endPosition, 0, 0);
        transform.localPosition = startVector;
        Time.timeScale = 1;
        StartCoroutine(StartMoveImageCR());
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (ready)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + (speed * Time.deltaTime), 0, 0);
            if (transform.localPosition.x >= endVector.x)
            {
                transform.localPosition = startVector;
                ready = false;
                StartCoroutine(StartMoveImageCR());
            }
        }
    }

    IEnumerator StartMoveImageCR()
    {
        yield return new WaitForSeconds(3);
        ready = true;
    }
}