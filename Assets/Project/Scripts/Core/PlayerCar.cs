using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
//using MoreMountains.NiceVibrations;
using PathCreation;
using Project.Scripts.Core;
using Project.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCar : BaseCar
{
    [SerializeField] public int multiplier;
    [SerializeField] private int damage = 5;

    [SerializeField] public Line lineRenderer;
    [SerializeField] public float duration;
    [SerializeField] public Collectable collectablePrefab;
    [SerializeField] public LevelText levelText;
    [SerializeField] public Transform visual;
    [SerializeField] public FollowCamera followCamera;
    [SerializeField] public ParticleSystem eggParticle;
    [SerializeField] private ParticleSystem obstacleParticle;
    [SerializeField] public ParticleSystem levelUpParticle;
    [SerializeField] public ParticleSystem levelFailParticle;
    [SerializeField] public ParticleSystem carTransformParticle;
    [SerializeField] private Transform smokeTrail;

    [SerializeField] private Transform levelEnd;
    


    [Header("LineColors")] [SerializeField]
    private List<Color> colors = new List<Color>();

    private bool isPushedBack;
    private float lastLinePointAddedDistance;

    [Header("Coin Specs"), Space] [SerializeField]
    public GameObject canvas;

    [SerializeField] public CoinAnimation coinAnimationPrefab;
    [SerializeField] public Transform coinTarget;

    [SerializeField] private List<Transform> carVisuals = new List<Transform>();
    [SerializeField] private List<Transform> carMesh = new List<Transform>();
    [SerializeField] private new Collider collider;

    [SerializeField] private List<Transform> breakVisuals = new List<Transform>();
    
    [Title("Path")] [SerializeField] public PathCreator newPath;


    private void OnEnable()
    {
        GameManager.OnGameStarted += InputEnable;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= InputEnable;
    }

    private void Start()
    {
        level = GameManager.Instance.startingLevel;
        levelText.UpdateText(level, $"level");
        path = FindObjectOfType<SelectedPath>().GetComponent<SelectedPath>().path;
        CheckVisual();
    }
    
    int lastArrayIndex = -1;

    public void CheckVisual()
    {
        for (int i = 0; i < carVisuals.Count; i++)
        {
            carVisuals[i].gameObject.SetActive(false);
        }

        int remain = level % 10;
        int modLevel = (level - remain) / 10;
        int arrayIndex = Mathf.Clamp(modLevel, 0, carVisuals.Count - 1);
        carVisuals[arrayIndex].gameObject.SetActive(true);
        if (lastArrayIndex != arrayIndex)
        {
            lastArrayIndex = arrayIndex;
            smokeTrail.transform.SetParent(carMesh[arrayIndex].transform);
            carVisuals[arrayIndex].GetComponent<CarAnimation>().PlayAnimation();
            ScaleAnimation();
            StartCoroutine(nameof(SpeedRoutine));
            lineRenderer.lineRenderer.materials[0].DOColor(colors[arrayIndex], 1).SetEase(Ease.InOutQuart);
        }
    }

    private void CheckBreak()
    {
        for (int i = 0; i < breakVisuals.Count; i++)
        {
            breakVisuals[i].DOKill(transform);
            breakVisuals[i].DOLocalMoveZ(0.01f, 0.2f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo).easeOvershootOrAmplitude =30;
        }
    }

    public IEnumerator LevelEndRoutine()
    {
        levelEnd.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        headTransform.DOScale(2, 0.5f);
        distanceTravelled = 1;
        path = newPath;
        lineRenderer.gameObject.SetActive(false);
        InputDisable();
        canMove = true;
        speed = 25;
    }

    private IEnumerator SpeedRoutine()
    {
        speed = 10;
        followCamera.ZoomInCamera(8);
        yield return new WaitForSeconds(2);
        speed = 7;
        followCamera.ZoomInCamera(-8);
    }

    private void ScaleAnimation()
    {
        levelUpParticle.Play();
        carTransformParticle.Play();
    }

    public void InputEnable()
    {
        InputPanel.Instance.OnPointerDownEvent.AddListener(OnPointerDown);
        InputPanel.Instance.OnPointerUpEvent.AddListener(OnPointerUp);
    }


    public void InputDisable()
    {
        InputPanel.Instance.OnPointerDownEvent.RemoveListener(OnPointerDown);
        InputPanel.Instance.OnPointerUpEvent.RemoveListener(OnPointerUp);
    }

    private void OnPointerDown()
    {
        canMove = true;
    }

    private void OnPointerUp()
    {
        canMove = false;
        //CheckBreak();
    }

    protected override void OnUpdate()
    {
        if (isPushedBack) return;
        if (distanceTravelled < lastLinePointAddedDistance) return;

        lastLinePointAddedDistance = distanceTravelled;
        Vector3 pos = headTransform.position;
        pos.y += 0.0125f;
        lineRenderer.AddPosition(pos);
    }

    public void PickAnimation()
    {
        visual.DOKill(transform);
        visual.DOScale(2.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuart);
    }

    public void OnLevelEnd()
    {
        InputDisable();
        canMove = true;
        speed = 8;
    }

    public void OnUpgradeScale(int upgradeLevel)
    {
        levelUpParticle.Play();
        Instantiate(collectablePrefab, headTransform.position + new Vector3(-1, -0.12f, -0.3f), Quaternion.identity,
            transform).InitLevel();
      //  MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        level += upgradeLevel;
        levelText.UpdateText(level, $"level");
    }

    [Button]
    public void KnockBack()
    {
        StartCoroutine(nameof(KnockBackRoutine));
    }

    private IEnumerator KnockBackRoutine()
    {
        // collider.enabled = false;
        obstacleParticle.Play();
      //  MMVibrationManager.Haptic(HapticTypes.Failure);
        InputDisable();
        if (level <= 4)
        {
            damage = level;
        }

        level = Mathf.Max(level - 5, 0);
        Instantiate(collectablePrefab, headTransform.position, Quaternion.identity, transform).Init(-5);
        CheckVisual();
        followCamera.CameraShake();
        canMove = true;
        speed = -4;
        levelText.UpdateText(level, $"level");
        MeshReflect(false);
        yield return new WaitForSeconds(0.1f);
        MeshReflect(true);
        yield return new WaitForSeconds(0.15f);
        MeshReflect(false);
        yield return new WaitForSeconds(0.2f);
        MeshReflect(true);
        yield return new WaitForSeconds(0.15f);
        canMove = false;
        speed = 7;
        InputEnable();
        if (level <= 0)
        {
            GameManager.Instance.LevelFail();
        }

        // collider.enabled = true;
    }

    private void MeshReflect(bool stat)
    {
        visual.gameObject.SetActive(stat);
    }
}