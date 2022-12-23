using System;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class SelectedPath : MonoBehaviour
    {
        [Title("Path")] [SerializeField] public PathCreator path;

        private void OnEnable()
        {
            path = GetComponent<PathCreator>();
        }
    }
}