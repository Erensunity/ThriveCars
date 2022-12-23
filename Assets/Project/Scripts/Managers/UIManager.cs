using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private List<Panel> panels = new List<Panel>();

    [SerializeField] private TextMeshProUGUI winLevelText = null;
    [SerializeField] private TextMeshProUGUI loseLevelText = null;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI currentStackText;
    [SerializeField] private TextMeshProUGUI currentGoldText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    private void Start()
    {
       currentLevelText.text = "LEVEL " + (PlayerPrefs.GetInt("levelkey") + 1);
       currentGoldText.text = $"{GameManager.Instance.currentGold}";
    }

    public void ShowPanel(PanelType panelType, bool hideOthers = true)
    {
        panels.ForEach(p =>
        {
            if (p.panelType == panelType || panelType == PanelType.All)
                p.Show();
            else if (hideOthers)
                p.Hide();
        });
    }

    public void HidePanel(PanelType panelType)
    {
        panels.ForEach(p =>
        {
            if (p.panelType == panelType || panelType == PanelType.All)
                p.Hide();
        });
    }

    public void UpdateLevelText()
    {
        currentLevelText.text = $"LEVEL {LevelManager.PrefsLevelKey}";
    }

    public void UpdateCurrentLevel()
    {
        currentStackText.transform.DOKill();
        currentStackText.transform.localScale = Vector3.one;
        currentStackText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo);
    }

    public void UpdateCurrentCoin()
    {
        currentGoldText.text = $"{GameManager.Instance.currentGold}";
        currentGoldText.transform.DOKill();
        currentGoldText.transform.localScale = Vector3.one;
        currentGoldText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo);
        PlayerPrefs.SetInt(GameManager.PrefsGoldKey, GameManager.Instance.currentGold);
    }
    

    public void UpdateLevelTexts()
    {
        int level = PlayerPrefs.GetInt(LevelManager.PrefsLevelKey) + 1;
        if (winLevelText)
        {
            winLevelText.text = $"Level {level} Completed";
        }

        if (loseLevelText)
        {
            loseLevelText.text = $"Level {level} Failed";
        }
    }
}

[Serializable]
public class Panel
{
    public GameObject panelObject;
    public PanelType panelType = PanelType.None;

    public void Show() => panelObject.SetActive(true);
    public void Hide() => panelObject.SetActive(false);
}

public enum PanelType
{
    None,
    All,
    TapToPlay,
    GamePlay,
    Win,
    Lose,
    Boss
}