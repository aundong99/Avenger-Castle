using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBat : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force = 5f;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) // Prevent Error if there is no Player.
        {
            Vector3 direction = player.transform.position - transform.position; // Calculate the direction to the Player.
            rb.velocity = direction.normalized * force;

            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);// Rotate the bullet towards the Player.
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);// Destroys bullets after 10 seconds.
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(10); //Deal 10 damage to player.
            }
            Destroy(gameObject);
        }
    }
}
