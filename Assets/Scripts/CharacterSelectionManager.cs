using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectionManager : MonoBehaviour
{
    public CharacterData[] characters;

    // UI Elements
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterCostText;
    public TextMeshProUGUI info;
    public Image characterImage;
    public Button nextButton;
    public Button previousButton;
    public Button buyButton;
    public Button selectButton;
    public TextMeshProUGUI totalCostText;

    public Button monstersButton;
    public Button aliensButton;
    public Button animalsButton;
    public Button robotsButton;

    public Image mainMenuCharacterImage; // For displaying selected character in main menu

    private int currentIndex = 0;
    private CharacterCategory selectedCategory = CharacterCategory.Monsters;
    private CharacterData[] filteredCharacters;

    // Define category ranges
    private int monsterStartIndex = 0;
    private int alienStartIndex = 8;
    private int animalStartIndex = 16;
    private int robotStartIndex = 24;

    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;
    public TextMeshProUGUI coinTxt;

    private void Start()
    {
        nextButton.onClick.AddListener(OnNextButton);
        previousButton.onClick.AddListener(OnPreviousButton);
        buyButton.onClick.AddListener(OnBuyButton);
        selectButton.onClick.AddListener(OnSelectButton);

        monstersButton.onClick.AddListener(() => OnCategoryButton(CharacterCategory.Monsters));
        aliensButton.onClick.AddListener(() => OnCategoryButton(CharacterCategory.Aliens));
        animalsButton.onClick.AddListener(() => OnCategoryButton(CharacterCategory.Animals));
        robotsButton.onClick.AddListener(() => OnCategoryButton(CharacterCategory.Robots));

        FilterCharacters();
        LoadSelectedCharacter(); // Load the selected character data on start
        UpdateUI(); // Load UI based on saved state
        UpdateButtonColors(); // Initialize button colors
    }


    private void Update()
    {
        coinTxt.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    private void FilterCharacters()
    {
        switch (selectedCategory)
        {
            case CharacterCategory.Monsters:
                filteredCharacters = System.Array.FindAll(characters, c => c.index >= monsterStartIndex && c.index < alienStartIndex);
                break;
            case CharacterCategory.Aliens:
                filteredCharacters = System.Array.FindAll(characters, c => c.index >= alienStartIndex && c.index < animalStartIndex);
                break;
            case CharacterCategory.Animals:
                filteredCharacters = System.Array.FindAll(characters, c => c.index >= animalStartIndex && c.index < robotStartIndex);
                break;
            case CharacterCategory.Robots:
                filteredCharacters = System.Array.FindAll(characters, c => c.index >= robotStartIndex && c.index < characters.Length);
                break;
        }
    }

    private void UpdateUI()
    {
        if (filteredCharacters == null || filteredCharacters.Length == 0)
            return;

        CharacterData character = filteredCharacters[currentIndex];

        // Load unlock state from PlayerPrefs
        character.isUnlocked = PlayerPrefs.GetInt(character.characterName + "_unlocked", 0) == 1;

        characterNameText.text = character.characterName;
        characterCostText.text = "Price: " + character.baseCost + "$";
        characterImage.sprite = character.characterSprite;

        if (character.isUnlocked)
        {
            // If the character is unlocked, show the "Select" button
            if (PlayerPrefs.GetInt("SelectedCharacter", -1) == character.index)
            {
                selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
                selectButton.GetComponent<Image>().color = selectedColor;
            }
            else
            {
                selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
                selectButton.GetComponent<Image>().color = defaultColor;
            }
            buyButton.gameObject.SetActive(false); // Hide the "Buy" button
            selectButton.gameObject.SetActive(true); // Show the "Select" button
        }
        else
        {
            // If the character is not unlocked, show the "Buy" button
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy: " + character.baseCost + "$";
            buyButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
        }
    }


    private void UpdateButtonColors()
    {
        monstersButton.GetComponent<Image>().color = defaultColor;
        aliensButton.GetComponent<Image>().color = defaultColor;
        animalsButton.GetComponent<Image>().color = defaultColor;
        robotsButton.GetComponent<Image>().color = defaultColor;

        switch (selectedCategory)
        {
            case CharacterCategory.Monsters:
                monstersButton.GetComponent<Image>().color = selectedColor;
                break;
            case CharacterCategory.Aliens:
                aliensButton.GetComponent<Image>().color = selectedColor;
                break;
            case CharacterCategory.Animals:
                animalsButton.GetComponent<Image>().color = selectedColor;
                break;
            case CharacterCategory.Robots:
                robotsButton.GetComponent<Image>().color = selectedColor;
                break;
        }
    }

    private void OnNextButton()
    {
        if (filteredCharacters == null || filteredCharacters.Length == 0)
            return;

        currentIndex = (currentIndex + 1) % filteredCharacters.Length;
        UpdateUI();
        SoundManager.instance.PlaySFX("btnSound");

    }

    private void OnPreviousButton()
    {
        if (filteredCharacters == null || filteredCharacters.Length == 0)
            return;

        currentIndex = (currentIndex - 1 + filteredCharacters.Length) % filteredCharacters.Length;
        UpdateUI();
        SoundManager.instance.PlaySFX("btnSound");

    }

    private void OnCategoryButton(CharacterCategory category)
    {
        selectedCategory = category;
        currentIndex = 0; // Reset index when switching categories
        FilterCharacters();
        UpdateUI();
        UpdateButtonColors();
    }

    public static bool SpendCoins(int amount)
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        if (coins >= amount)
        {
            coins -= amount;
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
            SoundManager.instance.PlaySFX("purchase");

            return true;
        }
        return false;
    }

    public void OnBuyButton()
    {
        CharacterData character = filteredCharacters[currentIndex];
        if (character == null) return;

        if (SpendCoins(character.baseCost))
        {
            character.isUnlocked = true;
            // Save the unlock state in PlayerPrefs
            PlayerPrefs.SetInt(character.characterName + "_unlocked", 1);
            PlayerPrefs.Save();
            UpdateUI();
            info.text = character.characterName + " purchased!";
            info.color = Color.green;
        }
        else
        {
            info.text = "Not enough coins to purchase " + character.characterName;
            info.color = Color.red;
        }
        SoundManager.instance.PlaySFX("btnSound");

        StartCoroutine(ClearTextAfterDelay(2f));
    }


    IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        info.text = "";
    }

    public void OnSelectButton()
    {
        CharacterData character = filteredCharacters[currentIndex];
        if (character == null || !character.isUnlocked) return;

        PlayerPrefs.SetInt("SelectedCharacter", character.index);
        PlayerPrefs.Save();
        info.text = character.characterName + " selected!";
        SoundManager.instance.PlaySFX("btnSound");

        UpdateMainMenuCharacter(character);
        UpdateUI();
    }

    private void LoadSelectedCharacter()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", -1);
        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < characters.Length)
        {
            CharacterData selectedCharacter = characters[selectedCharacterIndex];
            if (PlayerPrefs.GetInt(selectedCharacter.characterName + "_unlocked", 0) == 1)
            {
                UpdateMainMenuCharacter(selectedCharacter);
            }
        }
    }


    private void UpdateMainMenuCharacter(CharacterData character)
    {
        if (mainMenuCharacterImage != null)
        {
            mainMenuCharacterImage.sprite = character.characterSprite;
        }
    }
}
