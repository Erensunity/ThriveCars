using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Core
{
    public class RandomBackGround : MonoBehaviour
    {
        [SerializeField] private MeshRenderer materialColor;
        [SerializeField] private Color[] colors1;
        [SerializeField] private Color[] colors2;
        [SerializeField] private Color[] colors3;
        [SerializeField] private Color[] colors4;
        [SerializeField] private Color[] colors5;
        [SerializeField] private Color[] colors6;
        [SerializeField] private Color[] colors7;

        [SerializeField] private List<Color[]> colorSets = new List<Color[]>();
        
        private void Awake()
        {
            colorSets.Add(colors1);
            colorSets.Add(colors2);
            colorSets.Add(colors3);
            colorSets.Add(colors4);
            colorSets.Add(colors5);
            colorSets.Add(colors6);
            colorSets.Add(colors7);

            int colorIndex=Random.Range(0, colorSets.Count);
            materialColor.material.SetColor("_Color1",colorSets[colorIndex][0]);
            materialColor.material.SetColor("_Color2",colorSets[colorIndex][1]);
        }
    }
}