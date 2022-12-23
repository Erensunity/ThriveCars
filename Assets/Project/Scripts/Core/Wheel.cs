using System;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private PlayerCar player;
        [SerializeField] private Vector3 rotate;

        private void Update()
        {
            if (!player.canMove) return;
            transform.localRotation *= Quaternion.Euler(rotate);
        }
    }
}