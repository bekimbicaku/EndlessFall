using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    public GameObject[] playerCharacters; // Array to hold all player character GameObjects
    public VerticalCameraFollow cameraFollow; // Reference to the VerticalCameraFollow script

    private int monsterStartIndex = 0;
    private int alienStartIndex = 8;
    private int animalStartIndex = 16;
    private int robotStartIndex = 24;
    private PlayerController playerController;

    private void Start()
    {
        // Load the selected character index or default to 0 if none is set
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        playerController = FindObjectOfType<PlayerController>();

        // Ensure the selected character is within valid range
        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < playerCharacters.Length)
        {
            ActivateCharacter(selectedCharacterIndex);
        }
        else
        {
            Debug.LogError("Selected character index out of range or not found!");
        }
    }

    private void ActivateCharacter(int index)
    {
        // Deactivate all characters first
        foreach (GameObject character in playerCharacters)
        {
            character.SetActive(false);
        }

        // Activate the selected character
        playerCharacters[index].SetActive(true);

        // Update the camera follow target to the active character
        cameraFollow.player = playerCharacters[index].transform;
    }
   
}
