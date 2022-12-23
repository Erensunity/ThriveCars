using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaptoPlay : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelFade;
    
    private void OnEnable()
    {
        InputPanel.Instance.OnPointerDownEvent.AddListener(OnPointerDown);
    }

    private void OnDisable()
    {
        InputPanel.Instance.OnPointerDownEvent.RemoveListener(OnPointerDown);
    }

    private void OnPointerDown()
    {
        panelFade.DOFade(0, 0.2f).OnComplete(() => gameObject.SetActive(false));
        GameManager.Instance.GameStart();
    }
}