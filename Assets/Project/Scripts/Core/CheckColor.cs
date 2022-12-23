using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Core
{
    public class CheckColor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer canvas;

        [SerializeField] private AICar aiCarLevel;
        [SerializeField] private PlayerCar carLevel;

        [SerializeField] private Color positive;
        [SerializeField] private Color negative;


        private void Awake()
        {
            carLevel = GameManager.Instance.player;
        }

        private void Update()
        {
            canvas.color = carLevel.level >= aiCarLevel.Level ? positive : negative;
        }
    }
}