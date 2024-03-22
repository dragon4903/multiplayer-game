using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float hitPower = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            Vector3 hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized; // Ensure the direction vector has a magnitude of 1
            Rigidbody puckRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // Apply force to the puck
            puckRigidbody.AddForce(hitDirection * hitPower, ForceMode.Impulse);
        }
    }
}
