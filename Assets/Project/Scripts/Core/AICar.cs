using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
//using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Core
{
    public class AICar : PathFollower
    {
        public int Level => level;
        [SerializeField] private int level;
        [SerializeField] private LevelText leveltext;
        [SerializeField] private new Collider collider;

        private int carLevel;

        private void OnValidate()
        {
            collider = GetComponent<Collider>();
        }

        private void Awake()
        {
            speed *= -1;
            if (leveltext == null) return;
            leveltext.UpdateText(level, $"Lv");
        }

        private void Update()
        {
            MoveToCurrentDistanceOnPath();
            
            transform.rotation*=Quaternion.AngleAxis(180,Vector3.right);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                OnPickCar();
                GameManager.Instance.player.PickAnimation();
            }
        }

        public void OnHitPlayer()
        {
            leveltext.gameObject.SetActive(false);
            collider.enabled = false;
            transform.DOScale(0, 0.1f);
        }

        public void OnPickCar()
        {
            if (GameManager.Instance.player.level < level)
            {
                GameManager.Instance.player.KnockBack();
                return;
            }

            OnHitPlayer();
            GameManager.Instance.player.eggParticle.Play();
            Instantiate(GameManager.Instance.player.collectablePrefab,
                GameManager.Instance.player.headTransform.position+new Vector3(0,0.8f,0), Quaternion.identity,
                GameManager.Instance.player.transform).Init(level);
          //  MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            GameManager.Instance.player.level += level;
            GameManager.Instance.player.CheckVisual();

            GameManager.Instance.player.levelText.UpdateText(GameManager.Instance.player.level, $"level");
        }
    }
}