using System;
using System.Collections;
using DG.Tweening;
//using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class Money : MonoBehaviour
    {
        public int value;

        [SerializeField] private new Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                OnCollect();
            }
        }

        public void OnCollect()
        {
            collider.enabled = false;
            transform.DOScale(0, 0.2f).SetEase(Ease.InBack);
            GameManager.Instance.currentGold += value;
            GameManager.Instance.player.eggParticle.Play();
           // MMVibrationManager.Haptic(HapticTypes.LightImpact);
            Instantiate(GameManager.Instance.player.collectablePrefab,
                GameManager.Instance.player.headTransform.position, Quaternion.identity,
                GameManager.Instance.player.transform).InitG(1);
            CoinAnimation coinAnimation = Instantiate(GameManager.Instance.player.coinAnimationPrefab,
                GameManager.Instance.player.canvas.transform);
            coinAnimation.SendMoney(GameManager.Instance.player.coinTarget,
                GameManager.Instance.player.headTransform.position);
            UIManager.Instance.UpdateCurrentCoin();
        }
    }
}