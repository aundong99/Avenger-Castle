using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float moveDistance = 5f;
    private Vector3 startPos;
    private int direction = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(5); //Deal 5 damage to player.

        }
    }
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Change direction when reaching limit
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
        }
    }
}
