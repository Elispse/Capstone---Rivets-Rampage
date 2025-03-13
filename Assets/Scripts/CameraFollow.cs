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
        // Find the player if not already assigned or if the previous reference was destroyed
        if (!foundTarget || followTransform == null)
        {
            var player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                followTransform = player.transform;
                foundTarget = true;
            }
        }
        // Only move the camera if a valid target exists
        if (followTransform != null)
        {
            this.transform.position = new Vector3(
                followTransform.position.x,
                followTransform.position.y,
                this.transform.position.z
            );
        }
    }
}