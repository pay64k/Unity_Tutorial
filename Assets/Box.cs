using UnityEngine;

public class Box : MonoBehaviour
{

    public Color newBoxColor;
    public Color newPlaneColor;
    public float interval = 1f;

    float timeElapsed = 0f;
    bool onThePlane = false;
    Renderer boxRend;

    void Start()
    {
        boxRend = GetComponent<Renderer>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= interval && onThePlane)
        {
            boxRend.material.color = Random.ColorHSV();
            timeElapsed = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Collision with: " + collision.transform.name);

        Renderer planeRend = collision.transform.GetComponent<Renderer>();
        planeRend.material.color = newPlaneColor;

        onThePlane = true;
    }
}
