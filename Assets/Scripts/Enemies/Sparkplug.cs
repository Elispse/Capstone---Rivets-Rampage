using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Sparkplug : MonoBehaviour
{
    Transform target;
    [SerializeField] int segments;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private int damage = 1;
    [SerializeField] private float health = 10;
    [SerializeField] private float pointRadius = 10;
    [SerializeField] private GameObject healthDrop;

    private NavMeshAgent agent;
    private NavMeshData navData;
    private Animator animator;
    private Utility utility = new Utility();
    private DamageFlash damageFlash;

    private int distance = 3;
    private float rayDirection = 0;
    private bool wanderSpot = true;
    private bool castRay = true;
    public bool targetFound { get; set; }

    private EventInstance enemyFootsteps;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        navData = navMeshSurface.navMeshData;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        damageFlash = GetComponent<DamageFlash>();
        targetFound = false;

        enemyFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.enemyWalk);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            enemyFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void FixedUpdate()
    {
        if (castRay && !targetFound)
        {
            StartCoroutine(RayCastCR());
        }
        else if (targetFound)
        {
            target = GameObject.Find("Player").transform;
            Follow();
        }
        UpdateSound();
    }

    private IEnumerator RayCastCR()
    {
        castRay = false;
        yield return new WaitForSeconds(0.2f);
        int startAngle = -75;
        int finishAngle = 75;
        int increment = (int)(finishAngle / segments);
        Vector2 targetPos = Vector2.zero;
        Vector2 endPos;
        if (animator.GetBool("isWalking"))
        {
            if (agent.velocity.x >= 0.75)
            {
                rayDirection = -90;
            }
            if (agent.velocity.x <= -0.75)
            {
                rayDirection = 90;
            }
            if (agent.velocity.y <= -0.75)
            {
                rayDirection = -180;
            }
            if (agent.velocity.y >= 0.75)
            {
                rayDirection = 0;
            }
        }

        for (int i = startAngle; i < finishAngle; i += increment)
        {
            Vector2 rayDirection2D = (Quaternion.Euler(0, 0, i + rayDirection) * transform.up).normalized;
            targetPos = rayDirection2D * distance;
            endPos = new Vector2(transform.position.x + targetPos.x, transform.position.y + targetPos.y);

            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, rayDirection2D, distance, LayerMask.GetMask("Player"));
            if (hit2D && hit2D.rigidbody.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, endPos, Color.green);
                targetFound = true;
            }
            else
            {
                Debug.DrawLine(transform.position, endPos, Color.red);
                Wander();
            }
        }
        Gizmos.color = Color.red;
        castRay = true;
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        damageFlash.CallDamageFlash();
        if (health <= 0)
        {
            GenerateDrops();
            Destroy(gameObject);
        }
    }

    private void Follow()

    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(target.position);
        animator.SetFloat("CurX", agent.velocity.x);
        animator.SetFloat("CurY", agent.velocity.y);

        float distance = utility.GetDistanceBetweenTwoPoints(transform, target.transform);
    }

    private void Wander()
    {
        if (agent.remainingDistance <= 0.2 && wanderSpot == true)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable damagable) && collision.gameObject.CompareTag("Player"))
        {
            damagable.ApplyDamage(damage);
        }
    }

    IEnumerator ResetWanderDestination(float time)
    {
        wanderSpot = false;
        yield return new WaitForSeconds(time);
        Vector3 randomDirection = Random.insideUnitSphere * pointRadius;
        randomDirection += transform.position;
        RaycastHit2D hit = Physics2D.Raycast(randomDirection, Vector2.up);
        if (hit.collider != null && hit.collider.gameObject.name == "Floor" && hit.collider.gameObject.GetComponent<TilemapCollider2D>())
        {
            agent.SetDestination(randomDirection);
            wanderSpot = true;
            animator.SetBool("isWalking", true);
        }
        else
        {
            StartCoroutine(ResetWanderDestination(0));
        }
    }

    private void UpdateSound()
    {
        if (animator.GetBool("isWalking") == true)
        {
            PLAYBACK_STATE playbackState;
            enemyFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                enemyFootsteps.start();
            }
        }
        else
        {
            enemyFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void GenerateDrops()
    {
        int rand = Random.Range(1, 5);

        if (rand == 2)
        {
            GameObject newObject = Instantiate(healthDrop);
            newObject.transform.position = gameObject.transform.position;
        }
    }

    public void FoundTarget()
    {
        targetFound = true;
    }

    private void OnDestroy()
    {
        enemyFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
