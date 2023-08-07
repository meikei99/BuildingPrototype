using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // Reference to the UI Text displaying money

    public void UpdateMoneyUI(int currentMoney)
    {
        // Update the money text UI with the current amount of money
        moneyText.text = currentMoney.ToString();
    }
}
