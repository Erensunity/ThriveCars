using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpiralMeshGenerator : MonoBehaviour
{
    [SerializeField] private float upwardSpeed = 1f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float targetYPosition = 1f;
    [SerializeField] private int segments = 100;
    [SerializeField] private PathCreator pathCreator;
    
    [Button]
    private void CreateSpiralSpline()
    {
        List<Vector3> points = GetSpiralPositions(upwardSpeed, radius, targetYPosition, segments);
        pathCreator.SetPoints(points);
    }
    
    public static List<Vector3> GetSpiralPositions(float upwardSpeed = 0.5f, float radius = 0.5f, float targetYPos = 5.0f, int segmentAmount = 16)
    {
        List<Vector3> positions = new List<Vector3>();

        float yPosition = 0.0f;

        List<Vector3> circle = GetCircularPositions(Vector3.zero, segmentAmount, radius);
        while (yPosition <= targetYPos)
        {
            for (int i = 0; i < circle.Count; i++)
            {
                positions.Add(new Vector3(circle[i].x, yPosition, circle[i].y));
                yPosition += upwardSpeed;
            }
        }

        return positions;
    }

    private static List<Vector3> GetCircularPositions(Vector3 startPosition, int amount, float radius)
    {
        List<Vector3> positions = new List<Vector3>();

        float segmentWidth = Mathf.PI * 2.0f / amount;

        Vector3 dir = Vector3.zero;

        float posRadius = radius * 2.0f;
        float angle = 0.0f;
        for (int i = 0; i < amount; i++)
        {
            dir.x = Mathf.Cos(angle) * radius;
            dir.y = Mathf.Sin(angle) * radius;

            Vector3 position = startPosition + dir * posRadius;
            positions.Add(position);

            angle -= segmentWidth;
        }

        return positions;
    }
}
