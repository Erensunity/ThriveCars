using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Core
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateSpeed;
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;
        [SerializeField] private float waitingTime;

        private new Collider collider;
        private SplinePositioner positioner;

        private Vector3 mRotation;

        private void Awake()
        {
            mRotation = transform.localRotation.eulerAngles;
            collider = GetComponent<Collider>();
            positioner = GetComponentInParent<SplinePositioner>();
        }
        

        private void Start()
        {
            StartCoroutine(nameof(RollRoutine));
            StartCoroutine(nameof(RandomRotateRoutine));
        }
        
        private IEnumerator RollRoutine()
        {
            while (true)
            {
                transform.localRotation *= Quaternion.Euler(rotateSpeed);
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator RandomRotateRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));
                StopCoroutine(nameof(RollRoutine));
                transform.DOLocalRotate(mRotation, 0.3f);
                yield return new WaitForSeconds(waitingTime);
                StartCoroutine(nameof(RollRoutine));
            }
  

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