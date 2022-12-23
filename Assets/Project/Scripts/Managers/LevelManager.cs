using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Utils;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoSingleton<LevelManager>
{
    public List<LevelObject> levels = new List<LevelObject>();

    private List<LevelObject> shuffledLevels = new List<LevelObject>();

    [Space, ReadOnly] public LevelObject currentLevel = null;
    [Space] public bool hasTutorialLevel = false;
    public const string PrefsLevelKey = "Level";
    
    private void Awake()
    {
        CreateLevelList();
        StartCoroutine(nameof(LevelChangeRoutine));
    }

    private void CreateLevelList()
    {
        shuffledLevels = new List<LevelObject>(levels);

        int level = PlayerPrefs.GetInt(PrefsLevelKey);
        if (level >= shuffledLevels.Count)
        {
            if (hasTutorialLevel)
            {
                shuffledLevels.RemoveAt(0);
            } 
            shuffledLevels.Shuffle();
        }
        else
        {
            for (int i = 0; i < level; i++)
            {
                shuffledLevels.RemoveAt(0);
            }
        }
    }

    public void SaveLevel()
    {
        int level = PlayerPrefs.GetInt(PrefsLevelKey) + 1;
        PlayerPrefs.SetInt(PrefsLevelKey, level);
    }
	
    public void NextLevel()
    {
        SaveLevel();
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator LevelChangeRoutine()
    {
        if (currentLevel != null)
        {
            yield return null;
            Destroy(currentLevel.gameObject);
        }
	
        UIManager.Instance.HidePanel(PanelType.All);
        currentLevel = Instantiate(shuffledLevels[0], shuffledLevels[0].transform.position, Quaternion.identity);
        yield return null;
        UIManager.Instance.ShowPanel(PanelType.TapToPlay);
    }

}