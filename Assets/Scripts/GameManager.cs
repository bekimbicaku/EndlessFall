using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // List of possible spawn positions
    public Transform[] spawnPositions;  // Drag and drop your spawn points in the inspector
    public GameObject objectToSpawn;     // Drag your object prefab here
    public int score;
    private int highScore;

    public GameObject pausePanel;
   
    private PlayerController playerController;

    void Start()
    {
        // Call the MoveObjectToRandomPosition function
        MoveObjectToRandomPosition();
        playerController = FindObjectOfType<PlayerController>();

        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (SoundManager.instance != null)
        {
            // Play the background music
            SoundManager.instance.PlayBGM("gameMusic");
        }
        else
        {
            Debug.LogError("SoundManager instance is not found!");
        }
    }

    void MoveObjectToRandomPosition()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPositions.Length);
        // Move the existing object to a random spawn position
        objectToSpawn.transform.position = spawnPositions[randomIndex].position;
    }

  

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Quit the game (return to the main menu)
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
