using Unity.Netcode;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public Transform redSpawnPoint; // Assuming a single spawn point for simplicity
    public Transform blueSpawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetRedSpawnPoint()
    {
        return redSpawnPoint; // Return the red team's spawn point
    }

    public Transform GetBlueSpawnPoint()
    {
        return blueSpawnPoint; // Return the blue team's spawn point
    }
}

