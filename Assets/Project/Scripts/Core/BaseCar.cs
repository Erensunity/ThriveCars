using System;
using Project.Scripts.Core;
using Project.Scripts.Utils;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public struct Point
{
    public Vector3 Position;
    public Quaternion Rotation;
}

public class BaseCar : PathFollower
{
    [SerializeField, Min(0.0f)] private float startDistance = 1.2f;
    [Title("Mesh")] [SerializeField] protected float distanceBetweenPoints = 0.025f;

    [Title("Head")] [SerializeField] public Transform headTransform;
    

    public bool canMove;
    public int level = 1;

    private void Awake()
    {
        distanceTravelled = startDistance;
        UpdateMesh();
    }

    private void Update()
    {
        if (canMove)
        {
            MoveNext();
        }

        UpdateMesh();

        if (canMove)
        {
            OnUpdate();
        }
    }

    protected virtual void OnUpdate()
    {
    }

    [Button]
    protected void UpdateMesh()
    {
        (Vector3 headPosition, Quaternion headRotation) = GetPositionAndRotation();
        headTransform.SetPositionAndRotation(headPosition, headRotation * Quaternion.AngleAxis(90, Vector3.forward));
    }
    
}