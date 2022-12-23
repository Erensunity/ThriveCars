using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Project.Scripts.Core
{
    public class LevelSelect : MonoBehaviour
    {
        [SerializeField] private List<Transform> levels = new List<Transform>();
        public const string PrefsLevel = "levelkey";

        private void OnEnable()
        {
            int level=PlayerPrefs.GetInt(PrefsLevel);
            for (int i = 0; i < levels.Count; i++)
            {
                levels[0].gameObject.SetActive(false);
            }

            if (level >= levels.Count)
            {
                int newLevel = Random.Range(0, levels.Count);
                levels[newLevel].gameObject.SetActive(true);
            }
            else
            {
                levels[level].gameObject.SetActive(true);
            }
        }
        
        public void NextLevel()
        {
            int level = PlayerPrefs.GetInt(PrefsLevel) + 1;
            PlayerPrefs.SetInt(PrefsLevel, level);
            SceneManager.LoadScene(0);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}