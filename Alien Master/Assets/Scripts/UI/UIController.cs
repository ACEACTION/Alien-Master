using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyTxt;
    [SerializeField] GameObject winLosePanel;
    public static UIController Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        UpdateTotalMoneyUI();
    }

    public void ActiveWinLosePanel()
    {                
        winLosePanel.SetActive(true);
    }

    public void UpdateTotalMoneyUI()
    {
        moneyTxt.text = CoinManager.Instance.totalCoins.ToString();
    }
}
