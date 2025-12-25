using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, ReturnToPatrol }
    public EnemyState currentState;

    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public int attackDamage = 20;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;


    [Header("Vision Settings")]
    public float detectionRange = 10f;
    public float losePlayerRange = 15f;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int patrolIndex = 0;

    [Header("Random Return Settings")]
    public float randomPointRadius = 10f;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrol;

        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (GetComponent<Health>().isDead)
             return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.ReturnToPatrol:
                ReturnToPatrol();
                break;
        }

        UpdateAnimations();
    }

    // -----------------------------
    //        PATROL MODE
    // -----------------------------
    void Patrol()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }


    // -----------------------------
    //          CHASE MODE
    // -----------------------------
    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > losePlayerRange)
        {
            currentState = EnemyState.ReturnToPatrol;
            PickRandomPointAndMove();
            return;
        }

        agent.SetDestination(player.position);

                // اگر نزدیک پلیر شد → حمله
        if (distanceToPlayer <= attackRange)
        {
            agent.ResetPath();
            TryAttack();
            return;
        }
    }


    // -----------------------------
    //       LOST PLAYER MODE
    // -----------------------------
    void ReturnToPatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = EnemyState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    // تولید یک نقطهٔ تصادفی روی NavMesh
    void PickRandomPointAndMove()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomPointRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, randomPointRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    // -----------------------------
    //        ANIMATION HANDLER
    // -----------------------------
    void UpdateAnimations()
    {
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);
    }

    // -----------------------------
    //        ATTACK HANDLER
    // -----------------------------

    void TryAttack()
{
    if (Time.time - lastAttackTime < attackCooldown)
        return;

    lastAttackTime = Time.time;

    anim.SetTrigger("Attack");

    // ضربه بعد از یک زمان کوتاه زده می‌شود
    Invoke(nameof(DealDamageToPlayer), 0.5f); // زمان هماهنگ با انیمیشن
}
void DealDamageToPlayer()
{
    float dist = Vector3.Distance(transform.position, player.position);
    if (dist <= attackRange)
    {
        Health playerHp = player.GetComponent<Health>();
        playerHp?.TakeDamage(attackDamage);
    }
}


}
