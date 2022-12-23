using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Core
{
    public abstract class PathFollower : MonoBehaviour
    {
        [Title("Path")] [SerializeField] public PathCreator path;
        [SerializeField] public float speed;
        [SerializeField] public float distanceTravelled;

        protected void MoveNext()
        {
            distanceTravelled += Time.deltaTime * speed;
        }

        protected void MoveToCurrentDistanceOnPath()
        {
            MoveNext();

            (Vector3 position, Quaternion rotation) = GetPositionAndRotation();
            transform.SetPositionAndRotation(position, rotation);
        }

        protected (Vector3, Quaternion) GetPositionAndRotation()
        {
            return GetPositionAndRotation(distanceTravelled);
        }
        
        protected (Vector3, Quaternion) GetPositionAndRotation(float distance)
        {
            return (path.path.GetPointAtDistance(distance),
                path.path.GetRotationAtDistance(distance));
        }
    }
}