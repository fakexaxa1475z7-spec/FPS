using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    enum State
    {
        Idle,
        Wander,
        Flee,
        Charge,
        Dead
    }

    State currentState;

    NavMeshAgent agent;
    Transform player;

    [Header("Health")]
    public int maxHealth = 5;
    int currentHealth;

    [Header("Score")]
    public int scoreValue = 20;

    [Header("Idle")]
    bool isWaiting = false;
    float waitTimer = 0f;
    public float minWait = 2f;
    public float maxWait = 5f;

    [Header("Wander")]
    public float wanderRadius = 20f;
    public float wanderTimer = 5f;
    float timer;

    [Header("Detection")]
    public float detectDistance = 15f;

    [Header("Flee")]
    public float fleeDistance = 20f;
    public float runSpeed = 6f;
    public float walkSpeed = 2f;

    [Header("Attack")]
    public bool canAttack = false;
    public float attackDistance = 10f;
    public float chargeSpeed = 10f;
    public int damageToPlayer = 20;
    public float attackCooldown = 2f;

    float lastAttackTime;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        currentHealth = maxHealth;

        currentState = State.Wander;
        timer = wanderTimer;

        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        // 👉 ถ้าไม่มี player → เดินสุ่มอย่างเดียว
        if (player == null)
        {
            Wander();

            float speed = agent.velocity.magnitude;
            if (speed < 0.1f) speed = 0;

            anim.SetFloat("Speed", speed);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackDistance && Time.time > lastAttackTime + attackCooldown && canAttack)
        {
            StartCharge();
        }
        else if (distance < detectDistance && currentState != State.Flee && currentState != State.Charge)
        {
            StartFlee();
        }

        switch (currentState)
        {
            case State.Wander:
                Wander();
                break;

            case State.Flee:
                Flee();
                break;

            case State.Charge:
                Charge();
                break;
        }

        float speed2 = agent.velocity.magnitude;
        if (speed2 < 0.1f) speed2 = 0;

        anim.SetFloat("Speed", speed2);
    }

    void Wander()
    {
        // ถ้ากำลัง "หยุด"
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }

            return;
        }

        // ถ้าเดินถึงแล้ว → หยุด
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isWaiting = true;
            waitTimer = Random.Range(minWait, maxWait);

            agent.ResetPath(); // หยุดเดิน
            return;
        }

        // ถ้ายังไม่มีเป้าหมาย → สุ่มเดิน
        if (!agent.hasPath)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
        }
    }

    void StartFlee()
    {
        currentState = State.Flee;
        agent.speed = runSpeed;
    }

    void Flee()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 dir = transform.position - player.position;

            // เพิ่ม randomness
            Vector3 randomDir = dir.normalized + Random.insideUnitSphere * 0.5f;

            Vector3 newPos = transform.position + randomDir.normalized * fleeDistance;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(newPos, out hit, fleeDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }
    void StartCharge()
    {
        currentState = State.Charge;
        agent.speed = chargeSpeed;
    }
    void Charge()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
    }
    public void OnHitPlayer(Collision collision)
    {
        if (currentState != State.Charge) return;

        Debug.Log("Boar head hit player!");

        PlayerHealth hp = collision.transform.GetComponent<PlayerHealth>();

        if (hp != null)
        {
            hp.TakeDamage(damageToPlayer);
        }

        lastAttackTime = Time.time;

        StartFlee();
    }

    public void TakeDamage(int damage)
    {
        if (currentState == State.Dead)
            return;

        currentHealth -= damage;

        StartFlee();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (currentState == State.Dead)
            return;

        currentState = State.Dead;

        agent.isStopped = true;

        GetComponent<Collider>().enabled = false;

        anim.SetBool("IsDead", true);

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        Destroy(gameObject, 2f);
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }
}