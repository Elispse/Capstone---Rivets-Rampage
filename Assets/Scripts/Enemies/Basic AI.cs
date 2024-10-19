using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    Transform target;
    [SerializeField] int segments;
    [SerializeField] private NavMeshSurface navMeshSurface;
    private NavMeshData navData;
    private NavMeshAgent agent;
    private int distance = 3;
    private Animator animator;
    private Utility utility = new Utility();
    private bool wanderSpot = true;

    // Start is called before the first frame update
    void Start()
    {
        navData = navMeshSurface.navMeshData;
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
            if (hit2D && hit2D.rigidbody.CompareTag("Player"))
            {
                Debug.DrawLine(transform.position, endPos, Color.green);
                target = hit2D.collider.gameObject.transform;
                animator.SetBool("isFollowing", true);
            }
            else
            {
                Debug.DrawLine(transform.position, endPos, Color.red);
                Wander();
            }
        }
        if (animator.GetBool("isFollowing"))
        {
            Follow();
        }
    }

    private void Follow()
    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(target.position);
        animator.SetFloat("CurX", agent.velocity.x);
        animator.SetFloat("CurY", agent.velocity.y);
    }

    private void Wander()
    {
        if (agent.remainingDistance == 0 && wanderSpot == true)
        {
            StartCoroutine(ResetWanderDestination(8.0f));
            animator.SetBool("isWalking", false);
        }
        if (agent.remainingDistance <= 0.3 && agent.remainingDistance != 0)
        {
            animator.SetFloat("LastX", agent.velocity.x);
            animator.SetFloat("LastY", agent.velocity.y);
        }
        animator.SetFloat("CurX", agent.velocity.x);
        animator.SetFloat("CurY", agent.velocity.y);
    }

    IEnumerator ResetWanderDestination(float time)
    {
        wanderSpot = false;
        yield return new WaitForSeconds(time);
        agent.SetDestination(utility.GetRandomDestination(navData.sourceBounds));
        wanderSpot = true;
        animator.SetBool("isWalking", true);
    }
}
