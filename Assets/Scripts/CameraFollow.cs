using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTransform;


    private void Start()
    {
        followTransform = FindObjectOfType<PlayerMovement>().transform;
    }
    private void FixedUpdate()
    {
        this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z);
    }
}