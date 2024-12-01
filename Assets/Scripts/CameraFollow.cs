using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTransform;
    private bool foundTarget;

    private void Awake()
    {
        foundTarget = false;
    }
    private void Update()
    {
        if (!foundTarget)
        {
            followTransform = FindObjectOfType<PlayerMovement>().transform;
            foundTarget = true;
        }
        this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z);
    }
}