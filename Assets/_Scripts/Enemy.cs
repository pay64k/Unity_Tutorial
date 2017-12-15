using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public float health = 50f;
    public float lookRadius = 10f;
    public float idleTime = 3f;
    public float wanderTime = 5f;
    public float dyingTime = 3f;
    public ParticleSystem dieEffect;
    public float damage = 10f;

    public float attackSpeed = 1f;

    private PlayerStats stats;
    private float attackTimer = 0f;

    bool isDying = false;
    bool isWandering = false;
    bool isIdle = true;
    bool isFollowing = false;

    float idlingTime = 0f;
    float wanderigTime = 0f;

    Transform player;
    EnemyManager manager;
    
    NavMeshAgent agent;
    Vector3 wanderPickedDestination;

    void Start()
    {
        manager = GameObject.Find("Enemy Spawner").GetComponent<EnemyManager>();
        player = GameObject.Find("Player").transform;

        agent = GetComponent<NavMeshAgent>();

        stats = GameObject.Find("Player").GetComponent<PlayerStats>();

        InvokeRepeating("PickWanderingDestination", 0, 0.5f);
    }

    // This is rather crude and not easy to extend method. It is also very specific to idle-wander-follow-attack routine.
    // It should be redone follwoing one of the "mofre flexible" and modern methods for example like this tutorial here:
    // https://www.youtube.com/watch?v=cHUXh5biQMg&t=1s

    void Update()
    {
        if (isDying)
        {
            return;
        }

        if (idlingTime <= idleTime)
        {
            if (isIdle && !isFollowing)
            {
                isWandering = false;
                agent.SetDestination(gameObject.transform.position);
                idlingTime += Time.deltaTime;
            }
        }
        else
        {
            idlingTime = 0f;
            isIdle = false;
            isWandering = true;
            agent.SetDestination(wanderPickedDestination);
        }

        if (wanderigTime <= wanderTime)
        {
            if (isWandering && !isFollowing)
            {
                isWandering = true;
                wanderigTime += Time.deltaTime;
            }
        }
        else
        {
            wanderigTime = 0f;
            isIdle = true;
            isWandering = false;
        }

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= lookRadius)
        {
            isFollowing = true;
            agent.SetDestination(player.position);
            //distance = Vector3.Distance(player.position, transform.position);
            //print(distance.ToString() + " " + agent.stoppingDistance.ToString());
            if (distance <= agent.stoppingDistance)
            {
                //player.GetComponent<PlayerStats>().
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackSpeed)
                {
                    stats.TakeDamage(damage);
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            isFollowing = false;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDying)
            Die();
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        dieEffect.Play();
        isDying = true;
        agent.SetDestination(transform.position);
        manager.enemyKilled(gameObject);
        Destroy(gameObject, dyingTime);
    }

    void PickWanderingDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 100;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 100, 1);
        wanderPickedDestination = hit.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
