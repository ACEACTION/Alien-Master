using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinLosePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;    

    private void Start()
    {
        coinText.text = CoinManager.Instance.GetCoinsInLevel().ToString();

        if (GameManager.Instance.GetIsWin()) winPanel.SetActive(true);
        else losePanel.SetActive(true);
    }

    

}
