using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    enum State
    {
        Idle,
        Wander,
        Flee,
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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;

        currentState = State.Wander;
        timer = wanderTimer;

        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectDistance && currentState != State.Flee)
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
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    void StartFlee()
    {
        currentState = State.Flee;
        agent.speed = runSpeed;
    }

    void Flee()
    {
        Vector3 dir = transform.position - player.position;
        Vector3 newPos = transform.position + dir.normalized * fleeDistance;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(newPos, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
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