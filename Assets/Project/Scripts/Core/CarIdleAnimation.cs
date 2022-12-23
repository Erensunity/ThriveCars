using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class CarIdleAnimation : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.DOShakeRotation(0.2f, 1, 2).SetEase(Ease.InOutQuart).SetLoops(-1,LoopType.Yoyo);
        }
    }
}