using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int totalCoins;
    const string cointDataPath = "coin.data";

    public static CoinManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        LoadTotalCoins();
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public void MinusCoins(int coins)
    {
        totalCoins -= coins;
        SaveTotalCoins();
    }

    public void AddCoins(int coins)
    {
        totalCoins += coins;
    }

    public void SaveTotalCoins()
    {
        SaveLoadSystem.SaveData<int>(totalCoins, cointDataPath);
    }

    public void LoadTotalCoins()
    {
        totalCoins = SaveLoadSystem.LoadData<int>(cointDataPath);        
    }
}
