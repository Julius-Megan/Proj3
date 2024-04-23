using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public GameObject laserPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f; // Lifetime of the bullet
    public Transform shootPoint;
    public AudioClip laserSound;

    private AudioSource laserAudio;

    void Start()
    {
        laserAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !PauseMenu.GameIsPaused)
        {
            Shoot();
            laserAudio.PlayOneShot(laserSound, 1.0f);
        }
    }

    void Shoot()
    {
        if (shootPoint == null)
        {
            Debug.LogError("Shoot point not assigned!");
            return;
        }

        Vector3 spawnPosition = shootPoint.position;
        Quaternion spawnRotation = Quaternion.LookRotation(shootPoint.forward);

        GameObject bullet = Instantiate(laserPrefab, spawnPosition, spawnRotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.velocity = shootPoint.forward * bulletSpeed;

        // Destroy the bullet after the specified lifetime
        Destroy(bullet, bulletLifetime);
    }

    // OnCollisionEnter is called when the bullet collides with another object
    void OnTriggerEnter(Collider other)
    {
        // Check if the collision involves the bullet
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("crab"))
        {

            // Destroy the bullet when it collides with something
            Destroy(gameObject);

        }
    }
}
