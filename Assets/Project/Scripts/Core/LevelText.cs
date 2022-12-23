using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class LevelText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro levelText;

        private void LateUpdate()
        {
            Vector3 direction;
            direction = transform.position - Camera.main.transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        public void UpdateText(int level,string text)
        {
            levelText.text = $"{text} {level}";
            levelText.transform.DOKill();
            levelText.transform.localScale = Vector3.one * 0.4f;
            levelText.transform.DOScale(0.5f, 0.2f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo);
        }
    }
}