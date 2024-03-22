using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    public GameManager gameManager; // Assign in the Inspector

    public void Restart()
    {
        gameManager.RestartGame();
    }
}
