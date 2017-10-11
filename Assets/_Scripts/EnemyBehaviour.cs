using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public float health = 50f;
    public GameObject destroyedVariant;
    public GameObject explosion;

    private bool gotShoot;
    private EnemyManager manager;

    void Start()
    {
        gotShoot = false;
        manager = GameObject.Find("Enemy Spawner").GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {

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
            Destroy(explosionObj, 1f);
            Destroy(destroyed, 5f);
        }

    }
}
