using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class LevelEndGate : MonoBehaviour
    {
        [Header("AnimationSpecs")][SerializeField] private float endSize;
        [SerializeField] private float animationDuration;
        [SerializeField] private Ease ease;
        [SerializeField] private TextMeshPro incomeText;
        public float incomeMultiplier;
        private ParticleSystem confetti;
        

        private void Awake()
        {
            confetti = GetComponentInChildren<ParticleSystem>();
            incomeText = GetComponentInChildren<TextMeshPro>();
            incomeText.text = $"x{incomeMultiplier + PlayerPrefs.GetFloat(GameManager.PrefsStartingIncomeKey)}";
        }

        public void OnCollect()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            confetti.Play();
            transform.DOScale(endSize, animationDuration).SetEase(ease).SetLoops(2, LoopType.Yoyo);
        }
    }
}