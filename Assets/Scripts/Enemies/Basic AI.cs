using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.HableCurve;

public class BasicAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int segments;
    private int distance = 1;
    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        int startAngle = -75;
        int finishAngle = 75;
        int increment = (int)(finishAngle / segments);
        Vector2 targetPos = Vector2.zero;
        Vector2 endPos;

        for (int i = startAngle; i < finishAngle; i += increment)
        {
            Vector2 rayDirection2D = (Quaternion.Euler(0, 0, i) * transform.up).normalized;
            targetPos = rayDirection2D * distance;
            endPos = new Vector2(transform.position.x + targetPos.x, transform.position.y + targetPos.y);

            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, rayDirection2D, distance);
            if (hit2D.collider)
            {
                Debug.DrawLine(transform.position, endPos, Color.green);
                animator.SetBool("isFollowing", true);
            }
            else
            {
                Debug.DrawLine(transform.position, endPos, Color.red);
            }
        }
        if (animator.GetBool("isFollowing"))
        {
            Move();
        }
    }

    private void Move()
    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(target.position);
        animator.SetFloat("CurX", agent.velocity.x);
        animator.SetFloat("CurY", agent.velocity.y);
    }

    private void Wander()
    {
        animator.SetBool("isWalking", true);
    }
}
