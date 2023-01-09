using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int levelIndex;
    const string levelDataPah = "level.data";

    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void NextScene()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (levelIndex >= SceneManager.sceneCount)
        {
            // levels is over

        }
        else
        {
            //SaveLevelIndex();

            SceneManager.LoadScene(levelIndex);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void RestartLevel()
    {
        EnemyManager.enemiesList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveLevelIndex()
    {
        SaveLoadSystem.SaveData<int>(levelIndex, levelDataPah);
    }

    public void LoadLevelIndex()
    {
        levelIndex = SaveLoadSystem.LoadData<int>(levelDataPah);
    }

    public int GetLevelIndex() { return levelIndex; }


}
