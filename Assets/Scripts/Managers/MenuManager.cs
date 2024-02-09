using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private const string PLAYER_HAVE_CAR_BASE = "PlayerHaveCar_";

    private const int GAME_SCENE_INDEX = 1;

    [SerializeField] private SoundManager soundManager;

    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject garageWindow;

    [SerializeField] private TextMeshProUGUI mainCoinText;

    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Header("Garage")]
    [SerializeField] private PlayerCarSO[] carImages;
    private int selectedCarIndex = 0;
    [SerializeField] private Image garageCarImage;
    [SerializeField] private TextMeshProUGUI garageCarCostText;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private TextMeshProUGUI garageCoinText;

    [Header("Music")]
    [SerializeField] SoundManager.Sound[] mainMenuMusic;
    [SerializeField] SoundManager.Sound clickButtonSound;
    [SerializeField] SoundManager.Sound buyButtonSound;

    private void Start()
    {

        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        garageWindow.SetActive(false);

        SoundManager.instance.PlayMusic(mainMenuMusic);

        UpdateCoinText();

        PlayerPrefs.SetInt(PLAYER_HAVE_CAR_BASE + "0", 1);
        selectedCarIndex = PlayerPrefs.GetInt(GameManager.EQUIPPED_CAR, 0);
    }

    private void UpdateGarageImage() 
    {
        garageCarImage.sprite = carImages[selectedCarIndex].sprite;
        garageCarCostText.text = carImages[selectedCarIndex].cost.ToString();

        UpdateCoinText();

        if (PlayerPrefs.GetInt(PLAYER_HAVE_CAR_BASE + selectedCarIndex.ToString()) == 0)
        {
            //Do not have that car
            equipButton.SetActive(false);
            buyButton.SetActive(true);
        }
        else
        {
            //Have that car
            if (PlayerPrefs.GetInt(GameManager.EQUIPPED_CAR, 0) == selectedCarIndex)
            {
                //Also have this car as selected

                equipButton.SetActive(false);
                buyButton.SetActive(false);
            }
            else
            {
                //Also DO NOT have this car as selected

                equipButton.SetActive(true);
                buyButton.SetActive(false);
            }
        }
    }

    private void UpdateCoinText() 
    {
        garageCoinText.text = PlayerPrefs.GetInt(GameManager.COINS_COUNT,0).ToString();
        mainCoinText.text = PlayerPrefs.GetInt(GameManager.COINS_COUNT, 0).ToString();
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(GAME_SCENE_INDEX);
    }

    public void OnSettingsButton() 
    {
        mainWindow.SetActive(false);
        settingsWindow.SetActive(true);

        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void OnSoundToggle()
    {
        if (soundToggle.isOn)
        {
            soundToggle.gameObject.GetComponent<Image>().sprite = soundOnSprite;
            soundManager.TurnOnSound();
        }
        else
        {
            soundToggle.gameObject.GetComponent<Image>().sprite = soundOffSprite;
            soundManager.TurnOffSound();
        }
        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void OnSettingsBackButton()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);

        SoundManager.instance.PlaySound(clickButtonSound);
    }


    public void OnGarageButton()
    {
        mainWindow.SetActive(false);
        garageWindow.SetActive(true);

        UpdateGarageImage();

        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void OnGarageBackButton()
    {
        mainWindow.SetActive(true);
        garageWindow.SetActive(false);

        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void OnGarageBuyButton()
    {
        int totalCoins = PlayerPrefs.GetInt(GameManager.COINS_COUNT, 0);

        int price = carImages[selectedCarIndex].cost;

        if (price <= totalCoins)
        {
            totalCoins -= price;
            PlayerPrefs.SetInt(GameManager.COINS_COUNT, totalCoins);

            PlayerPrefs.SetInt(PLAYER_HAVE_CAR_BASE + selectedCarIndex.ToString(), 1);

            PlayerPrefs.SetInt(GameManager.EQUIPPED_CAR, selectedCarIndex);

            SoundManager.instance.PlaySound(buyButtonSound);
        }

        UpdateGarageImage();
    }

    public void OnGarageEquipButton()
    {
        PlayerPrefs.SetInt(GameManager.EQUIPPED_CAR,selectedCarIndex);
        UpdateGarageImage();

        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void OnGarageRightArrowButton()
    {
        if (selectedCarIndex + 1 < carImages.Length)
        {
            selectedCarIndex++;
        }
        UpdateGarageImage();

        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void OnGarageLeftArrowButton()
    {
        if (selectedCarIndex - 1 >= 0)
        {
            selectedCarIndex--;
        }
        UpdateGarageImage();

        SoundManager.instance.PlaySound(clickButtonSound);
    }

    public void Quit()
    {
        SoundManager.instance.PlaySound(clickButtonSound);

        PlayerPrefs.SetInt(GameManager.COINS_COUNT, 100);
        Debug.Log("Quit");
        Application.Quit();
    }
}
