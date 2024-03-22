using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    [SerializeField]
    private float minX = -5f;
    [SerializeField]
    private float maxX = 5f;
    [SerializeField]
    private float minZ = -5f;
    [SerializeField]
    private float maxZ = 5f;

    void Update()
    {
        // Clamp the player's position within the specified bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minZ, maxZ));
    }
}
