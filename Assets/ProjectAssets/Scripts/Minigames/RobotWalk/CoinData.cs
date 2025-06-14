using UnityEngine;

[CreateAssetMenu(menuName = "Data/CoinData")]
public class CoinData : ScriptableObject
{
    public int coins;

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void ResetCoins()
    {
        coins = 0;
    }
}