using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 50f;
    public float lookRadius = 10f;
    public float deadTime = 3f;
    public ParticleSystem dieEffect;

    Transform player;
    bool gotShoot = false;
    bool isDying = false;
    bool isWandering = true;
    EnemyManager manager;
    Animator anim;
    NavMeshAgent agent;

    void Start()
    {
        manager = GameObject.Find("Enemy Spawner").GetComponent<EnemyManager>();
        player = GameObject.Find("Player").transform;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        InvokeRepeating("Wander", 0, Random.Range(2f,5f));
    }

    

    void Update()
    {
        anim.SetBool("isIdle", true);
        float distance = Vector3.Distance(player.position, transform.position);
        isWandering = true;
        // If inside the radius
        if (distance <= lookRadius)
        {
            isWandering = false;
            if (isDying)
                return;

            // Move towards the player
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
            agent.SetDestination(player.position);
            if (distance <= agent.stoppingDistance)
            {
                // Attack
                FaceTarget();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        gotShoot = true;
        health -= amount;
        if (health <= 0 && !isDying)
            Die();
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        dieEffect.Play();
        isDying = true;
        agent.SetDestination(transform.position);
        anim.SetBool("isDying",true);
        manager.enemyKilled(gameObject);
        Destroy(gameObject, deadTime);
    }

    void Wander()
    {
        if (isWandering)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 100;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 100, 1);
            Vector3 finalPosition = hit.position;
            agent.destination = finalPosition;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
