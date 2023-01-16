using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour 
{

    [Header("Slow Motion")]
    [SerializeField] float slowMotionScale = .2f;
    [SerializeField] float slowMotionDuration = 1;

    bool isWin;

    public static GameManager Instance;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance == null)
            Instance = this;
    }



    public void DoSlowMotion()
    {
        StartCoroutine(SlowMotionSequence());
    }

    IEnumerator SlowMotionSequence()
    {
        Time.timeScale = slowMotionScale;
        yield return new WaitForSeconds(slowMotionDuration);
        Time.timeScale = 1;
    }

    public void SetIsWin(bool isWin)
    {
        this.isWin = isWin;
        UIController.Instance.ActiveWinLosePanel();
    }
    
    public bool GetIsWin()
    {
        return isWin;
    }

}
