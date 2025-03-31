using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private Vector3 startPos;
    private float timer;
    private GameObject player;
    private int direction = 1;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float moveDistance = 5f;

    void Start()
    {
        startPos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Change direction when reaching limit
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
            Flip(); // Call the Flip function when changing direction.
        }

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 20)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Shoot()
    {
        if (player == null) return;

        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
