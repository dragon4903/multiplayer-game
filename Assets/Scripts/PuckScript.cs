using UnityEngine;

public class PuckScript : MonoBehaviour
{
    private GameManager gameManager;

    public float maxSpeed = 10f; // The maximum speed the puck can travel at

    private AudioSource audioSource;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
        audioSource = GetComponent<AudioSource>();

    }

    private void FixedUpdate()
    {
        // Check if the puck's current speed exceeds the maximum allowed speed
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.velocity.magnitude > maxSpeed)
        {
            // If it does, set the velocity to the maximum speed but preserve the direction
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();

        if (collision.gameObject.CompareTag("RedGoal"))
        {
            gameManager.AddScoreToPlayer(2); // Assuming Player 2 scored against Player 1's goal
            ResetPuckPosition();
        }
        else if (collision.gameObject.CompareTag("BlueGoal"))
        {
            gameManager.AddScoreToPlayer(1); // Assuming Player 1 scored against Player 2's goal
            ResetPuckPosition();
        }
    }

    private void ResetPuckPosition()
    {
        // Reset logic as before
        transform.position = Vector3.zero;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
