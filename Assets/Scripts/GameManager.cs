using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode;
using System.Collections;

public class GameManager : NetworkBehaviour
{
    private AudioSource audioSource;

    public NetworkVariable<int> scorePlayer1 = new NetworkVariable<int>();
    public NetworkVariable<int> scorePlayer2 = new NetworkVariable<int>();

    public TMP_Text scoreTextPlayer1; // Change type to TMP_Text
    public TMP_Text scoreTextPlayer2; // Change type to TMP_Text

    public GameObject puck;
    private Vector3 puckStartPosition;

    public GameObject winningScreen;
    public TMP_Text winningText; // Change type to TMP_Text

    public int winningScore = 7;

    public GameObject puckPrefab;
    private GameObject puckInstance;

    public void SpawnPuck()
    {
        if (puckInstance == null && NetworkManager.Singleton.IsServer) // Ensure it's the server and puck isn't already spawned
        {
            puckInstance = Instantiate(puckPrefab, Vector3.zero, Quaternion.identity);
            NetworkObject puckNetworkObject = puckInstance.GetComponent<NetworkObject>();
            if (puckNetworkObject != null)
            {
                puckNetworkObject.Spawn(); // Spawn over the network
            }
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        scorePlayer1.OnValueChanged += HandleScoreChanged;
        scorePlayer2.OnValueChanged += HandleScoreChanged;
        UpdateScoreText(); // Initial update
        puckStartPosition = puck.transform.position;
        UpdateScoreText();
        winningScreen.SetActive(false);
    }

    private void HandleScoreChanged(int oldValue, int newValue)
    {
        UpdateScoreText();
    }

    public void AddScoreToPlayer(int playerNumber)
    {
        if (playerNumber == 1)
        {
            scorePlayer1.Value++; // Corrected
        }
        else if (playerNumber == 2)
        {
            scorePlayer2.Value++; // Corrected
        }

        UpdateScoreText();
        audioSource.Play();

        CheckForWinner();
    }


    void UpdateScoreText()
    {
        scoreTextPlayer1.text = scorePlayer1.Value.ToString();
        scoreTextPlayer2.text = scorePlayer2.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreToPlayerServerRpc(int playerNumber)
    {
        if (playerNumber == 1)
        {
            scorePlayer1.Value++;
        }
        else if (playerNumber == 2)
        {
            scorePlayer2.Value++;
        }

        UpdateScoreText(); // Ensure this method updates the UI appropriately
        CheckForWinner(); // Check if the new score has produced a winner
    }

    void CheckForWinner()
    {
        if (scorePlayer1.Value >= winningScore)
        {
            ShowWinnerClientRpc($"Game Over. Player 1 wins!");
        }
        else if (scorePlayer2.Value >= winningScore)
        {
            ShowWinnerClientRpc($"Game Over. Player 2 wins!");
        }
    }

    [ClientRpc]
    public void ShowWinnerClientRpc(string winnerMessage)
    {
        winningScreen.SetActive(true);
        winningText.text = winnerMessage;
    }


    public void RestartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            ResetScoresServerRpc(); // Reset scores before restart
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene

        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetScoresServerRpc()
    {
        scorePlayer1.Value = 0;
        scorePlayer2.Value = 0;
        UpdateScoreText(); // Refresh score display after reset
    }


}