using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public int maxHealth = 5;
    public bool facingLeft = true;
    public float moveSpeed = 2f;
    public Transform wallCheck;
    public float distance = 0.5f;
    public LayerMask wallLayer;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;


    void Update()
    {
        if (FindObjectOfType<GameManager>().isGameActive == false)
        {
            return;
        }

        if (maxHealth <= 0)
        {
            Die();
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if (inRange)
        {
            if (player.position.x > transform.position.x && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);
            }
        }

        // Always walk to the left
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        // Detect front wall
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, Vector2.left, distance, wallLayer);

        // If you hit a wall -> turn around.
        if (wallHit.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        transform.eulerAngles = facingLeft ? new Vector3(0, 0, 0) : new Vector3(0, -180, 0);
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<Player>() != null)
            {
                collInfo.gameObject.GetComponent<Player>().TakeDamage(5);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }

        maxHealth -= damage;
        Debug.Log(gameObject.name + " โดนโจมตี! เหลือ HP: " + maxHealth);
    }


    private void OnDrawGizmosSelected()
    {
        if (wallCheck == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallCheck.position, Vector2.left * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    void Die()
    {
        Debug.Log(this.transform.name + " Died.");
        Destroy(this.gameObject);
    }
}
