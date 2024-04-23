using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public Animation anim;
    public string walkAnimationName = "Run";

    public float moveSpeed = 3f;

    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Get the Animation component attached to the GameObject
        anim = GetComponent<Animation>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Make sure the animation clip exists in the Animation component
        if (anim != null && anim[walkAnimationName] != null)
        {
            // Set the walk animation to loop
            anim[walkAnimationName].wrapMode = WrapMode.Loop;

            // Play the walk animation
            anim.Play(walkAnimationName);
        }
        else
        {
            Debug.LogWarning("Animation or animation clip not found!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            transform.LookAt(player);
            Vector3 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed, ForceMode.Force);
        }



    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the specific enemy
        if (collision.gameObject.CompareTag("Player"))
        {

        }

    }
}
