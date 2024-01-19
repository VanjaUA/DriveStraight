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

    private Vector2 inputVector;

    private void Start()
    {
        UpdateCoinsText(0);

        inputVector = Vector2.zero;
    }

    public void UpdateCoinsText(int newAmount) 
    {
        coinsText.text = newAmount.ToString();
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
}
