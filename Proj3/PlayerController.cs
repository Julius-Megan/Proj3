using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public int maxHealth = 100; // Maximum health of the player
    public float bounceForce = 10f;
    public GameManager gameManagerScript;
    public Slider HealthBar;
    public Canvas menuButton;
    public GameObject laserPref;
    public GameObject gameOverText;
    public Camera mainCamera;
    public AudioClip hitSound;
    public AudioClip transformSfx;
    public AudioClip spiderHitSfx;

    private AudioSource playerAudio;
    public int currentHealth; // Current health of the player
    private Rigidbody rb;
    private float verticalRotation = 0f;
    private Animator playerAnim;
    private bool isClosed;


    public static PlayerController Instance { get; private set; }

    // Add other GameManager variables and methods as needed

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager between scenes if needed
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        currentHealth = maxHealth;
        // Lock cursor to center of screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        


    }

    void Update()
    {   
        if (!PauseMenu.GameIsPaused)
        {
            MovePlayer();

            // Player looking/turning
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            transform.Rotate(Vector3.up * mouseX);
            if (Camera.main != null)
            {
                Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            }
            

            //ChangeHealthBar
            HealthBar.value = currentHealth;

            //Change from open to rolling
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TransformRobot();
                playerAudio.PlayOneShot(transformSfx, 1.0f);
            }

            if (currentHealth <= 1)
            {
                //Add death animation
                //PauseGame
                currentHealth = 0;
                gameOverText.SetActive(true);
                PauseMenu.GameIsPaused = true;
                gameOverText.SetActive(true);
                StartCoroutine(DelayBeforeFunctionCall());
            }
        }


    }

    void Death()
    {
        currentHealth = 0; 
        PauseMenu.GameIsPaused = true;
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.QuitGame();
        }

    }
    IEnumerator DelayBeforeFunctionCall()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        // Call your function here after 5 seconds
        Death();

    }

    // Function to handle damage taken by the enemy
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

    }

    void MovePlayer()
    {
        // Player movement
        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 moveDirection = transform.forward * verticalMovement;
        rb.AddForce(moveDirection, ForceMode.VelocityChange);

        // Limit the maximum speeds
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Check if the player is moving
        bool isMoving = (Mathf.Abs(verticalMovement) > 0.00001f);

        // Set the "IsMoving" parameter in the animator
        playerAnim.SetBool("isMoving", isMoving);

    }
    public void TransformRobot()
    {
        // Perform actions based on the isOpen value
        if (playerAnim.GetBool("isOpen"))
        {   //Player Close
            playerAnim.SetBool("isOpen", false);
            moveSpeed = 500.0f;
            isClosed = true;
        }
        else
        {   //Player Open
            playerAnim.SetBool("isOpen", true);
            moveSpeed = 250.0f;
            isClosed = false;
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the specific enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Calculate the bounce direction away from the enemy
            Vector3 bounceDirection = (-transform.position);

            // Apply the bounce force to the player
            rb.AddForce(bounceDirection * bounceForce, ForceMode.Force);

            TakeDamage(10);
            Debug.Log("Damage 5");
            playerAudio.PlayOneShot(spiderHitSfx, 1.0f);

            TransformRobot();
        }

        if (collision.gameObject.CompareTag("crab"))
        {
            if (isClosed)
            {
                TransformRobot();
            }
            else
            {
                TakeDamage(3);
                playerAudio.PlayOneShot(hitSound, 0.5f);
            }

            
        }

        if (collision.gameObject.CompareTag("spider"))
        {
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(spiderHitSfx, 1.0f);
            if (isClosed)
            {

                GameManager gameManager = GameManager.Instance;
                if (gameManager != null)
                {
                    gameManagerScript.IncreaseScore(2);
                }

            }
            else
            {

                TakeDamage(10);
            }
        }

        if (collision.gameObject.CompareTag("portal"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            transform.position = new Vector3(-4.4f, 0f, 8.7f);
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (collision.gameObject.CompareTag("ship"))
        {

        }

    }

}
