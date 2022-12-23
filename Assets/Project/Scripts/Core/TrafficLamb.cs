using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class TrafficLamb : MonoBehaviour
    {
        [SerializeField] private Transform red;
        [SerializeField] private Transform yellow;
        [SerializeField] private Transform green;

        [SerializeField] private float duration;


       [SerializeField] private new Collider collider;

        private void OnValidate()
        {
            collider = GetComponent<Collider>();
        }

        private void Start()
        {
            StartCoroutine(nameof(LightRoutine));
        }

        private IEnumerator LightRoutine()
        {
            while (true)
            {
                collider.enabled = true;
                red.gameObject.SetActive(true);
                yellow.gameObject.SetActive(false);
                yield return new WaitForSeconds(duration * 0.8f);
                yellow.gameObject.SetActive(true);
                yield return new WaitForSeconds(duration);
                yellow.gameObject.SetActive(false);
                red.gameObject.SetActive(false);
                collider.enabled = false;
                green.gameObject.SetActive(true);
                yield return new WaitForSeconds(duration * 1.5f);
                yellow.gameObject.SetActive(true);
                green.gameObject.SetActive(false);
                yield return new WaitForSeconds(duration);
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