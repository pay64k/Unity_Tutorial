using UnityEngine;

public class TargetBehaviour : MonoBehaviour {

    public float health = 50f;

    public ParticleSystem hit;
    public ParticleSystem destroy;

    private EnemyManager manager;

    void Start()
    {
        manager = GameObject.Find("Enemy Spawner").GetComponent<EnemyManager>();
    }

    public void TakeDamage(float amount)
    {
        hit.Play();
        health -= amount;
        if(health <= 0)
        {
            destroy.Play();
            manager.enemyKilled(gameObject);
            Destroy(gameObject, destroy.main.duration);
        }
    }

}
