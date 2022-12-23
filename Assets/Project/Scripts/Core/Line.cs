using UnityEngine;

namespace Project.Scripts.Core
{
    public class Line : MonoBehaviour
    {
        [SerializeField] public LineRenderer lineRenderer;

        public void AddPosition(Vector3 position)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount++, position);
        }
    }
}