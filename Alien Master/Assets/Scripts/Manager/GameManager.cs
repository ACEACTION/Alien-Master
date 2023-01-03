using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

    [Header("Slow Motion")]
    [SerializeField] float slowMotionScale = .2f;
    [SerializeField] float slowMotionDuration = 1;
    public int totalCoin;

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

    

}
