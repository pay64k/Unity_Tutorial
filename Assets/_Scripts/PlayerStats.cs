using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public float health = 100f;

    public Text healthText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

}
