using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviour
{
    public GameObject hostPlayerPrefab;
    public GameObject clientPlayerPrefab;
    public int clientsJoined = 0;


    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnServerStarted()
    {
        // Spawn the host player prefab only if this is the Host instance.
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnPlayer(NetworkManager.Singleton.LocalClientId, hostPlayerPrefab);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Check if this is the Host instance and the connected client is not the Host
        if (NetworkManager.Singleton.IsHost && clientId != NetworkManager.Singleton.LocalClientId)
        {
            // This is a client connecting to the host
            SpawnPlayer(clientId, clientPlayerPrefab);
            // Increment clientsJoined for each client that connects
            // Note: The host also counts as a client in most networking setups
            clientsJoined++;

            // Optionally, call some method to check if the puck should be spawned
            CheckAndSpawnPuck();
        }
        else if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsHost)
        {
            // This is a standalone server (non-host), spawn for every connected client
            SpawnPlayer(clientId, clientPlayerPrefab);
        }
    }

    private void SpawnPlayer(ulong clientId, GameObject prefab)
    {
        GameObject playerInstance = Instantiate(prefab);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId);
        }
        else
        {
            Debug.LogError("Spawned player prefab does not have a NetworkObject component.");
        }
    }

    void CheckAndSpawnPuck()
    {
        // Assuming the host does not count towards clientsJoined, or it's adjusted accordingly
        if (clientsJoined == 1) // This checks for one client connected besides the host
        {
            FindObjectOfType<GameManager>().SpawnPuck();
        }
    }
}
