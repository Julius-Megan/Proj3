using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player; // Reference to the player GameObject
    public float followDistance = 40f;
    public float attackDistance = 2f;
    public float moveSpeed = 3f;
    public float maxSpeed = 5f;
    public int maxHealth = 100; // Maximum health of the enemy
    private GameManager gameManagerInstance;

    private int currentHealth; // Current health of the enemy
    private Animator enemyAnimator;
    private Rigidbody enemyRb;
    private bool isRunning;
    

    void Start()
    {
        currentHealth = maxHealth;
        enemyRb = GetComponent<Rigidbody>();
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManagerInstance = GameManager.Instance;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //If player is in view range
        if (distanceToPlayer <= followDistance && !PauseMenu.GameIsPaused)
        {
            // Face the player
            transform.LookAt(player);

            
            //If player is not in attack range
            if (distanceToPlayer > attackDistance)
            {
                isRunning = true;
                Move();
            }
            else
            {
                isRunning = false;
                Attack();
            }
        }
        else
        {
            enemyAnimator.SetBool("isChasing", false);
        }

    }

    // Function to handle damage taken by the enemy
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        gameManagerInstance.IncreaseScore(10);

        // Check if the enemy's health falls to zero or below
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle enemy death (e.g., play death animation, drop items, etc.)
        Debug.Log("Enemy died!");
        // Add your code for handling enemy death here
        Destroy(gameObject); // For example, destroy the enemy GameObject
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collision involves the bullet
        if (other.gameObject.CompareTag("Laser"))
        {
            TakeDamage(5);
            enemyAnimator.SetTrigger("isHit");
            Debug.Log("5 Damage");

            //Do some Effect and Sound over here!!!
        }
    }

    void Move()
    {
        if (isRunning == true){
            Vector3 direction = (player.position - transform.position).normalized;
            enemyRb.AddForce(direction * moveSpeed, ForceMode.Force);

            // Limit the maximum speeds
            if (enemyRb.velocity.magnitude > maxSpeed)
            {
                enemyRb.velocity = enemyRb.velocity.normalized * maxSpeed;
            }
            enemyAnimator.SetBool("isChasing", true);

        }

    }

    void Attack()
    {
        // Perform attack animation when near the player.
        isRunning = false;
        enemyAnimator.SetBool("isChasing", false);
        enemyAnimator.SetTrigger("isAttacking");
    }
}
