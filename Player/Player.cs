using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Text health;
    public Animator animator;
    private Rigidbody2D rb;
    public float jumpHeight = 5f;
    public bool isGround = true;
    private Rigidbody2D rigidBody2D;
    public float moveSpeed = 5f;
    private float movement;
    private bool facingRight = true;
    private Vector3 originalScale;
    private Vector3 originalSize;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;
    public GameManager gameManager;
    public GameObject reactionGroup;

    //This code is used to initialize the object in Unity by grabbing the Rigidbody2D, setting the health (currentHealth = maxHealth),
    //and saving the original size (originalSize) of the object. However, there are redundant variables 
    //(ridigBody2D and rb) and there may be typos (ridigBody2D → rigidBody2D).
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        originalSize = transform.localScale;
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = Vector3.zero; // หรือจุด Spawn
            Debug.LogWarning("Player fell out of bounds! Resetting position.");
        }

        //Check the health value, if the health value is less than or equal to 0 →
        //call the Die(); function.
        if (currentHealth <= 0)
        {
            Die();
        }

        //Control the character's rotation
        health.text = currentHealth.ToString();
        movement = Input.GetAxis("Horizontal");
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        //Jump when pressing Space
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        //Updated running animation
        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else
        {
            animator.SetFloat("Run", 0f);
        }

        //Play attack animation when left click is pressed.
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    //Speed ​​and Size/Health Buff in Unity via OnEnable and OnDisable Events
    private void OnEnable()
    {
        SpeedItem.OnSpeedCollected += BoostSpeed;
        SizeHealthItem.OnSizeHealthCollected += IncreaseSizeAndHealth;
    }

    private void OnDisable()
    {
        SpeedItem.OnSpeedCollected -= BoostSpeed;
        SizeHealthItem.OnSizeHealthCollected -= IncreaseSizeAndHealth;
    }

    private void BoostSpeed(float multiplier) //BoostSpeed and reset after 5 seconds
    {
        moveSpeed *= multiplier;
        StartCoroutine(ResetSpeedBoost());
    }

    private IEnumerator ResetSpeedBoost()
    {
        yield return new WaitForSeconds(5f); //BoostSpeed and reset after 5 seconds
        moveSpeed = 5f;
    }

    private void IncreaseSizeAndHealth(float sizeMultiplier, int healthBonus)
    {
        transform.localScale = originalSize * sizeMultiplier; //Restore original size
        maxHealth += healthBonus;
        currentHealth += healthBonus;
        StartCoroutine(ResetSize()); //ResetSize
    }
    private IEnumerator ResetSize()
    {
        yield return new WaitForSeconds(5f);

        transform.localScale = originalSize;//Restore original size

        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.mass = 1f;
            jumpHeight = 15f; //Reset jump force to normal
        }

        Debug.Log($"Resetting size to {originalSize}");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpeedItem"))
        {
            collision.GetComponent<SpeedItem>().Collect();
        }
        else if (collision.CompareTag("SizeHealthItem"))
        {
            collision.GetComponent<SizeHealthItem>().Collect();
        }
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }
    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<Enemy2>() != null)
            {
                collInfo.gameObject.GetComponent<Enemy2>().TakeDamage(5);
                Debug.Log("Attack Enemy" + collInfo.gameObject.name);
            }
            if (collInfo.gameObject.GetComponent<Enemy>() != null)
            {
                collInfo.gameObject.GetComponent<Enemy>().TakeDamage(1);
                Debug.Log("Attack Enemy" + collInfo.gameObject.name);
            }
        }
    }
    void Jump()
    {
        if (rigidBody2D != null)
        {
            rigidBody2D.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }
        currentHealth -= damage;
    }
    void Die()
    {
        Debug.Log(" Player Died.");
        FindObjectOfType<GameManager>().isGameActive = false;
        gameManager.gameOver();
        Destroy(gameObject);
    }
}
