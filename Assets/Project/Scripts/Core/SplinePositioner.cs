using System;
using PathCreation;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class SplinePositioner : MonoBehaviour
    {
        [SerializeField] private PathCreator path;
        [SerializeField] public float distanceOnPosition;
        [SerializeField] private EndOfPathInstruction instruction = EndOfPathInstruction.Loop;
        
        [SerializeField] private bool isTime;
        
        private void OnValidate()
        {
            if (!path)
            {
                return;
            }

            if (isTime)
            {
                
                transform.position = path.path.GetPointAtTime(Mathf.Clamp01(distanceOnPosition),instruction);
                transform.rotation = path.path.GetRotation(Mathf.Clamp01(distanceOnPosition),instruction) *
                                     Quaternion.AngleAxis(90, Vector3.forward);
                
                return;
            }
            transform.position = path.path.GetPointAtDistance(distanceOnPosition,instruction);
            transform.rotation = path.path.GetRotationAtDistance(distanceOnPosition,instruction) *
                                 Quaternion.AngleAxis(90, Vector3.forward);
        }
    }
}