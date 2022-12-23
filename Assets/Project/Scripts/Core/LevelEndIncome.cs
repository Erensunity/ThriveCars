using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelEndIncome : MonoBehaviour
{
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private new Collider collider;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private int nextLevel;


    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            StartCoroutine(nameof(LevelCheckRoutine));
        }
    }

    private IEnumerator LevelCheckRoutine()
    {
        confetti.Play();
        collider.enabled = false;
        levelText.transform.DOKill();
        levelText.transform.localScale = Vector3.one;
        levelText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo);
        if (nextLevel > GameManager.Instance.player.level)
        {
            GameManager.Instance.player.speed = 0;
            yield return new WaitForSeconds(1f);
            GameManager.Instance.LevelWin();
        }
    }
}