using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnd : MonoBehaviour
{
    public Camera secondCamera;
    public GameObject shipObject;
    public float levitationSpeed = 1f;

    private bool isLevitating = false;
    private float levitationHeight = 5f;
    private bool collided = false;

    private void Start()
    {
        secondCamera.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collided = true;
            ActivateCameraAndLevitate();
            StartCoroutine(DelayBeforeFunctionCall());
        }
    }

    IEnumerator DelayBeforeFunctionCall()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        if (collided)
        {
            // Call your function here after 5 seconds
            YourFunctionAfterDelay();
        }
    }

    void YourFunctionAfterDelay()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.QuitGame();
        }
    }

    void Update()
    {
        if (isLevitating && shipObject != null)
        {
            shipObject.transform.Translate(Vector3.up * Time.deltaTime * levitationSpeed);

            if (shipObject.transform.position.y >= levitationHeight)
            {
                isLevitating = false;
            }
        }
    }

    void ActivateCameraAndLevitate()
    {
        if (secondCamera != null && shipObject != null)
        {
            Camera.main.enabled = false;
            secondCamera.enabled = true;
            isLevitating = true;
        }
    }

}
