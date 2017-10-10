using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntEnemyBehaviour : MonoBehaviour {

    public float health = 50f;

    //public ParticleSystem hit;
    public GameObject destroyedVariant;
    public GameObject explosion;

    private EnemyManager manager;

    void Start()
    {
        manager = GameObject.Find("Enemy Spawner").GetComponent<EnemyManager>();
    }

    public void TakeDamage(float amount)
    {
        //hit.Play();
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
