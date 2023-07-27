using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinLosePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject finishGamePanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;    

    private void Start()
    {
        coinText.text = CoinManager.Instance.GetCoinsInLevel().ToString();

        if (SceneManager.GetActiveScene().name == "LEVEL-06")
        {
            finishGamePanel.SetActive(true);
            // ~~~ test ~~~ 
            coinText.transform.parent.gameObject.SetActive(false);
            return;
        }

        if (GameManager.Instance.GetIsWin()) winPanel.SetActive(true);
        else losePanel.SetActive(true);


    }

    public void NextLevel()
    {

        LevelManager.Instance.NextScene();
    }
    public void Retry() => LevelManager.Instance.RestartLevel();

}
