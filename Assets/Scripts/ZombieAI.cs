using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Health")]
    public float startHealth;
    public float health;
    [Header("Patrol")]
    public float wanderRadius;
    public float wanderTimer;
    public float viewDistance;
    public float viewAngle;
    public bool playerDetected;
    public LayerMask wallMask;

    [Header("Attacking")]
    public float runSpeed;
    public float damage;
    public float attackSpeed;
    public bool isCrawling;
    private bool inAttackRange;
    private float ROF;
    public bool canAttack;

    [Header("Misc")]
    public Animator animator;
    public NavMeshAgent nv;
    public bool isIdle;
    public bool isDead;
    public GameObject player;
    public LayerMask playerMask;
    public LayerMask targetMask;
    public float distanceFromPlayer;
    public GameObject playerOrien;
    private PlayerInventory playerHP;

    public NavMeshAgent agent;
    private float timer;

    public enum State
    {
        Patroling,
        WalkAtPlayer,
        RunAtPlayer,
        Attacking,
        Crawl,
        Dead,
        idle,
    }

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    public State state;

    private void Awake()
    {
        health = startHealth;
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        playerOrien = GameObject.FindGameObjectWithTag("Player").transform.Find("Orientation").gameObject;
        state = State.Patroling;

        animator = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();
        //isIdle = true;
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");

        if (isIdle)
        {
            state = State.idle;
        }
    }
    void Start()

    {
       

    }

    void ZombieBrain()
    {
        if (!isDead)
        {
            switch (state)
            {
                case State.Patroling:
                    Patrol();
                    break;
                case State.WalkAtPlayer:
                    WalkAtPlayer();
                    break;
                case State.RunAtPlayer:
                    RunAtPlayer();
                    break;
                case State.Attacking:
                    Attack();
                    break;
                case State.Crawl:
                    Crawling();
                    break;
                case State.Dead:
                    Death();
                    break;
                case State.idle:
                    Idle();
                    break;
            }
        }

    }
    void Update()
    {
        HealthManager();
        ZombieBrain();
        PlayerDetection();
        CrawlManager();
        
        DistanceCheck();
    }

    public void DistanceCheck()
    {
        if (!isCrawling)
        {
            distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceFromPlayer <= 1.5f && playerDetected)
            {
                state = State.Attacking;
            }
            else if (distanceFromPlayer >= 1.5f && playerDetected)
            {
                state = State.RunAtPlayer;
                
            }
        }
        
    }

    public void HealthManager()
    {
        if (health <= 0)
        {
            state = State.Dead;
        }
    }

    public void Idle()
    {
        animator.SetBool("idle", true);
    }

    public void Death()
    {
        if (isCrawling)
        {
            animator.SetBool("crawlingDeath", true);
        }
        else
        {
            animator.SetBool("dead", true);
        }

        animator.SetBool("chasing", false);
        animator.SetBool("attacking", false);
        animator.SetBool("crawling", false);

        isDead = true;
        Destroy(nv);

        foreach(Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }

    public void Crawling()
    {
        nv.speed = 1f;
        nv.SetDestination(player.transform.position);
        animator.SetBool("crawling", true);
    }

    public void TakeDamage(float damage)
    {
        if (!playerHP.weaponHolster[playerHP.weaponToEquip].GetComponent<Gun>().isSupressed)
        {
            playerDetected = true;
            state = State.RunAtPlayer;

        }
        
        health -= damage;
    }

    public void CrawlManager()
    {
        if (isCrawling)
        {
            state = State.Crawl;
        }
    }

    public void Patrol()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
    public void Attack()
    {
        nv.speed = 1.8f;
        animator.SetBool("attacking", true);
        animator.SetBool("chasing", false);
        if (Time.time > ROF)
        {
            canAttack = true;
            ROF = Time.time + attackSpeed;
            
            Vector3 targetPoso = new Vector3(playerOrien.transform.position.x, transform.position.y, playerOrien.transform.position.z);
            transform.LookAt(targetPoso);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, playerMask))
            {
               

                    //playerHP.TakeDamage(10f);
                    //return;
                    //canAttack = false;
                
            }

        }
        
    }
    public void RunAtPlayer()
    {
        animator.SetBool("attacking", false);
        animator.SetBool("chasing", true);
        nv.speed = 3f;
        nv.SetDestination(player.transform.position);
    }
    public void WalkAtPlayer()
    {
        animator.SetBool("attacking", false);
        nv.speed = 1f;
        nv.SetDestination(player.transform.position);
    }
    private void PlayerDetection()
    {
        if (!playerDetected && state != State.Dead)
        {
            Collider[] playersInView = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

            for (int i = 0; i < playersInView.Length; i++)
            {
                Transform target = playersInView[i].transform;
                player = target.gameObject;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, wallMask))
                    {
                        if (!inAttackRange)
                        {
                            playerDetected = true;
                            state = State.WalkAtPlayer;
                        }
                    }
                }
            }
        }
    }

    public Vector3 dirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //inAttackRange = true;
            //state = State.Attacking;
        }

        if (other.CompareTag("Door"))
        {
            if (other.GetComponentInParent<Door>().isPowered)
            {
                other.GetComponentInParent<Animator>().SetBool("open", true);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // inAttackRange = false;

            if (health < startHealth)
            {
                //state = State.RunAtPlayer;
            }
            else
            {
               // state = State.WalkAtPlayer;
            }

        }

        if (other.CompareTag("Door"))
        {
            if (other.GetComponentInParent<Door>().isPowered)
            {
                other.GetComponentInParent<Animator>().SetBool("open", false);
            }
        }
    }
}
