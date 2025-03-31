using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    private bool isGameOver = false;
    public bool isGameActive = true;

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);//Hide Game Over UI at game start
        }
    }

    public void gameOver()
    {
        if (!isGameOver)//Prevent Game Over from being called again
        {
            isGameOver = true;
            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
            }
            else
            {
                Debug.LogError("Game Over UI is not set in Inspector yet");
            } 
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
