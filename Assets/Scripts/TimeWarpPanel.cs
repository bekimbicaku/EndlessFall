using UnityEngine;

public class TimeWarpPanel : MonoBehaviour
{
    public void OnUseTimeWarpButtonClicked()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.UseTimeWarp();
        }
    }

    public void OnQuitButtonClicked()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.QuitGame();
        }
    }
}