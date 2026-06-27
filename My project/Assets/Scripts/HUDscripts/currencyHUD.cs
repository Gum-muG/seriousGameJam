using TMPro;
using UnityEngine;

public class currencyHUD : MonoBehaviour
{
    public TMP_Text getCurrency;

    private playerCurrency currency;

    private void Start()
    {
        currency = FindAnyObjectByType<playerCurrency>();
        updateCurrencyDisplay();
    }

    private void Update()
    {
        if (currency == null)
            currency = FindAnyObjectByType<playerCurrency>();

        updateCurrencyDisplay();
    }

    private void updateCurrencyDisplay()
    {
        if (currency == null || getCurrency == null)
            return;

        getCurrency.text = "Coins: " + currency.coins;
    }
}