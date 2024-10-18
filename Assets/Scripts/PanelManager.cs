using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject trialPanel;
    public GameObject shieldPanel;
    public GameObject firePanel;

    public Button trialButton;
    public Button shieldButton;
    public Button fireButton;

    private void Start()
    {
        // Set initial state: Trial panel is active, others are inactive
        ActivatePanel(trialPanel);

        // Set TrialButton as selected button
        SetButtonSelected(trialButton);

        // Add listeners to buttons
        trialButton.onClick.AddListener(() => { ActivatePanel(trialPanel); SetButtonSelected(trialButton); });
        shieldButton.onClick.AddListener(() => { ActivatePanel(shieldPanel); SetButtonSelected(shieldButton); });
        fireButton.onClick.AddListener(() => { ActivatePanel(firePanel); SetButtonSelected(fireButton); });
    }

    // Activates the chosen panel and deactivates others
    private void ActivatePanel(GameObject activePanel)
    {
        trialPanel.SetActive(activePanel == trialPanel);
        shieldPanel.SetActive(activePanel == shieldPanel);
        firePanel.SetActive(activePanel == firePanel);
    }

    // Set the visual state of buttons to reflect the selected one
    private void SetButtonSelected(Button selectedButton)
    {
        trialButton.interactable = selectedButton != trialButton;
        shieldButton.interactable = selectedButton != shieldButton;
        fireButton.interactable = selectedButton != fireButton;
    }
}
