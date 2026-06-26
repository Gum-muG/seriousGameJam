using UnityEngine;

public class playerCurrency : MonoBehaviour
{
    public int coins = 0;

    public bool spendCoins(int amount)
    {
        if (coins < amount)
            return false;

        coins -= amount;
        return true;
    }

    public void addCoins(int amount)
    {
        coins += amount;
    }
}