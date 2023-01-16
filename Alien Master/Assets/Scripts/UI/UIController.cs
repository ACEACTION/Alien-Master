using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject winLosePanel;

    public static UIController Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void ActiveWinLosePanel() => winLosePanel.SetActive(true);

}
