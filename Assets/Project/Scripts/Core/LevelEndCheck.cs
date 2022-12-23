using System;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class LevelEndCheck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                GameManager.Instance.player.OnLevelEnd();
            }
        }
    }
}