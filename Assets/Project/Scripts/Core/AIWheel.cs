using UnityEngine;

namespace Project.Scripts.Core
{
    public class AIWheel : MonoBehaviour
    {
        [SerializeField] private Vector3 rotate;

        private void Update()
        {
            transform.localRotation *= Quaternion.Euler(rotate);
        }
    }
}