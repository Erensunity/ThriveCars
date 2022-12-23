using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateSpeed;
        
        private void Start()
        {
            StartCoroutine(nameof(RotateRoutine));
        }

        private IEnumerator RotateRoutine()
        {
            while (true)
            {
                transform.localRotation *= Quaternion.Euler(rotateSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}