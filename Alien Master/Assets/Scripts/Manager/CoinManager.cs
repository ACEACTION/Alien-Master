using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] int coinsInLevel;
    public int totalCoins;
    const string cointDataPath = "coin.data";

    public static CoinManager Instance;

    private void Awake()
    {
        coinsInLevel = 0;
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        LoadTotalCoins();
    }

    public int GetCoinsInLevel()
    {
        return coinsInLevel;
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public void MinusCoins(int coins)
    {
        totalCoins -= coins;
        SaveTotalCoins();
        UIController.Instance.UpdateTotalMoneyUI();
    }

    public void AddCoins(int coins)
    {
        coinsInLevel += coins;
        totalCoins += coins;
        UIController.Instance.UpdateTotalMoneyUI();
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
