using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    // Removed the NetworkVariable for team since the team is determined by host or client status
    // Assuming there are methods or properties in SpawnManager to get spawn points directly

    public override void OnNetworkSpawn()
    {
        if (IsServer) // The host
        {
            SetPlayerTeamServerRpc(1); // 1 for Red Team
        }
        else if (IsClient && IsOwner) // The client and owns this player
        {
            SetPlayerTeamServerRpc(2); // 2 for Blue Team
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SetPlayerTeamServerRpc(int team)
    {
        // Server determines the spawn point based on the team
        Transform spawnPoint = team == 1 ? SpawnManager.Instance.GetRedSpawnPoint() : SpawnManager.Instance.GetBlueSpawnPoint();

        // Move the player to the spawn point
        transform.position = spawnPoint.position;

        // If you have additional setup based on the team, you can add it here.
        // For example, setting the player's color or assigning team-specific abilities.
    }
}
