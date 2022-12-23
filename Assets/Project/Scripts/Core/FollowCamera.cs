using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEditor.Rendering;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] public Vector3 offset;
    [SerializeField] private Vector3 lookOffset;
    [SerializeField] private float maxDelta;

    [Title("Camera Shake")] [SerializeField]
    private float duration;

    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;
    [SerializeField] public Vector3 zoomOut;

    [SerializeField] private Transform camDistance;
    [SerializeField] private float zoomTime;
    [SerializeField] private Vector3 currentZoom;
    [SerializeField] private Vector3 newCameraPosition;
    

    private Vector3 velocity;
    private Vector3 targetLocalPosition;
    
    private void OnEnable()
    {
        targetLocalPosition = camDistance.localPosition;
        GameManager.OnGameStarted += OnGameStarted;
        StartCoroutine(nameof(CameraMovementRoutine));
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
       // StopCoroutine(nameof(CameraMovementRoutine));
    }

    private void Start()
    {
        MoveCamera(true);
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    public void LevelEndCamera()
    {
        target = null;
        transform.DOMove(newCameraPosition, 0.8f);
        GameManager.Instance.player.StartCoroutine(nameof(GameManager.Instance.player.LevelEndRoutine));
    }

    private IEnumerator CameraMovementRoutine()
    {
        while (true)
        {
            camDistance.DOLocalMove(
                Vector3.SmoothDamp(camDistance.localPosition, targetLocalPosition, ref velocity,
                    zoomTime * Time.deltaTime), 2);
            yield return null;
        }
    }
    
    private void MoveCamera(bool snap = false)
    {
        if(target==null) return;
        Vector3 localOffset = offset;

        Vector3 targetPosition = target.position + localOffset + camDistance.localPosition;

        transform.position =
            Vector3.SmoothDamp(transform.position, targetPosition, ref velocity,
                snap ? 0.0f : maxDelta * Time.deltaTime);
    }

    public void ZoomInCamera(int eggLevel)
    {
        camDistance.transform.DOKill();
        currentZoom = zoomOut * eggLevel;
        targetLocalPosition = currentZoom + camDistance.localPosition;
    }

    public void ZoomOutCamera(int eggLevel)
    {
        Vector3 zoomValue = currentZoom - (zoomOut * eggLevel);
        camDistance.transform.DOLocalMove(zoomValue - camDistance.localPosition, zoomTime).SetEase(Ease.InBack);
    }

    public void CameraShake()
    {
        transform.DOShakeRotation(duration, strength, vibrato, randomness);
    }
}