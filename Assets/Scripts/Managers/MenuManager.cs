using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private const int GAME_SCENE_INDEX = 1;

    [SerializeField] private SoundManager soundManager;

    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject garageWindow;

    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Header("Music")]
    [SerializeField] SoundManager.Sound[] mainMenuMusic;

    private void Start()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        garageWindow.SetActive(false);

        SoundManager.instance.PlayMusic(mainMenuMusic);
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(GAME_SCENE_INDEX);
    }

    public void OnSettingsButton() 
    {
        mainWindow.SetActive(false);
        settingsWindow.SetActive(true);
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
    }

    public void OnSettingsBackButton()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }


    public void OnGarageButton()
    {
        mainWindow.SetActive(false);
        garageWindow.SetActive(true);
    }

    public void OnGarageBackButton()
    {
        mainWindow.SetActive(true);
        garageWindow.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
