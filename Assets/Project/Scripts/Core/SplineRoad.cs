using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;

public class SplineRoad : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float yOffset;
    [SerializeField] private float stepLength;
    
    [Button]
    private void Generate()
    {
        lineRenderer.positionCount = 0;

        float t = 0.0f;

        while (t <= 1.0f)
        {
            Vector3 position=pathCreator.path.GetPointAtTime(t);
            position.y += yOffset;
            lineRenderer.SetPosition(lineRenderer.positionCount++, position);
            t += Time.fixedDeltaTime * stepLength;
        }
    }
}
