using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwatGuy : MonoBehaviour
{
    [Header("Health")]
    [HideInInspector] public float health;
    public float maxHealth;
    public bool isDead;

    [Header("Combat")]
    public float damage;
    public float fireRate; //delay between each shot
    private float fireTimer;
    public float spreadFactor = 0.1f;
    public float bulletSpeed;
    public ParticleSystem muzzleFlash;
    public AudioSource gunShot;

    [Header("Player Detection")]
    public float viewDistance;
    [Range(0, 360)] public float viewAngle;
    public LayerMask targetMask, wallMask, zombieMask;
    public float distanceToPlayer;
    public bool playerDetected;
    public bool zombieDetected;
    public bool isClose;
    public bool isFar;

    [Header("Patrolling")]
    public bool isPatrol;
    public Transform[] patrolPoints;
    private int destPoint = 0;

    [Header("Misc")]
    public GameObject player;
    public GameObject playerOrien;
    public Animator animator;
    public NavMeshAgent nav;
    private PlayerInventory playerinv;





    public State state;

    public enum State
    {
        Idle,
        Patrolling,
        Shooting,
        Strafing,
        Dead,
        ShootingZombie,
    }

    private void Start()
    {
        playerinv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        if (isPatrol)
        {
            state = State.Patrolling;
        }
        else
        {
            state = State.Idle;
        }

        health = maxHealth;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerOrien = GameObject.FindGameObjectWithTag("Player").transform.Find("Orientation").gameObject;
    }
    private void Update()
    {
        HealthManager();
        if (isDead)
        {
            GetComponent<Collider>().enabled = false;
        }
        if (!isDead)
        {
            distanceToPlayer = Vector3.Distance(transform.position, playerOrien.transform.position);
            SwatBrain();
            PlayerDetection();
        }

    }

    private void SwatBrain()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Shooting:
                Shoot();
                break;
            case State.Patrolling:
                Patrol();
                break;
            case State.Strafing:
                Strafe();
                break;
            case State.Dead:
                Die();
                break;

        }
    }

    void Idle()
    {
        animator.SetBool("isIdle", true);
    }

    public void RandomStrafe()
    {
        bool trueOrFalse = (Random.value > 0.5f);
        if (trueOrFalse)
        {
            animator.SetBool("isStrafe", true);
            state = State.Strafing;
        }
    }
    private void Strafe()
    {
        nav.Move(transform.right * 2 * Time.deltaTime);
    }
    void Shoot()
    {
        nav.ResetPath();
        if (playerDetected)
        {
            Vector3 targetPoso = new Vector3(playerOrien.transform.position.x + -0.5f, transform.position.y, playerOrien.transform.position.z);
            if (distanceToPlayer <= 3)
            {
                isClose = true;
                transform.LookAt(targetPoso);
            }
            if (distanceToPlayer >= 3 && distanceToPlayer <= 6)
            {
                isClose = false;
                isFar = false;
                transform.LookAt(targetPoso);
            }
            if (distanceToPlayer > 6)
            {
                isClose = false;
                isFar = true;
                transform.LookAt(targetPoso);
            }
        }
        if (isClose)
        {
            animator.SetBool("isClose", true);
            animator.SetBool("isShooting", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isFar", false);
            nav.Move(transform.forward * -2 * Time.deltaTime);
        }
        if (!isClose && playerDetected)
        {
            animator.SetBool("isShooting", true);
            animator.SetBool("isClose", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isFar", false);
        }
        if (!isClose && isFar && playerDetected)
        {
            animator.SetBool("isShooting", true);
            animator.SetBool("isClose", false);
            animator.SetBool("isFar", true);
            animator.SetBool("isIdle", false);
            nav.Move(transform.forward * 2 * Time.deltaTime);
        }
        Fire();
    }
    public void HealthManager()
    {
        if (health <= 0)
        {
            state = State.Dead;
        }
    }
    public void Die()
    {
        animator.SetTrigger("dead");
        Destroy(nav);
    }
    private void Fire()
    {
        if (Time.time > fireTimer)
        {
            fireTimer = Time.time + fireRate;
            RaycastHit hit;
            muzzleFlash.Play();
            gunShot.Play();

            Vector3 shootDirection = transform.forward;
            shootDirection.x += Random.Range(-spreadFactor, spreadFactor);
            shootDirection.y += Random.Range(-spreadFactor, spreadFactor);
            Debug.Log("SHOOT");


            if (Physics.Raycast(transform.position, shootDirection, out hit))
            {
                if (hit.transform.GetComponent<PlayerInventory>())
                {
                    playerinv.TakeDamage(5f);
                    Debug.Log("HIT");
                }
            }
        }



    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        playerDetected = true;
        state = State.Shooting;
    }

    private void Patrol()
    {
        animator.SetBool("isPatrolling", true);
        if (!nav.pathPending && nav.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }
        else
        {
            nav.destination = patrolPoints[destPoint].position;
            destPoint = (destPoint + 1) % patrolPoints.Length;
        }
    }

    private void ZombieDetection()
    {
        Collider[] zombieInView = Physics.OverlapSphere(transform.position, viewDistance, zombieMask);
        for (int i = 0; i < zombieInView.Length; i++)
        {
            Transform target = zombieInView[i].transform;
            player = target.gameObject;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, wallMask))
                {
                    zombieDetected = true;
                    state = State.Shooting;

                }
                
            }
        }
    }


    private void PlayerDetection()
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
                    playerDetected = true;
                    state = State.Shooting; 
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
}
