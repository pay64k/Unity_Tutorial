using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntEnemyBehaviour : MonoBehaviour {

    public float health = 50f;

    //public ParticleSystem hit;
    public GameObject destroyedVariant;
    public GameObject explosion;
    public float moveSpeed;


    private EnemyManager manager;
    private GameObject player;
    private Rigidbody rb;

    private bool gotShoot; 

    private Collider navCollider;
    private Vector3 wayPoint;
    private float startX;
    private float startZ;

    void Start()
    {
        manager = GameObject.Find("Enemy Spawner").GetComponent<EnemyManager>();
        
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();

        gotShoot = false;

        navCollider = GameObject.Find("Navigation").GetComponent<BoxCollider>();
        startX = (navCollider.bounds.center - navCollider.bounds.size / 2).x;
        startZ = (navCollider.bounds.center - navCollider.bounds.size / 2).z;
        InvokeRepeating("PickWayPoint", 0f, 2f);
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        Chase();
    }

    private void Chase()
    {
        transform.LookAt(player.transform);
        Vector3 direction = player.transform.position - transform.position;
        float magnitude = direction.magnitude;

        if (magnitude < 10f || gotShoot )
        {
            MoveTowards(direction, rb);
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        transform.LookAt(wayPoint);
        MoveTowards(wayPoint - transform.position, rb);
    }

    private void MoveTowards(Vector3 direction, Rigidbody rb)
    {
        direction.Normalize();
        Vector3 velocity = direction * moveSpeed;
        //rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        transform.position = transform.position + direction * Time.deltaTime * moveSpeed;
    }

    private void PickWayPoint()
    {
        wayPoint = new Vector3(Random.Range(startX, startX + navCollider.bounds.size.x), transform.position.y, Random.Range(startZ, startZ + navCollider.bounds.size.z));
    }

    public void TakeDamage(float amount)
    {
        //hit.Play();
        gotShoot = true;
        health -= amount;
        if (health <= 0)
        {
            manager.enemyKilled(gameObject);
            Destroy(gameObject);
            GameObject destroyed = Instantiate(destroyedVariant, transform.position, transform.rotation);
            GameObject explosionObj = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(explosionObj,1f);
            Destroy(destroyed, 5f);
        }
    }
}
