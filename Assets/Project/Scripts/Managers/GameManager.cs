using System;
using DG.Tweening;
//using MoreMountains.NiceVibrations;
using Project.Scripts.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public static event Action OnGameStarted = delegate { };
    [SerializeField] public PlayerCar player;


    [Title("UpgradeSpecs")] [SerializeField]
    private int levelUpPrice;
    
    public const string PrefsGoldKey = "Gold";
    public const string PrefsLevelUpgradeCostKey = "LevelUpgradeCost";
    public const string PrefsStartingLevelKey = "StartingLevel";
    public const string PrefsIncomeUpgradeCostKey = "LevelIncomeUpgradeCost";
    public const string PrefsStartingIncomeKey = "StartingIncome";

    public TextMeshProUGUI levelUpText;
    public TextMeshProUGUI incomeText;

    public int upgradeCostPerLevel;
    [ReadOnly] public int currentGold;
    [ReadOnly] public int startingLevel;
    [ReadOnly] public int upgradeLevelCost;
    [ReadOnly] public int upgradeIncomeCost;

    [SerializeField] private CanvasGroup incomeCanvas;
    [SerializeField] private CanvasGroup startingCanvas;
    
    [SerializeField] private Button startingButton;
    [SerializeField] private Button incomeButton;

    [SerializeField] private int incomePrice;
    [SerializeField] private int levelUpMultiplier;
    [SerializeField] private int incomeMultiplier;

    [SerializeField] private float startingSize;
    [SerializeField] private float incomeValue;

    [SerializeField] private SelectedPath selectedPath;
    
    

    private void Awake()
    {
        Application.targetFrameRate = 60;     
        currentGold = PlayerPrefs.GetInt(PrefsGoldKey);
        upgradeLevelCost = PlayerPrefs.GetInt(PrefsLevelUpgradeCostKey);
        upgradeIncomeCost = PlayerPrefs.GetInt(PrefsIncomeUpgradeCostKey);
        startingLevel = PlayerPrefs.GetInt(PrefsStartingLevelKey);
        incomeValue = PlayerPrefs.GetFloat(PrefsStartingIncomeKey);
        if (startingLevel == 0)
        {
            startingLevel = 1;
            upgradeLevelCost = 20;
            upgradeIncomeCost = 20;
        }

        if (upgradeIncomeCost == 0)
        {
            upgradeIncomeCost = 20;
            PlayerPrefs.SetInt(PrefsIncomeUpgradeCostKey, upgradeIncomeCost);
        }
        levelUpText.text = $"{upgradeLevelCost}";
        incomeText.text = $"{upgradeIncomeCost}";
        CheckButton();
    }

    public void GameStart()
    {
        OnGameStarted?.Invoke();
     //   Zerosum.Analytics.LevelStarted(PlayerPrefs.GetInt("levelkey") + 1);
    }

    public void IncomeUpgrade()
    {
        CheckButton();
        if (currentGold < upgradeIncomeCost)
        {
            incomeCanvas.alpha = 0.5f;
            return;
        }
        currentGold -= upgradeIncomeCost;
       // MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        incomeValue += 0.1f;
        upgradeIncomeCost += incomeMultiplier;
        UIManager.Instance.UpdateCurrentCoin();
        PlayerPrefs.SetFloat(PrefsStartingIncomeKey,incomeValue);
        PlayerPrefs.SetInt(PrefsIncomeUpgradeCostKey, upgradeIncomeCost);
        incomeText.text = $"{upgradeIncomeCost}";
        CheckButton();
    }

    public void CarLevelUpgrade()
    { 
        CheckButton();
        if (currentGold < upgradeLevelCost)
        {
            startingCanvas.alpha = 0.5f;
            return;
        }
        player.OnUpgradeScale(1);
      //  MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        currentGold -= upgradeLevelCost;
        ++startingLevel;
        upgradeLevelCost += upgradeCostPerLevel;

        UIManager.Instance.UpdateCurrentCoin();
        PlayerPrefs.SetInt(PrefsStartingLevelKey, startingLevel);
        PlayerPrefs.SetInt(PrefsLevelUpgradeCostKey, upgradeLevelCost);

       player.level = startingLevel;
       levelUpText.text = $"{upgradeLevelCost}";
       player.CheckVisual();
       CheckButton();
    }

    private void CheckButton()
    {
        if (currentGold < upgradeLevelCost)
        {
            startingCanvas.alpha = 0.5f;
        }
        if (currentGold < upgradeIncomeCost)
        {
            incomeCanvas.alpha = 0.5f;
        }
    }

    public void LevelWin()
    {
        GameEnd();
        UIManager.Instance.ShowPanel(PanelType.Win);
        // MMVibrationManager.Haptic(HapticTypes.Success);
        // Zerosum.Analytics.LevelCompleted(PlayerPrefs.GetInt("levelkey") + 1);
    }

    public void LevelFail()
    {
        GameEnd();
        UIManager.Instance.ShowPanel(PanelType.Lose);
        //MMVibrationManager.Haptic(HapticTypes.Failure);
        player.levelFailParticle.Play();
        //Zerosum.Analytics.LevelFailed(PlayerPrefs.GetInt("levelkey") + 1);
    }

    public void GameEnd()
    {
        player.canMove = false;
        player.InputDisable();
    }
}