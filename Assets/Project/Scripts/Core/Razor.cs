using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class Razor : MonoBehaviour
    {
        [SerializeField] private float movePosition;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private Vector3 rotateSpeed;

        
        private new Collider collider;
        private SplinePositioner positioner;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            positioner = GetComponentInParent<SplinePositioner>();
        }

        private void Start()
        {
            transform.DOLocalMoveX(movePosition, duration).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
            StartCoroutine(nameof(RotationRoutine));
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                GameManager.Instance.player.KnockBack();
            }

            if (other.CompareTag("Body"))
            {
                collider.enabled = false;
            }
        }
        private IEnumerator RotationRoutine()
        {
            while (true)
            {
                transform.localRotation *= Quaternion.Euler(rotateSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}