using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs; // List of player prefabs to spawn
    public Transform[] spawnPoints; // Array of possible spawn points

    private GameObject selectedPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0); // Get the selected character index
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length); // Select a random spawn point

        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < playerPrefabs.Length)
        {
            Transform spawnPoint = spawnPoints[randomSpawnIndex]; // Get the randomly selected spawn point
            selectedPlayer = Instantiate(playerPrefabs[selectedCharacterIndex], spawnPoint.position, spawnPoint.rotation); // Spawn the player at the random point
        }
        else
        {
            Debug.LogError("Invalid character index selected!");
        }
    }
}
