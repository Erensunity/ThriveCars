using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class Guillotine : MonoBehaviour
    {
        [SerializeField] private float movePosition;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private SplinePositioner positioner;

        private new Collider collider;
        private void Awake()
        {
            collider = GetComponent<Collider>();
            positioner = GetComponentInParent<SplinePositioner>();
        }

        private void Start()
        {
            transform.DOLocalMoveY(movePosition, duration).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                GameManager.Instance.player.KnockBack();
            }
        }
    }
}