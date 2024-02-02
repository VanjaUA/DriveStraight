using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Image fuelFill;
    [SerializeField] private Gradient fuelBarGradient;


    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject loseUI;

    [SerializeField] private TextMeshProUGUI totalCoinsText;

    private Vector2 inputVector;

    private void Start()
    {
        UpdateCoinsText(0);

        inputVector = Vector2.zero;

        gameUI.SetActive(true);
        loseUI.SetActive(false);
    }

    public void UpdateCoinsText(int newAmount) 
    {
        coinsText.text = newAmount.ToString();
    }

    public void UpdateTotalCoinsText(int newAmount)
    {
        totalCoinsText.text =  "+ " + newAmount.ToString();
    }

    public void UpdateFuelFillAmount(float maxAmount,float currentAmount)
    {
        fuelFill.fillAmount = currentAmount / maxAmount;

        fuelFill.color = fuelBarGradient.Evaluate(currentAmount / maxAmount);
    }



    public void HorizontalAxis(float value) 
    {
        inputVector.x += value;
    }

    public void VerticalAxis(float value)
    {
        inputVector.y += value;
    }

    public Vector2 GetInputVector() 
    {
        return inputVector;
    }

    public void ActivateLoseScreen() 
    {
        gameUI.SetActive(false);
        loseUI.SetActive(true);
    }

    public void OnRetryButton() 
    {
        GameManager.instance.LoadGame();
    }

    public void OnBackButton()
    {
        GameManager.instance.LoadMenu();
    }
}
