using PathCreation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project.Scripts.Utils
{
    public class MeshGenerationSettings
    {
        public int SegmentCount;
        public int PointCount;
        public float StartAngle;
        public float Distance;
        public float DistanceBetweenPoints;
        public PathCreator Path;

        public AnimationCurve BodyCurve;
        public float MinRadius;
        public float MaxRadius;

        public int EatBumpStart;
        public int EatBumpLength;
        public AnimationCurve CurrentAnimationCurve;
    }

    public static class MeshGenerator
    {
        private static (Vector3, Quaternion) GetPositionAndRotation(PathCreator path, float distance)
        {
            return (path.path.GetPointAtDistance(distance), path.path.GetRotationAtDistance(distance));
        }

        private static NativeArray<Point> GetPoints(MeshGenerationSettings settings)
        {
            NativeArray<Point> points = new NativeArray<Point>(settings.PointCount, Allocator.Persistent);

            for (int i = 0; i < settings.PointCount; i++)
            {
                (Vector3 position, Quaternion rotation) = GetPositionAndRotation(settings.Path,
                    settings.Distance - (i * settings.DistanceBetweenPoints));

                float offset = Mathf.Lerp(settings.MinRadius, settings.MaxRadius,
                    settings.BodyCurve.Evaluate(i / (float) settings.PointCount));

                Vector3 offsetAmount = (((rotation * Quaternion.AngleAxis(90, Vector3.forward)) * Vector3.up) * offset);
                Point point = new Point
                {
                    Position = position + offsetAmount,
                    Rotation = rotation
                };
                points[i] = point;
            }

            return points;
        }

        public static void CalculateCircularPositions(Point point, int segmentCount, float startAngle,
            float circleRadius, ref Vector3[] circularPositions)
        {
            float segmentWidth = Mathf.PI * 2.0f / segmentCount;
            float angle = startAngle;

            Vector3 dummyVector = Vector3.zero;
            Matrix4x4 transformMat = Matrix4x4.TRS(point.Position, point.Rotation, Vector3.one);
            for (int i = 0; i < segmentCount; i++)
            {
                dummyVector.x = Mathf.Cos(angle) * circleRadius;
                dummyVector.y = Mathf.Sin(angle) * circleRadius;

                circularPositions[i] = transformMat.MultiplyPoint3x4(dummyVector);

                angle -= segmentWidth;
            }
        }

        public static void GenerateMesh(MeshGenerationSettings settings, ref Mesh mesh)
        {
            NativeArray<Point> points = GetPoints(settings);
            int trisCount = (settings.PointCount - 1) * settings.SegmentCount * 6;
            int verticesCount = settings.PointCount * settings.SegmentCount;

            Mesh.MeshDataArray dataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData data = dataArray[0];

            data.SetVertexBufferParams(verticesCount,
                new VertexAttributeDescriptor(VertexAttribute.Position),
                new VertexAttributeDescriptor(VertexAttribute.Normal, stream: 1));
            data.SetIndexBufferParams(trisCount, IndexFormat.UInt32);

            NativeArray<Vector2> uvs = new NativeArray<Vector2>(verticesCount, Allocator.TempJob);

            int index = 0;
            for (int y = 0; y < settings.PointCount; y++)
            {
                for (int x = 0; x < settings.SegmentCount; x++)
                {
                    uvs[index++] = new Vector2(x / (float) (settings.SegmentCount - 1), y / (float) (settings.PointCount - 1));
                }
            }

            NativeArray<Vector3> vertices = data.GetVertexData<Vector3>();
            NativeArray<int> tris = data.GetIndexData<int>();

            Vector3[] circularPositions = new Vector3[settings.SegmentCount];

            for (int i = 0; i < settings.PointCount; i++)
            {
                float radii = Mathf.Lerp(settings.MinRadius, settings.MaxRadius, settings.BodyCurve.Evaluate(i / (float) settings.PointCount));
                if (i >= settings.EatBumpStart - settings.EatBumpLength && i <= settings.EatBumpStart + settings.EatBumpLength)
                {
                    float time =
                        Mathf.Clamp01(1.0f - (Mathf.Abs(settings.EatBumpStart - i) / (float) settings.EatBumpLength));
                    radii *= settings.CurrentAnimationCurve.Evaluate(time);
                }

                CalculateCircularPositions(points[i], settings.SegmentCount, settings.StartAngle, radii, ref circularPositions);
                int count = circularPositions.Length;
                for (int j = 0; j < count; j++)
                {
                    vertices[(i * settings.SegmentCount) + j] = circularPositions[j];
                }
            }

            points.Dispose();

            int trisIndex = 0;

            //FRONT FACE
            for (int i = 0; i < settings.PointCount - 1; i++)
            {
                for (int j = 0; j < settings.SegmentCount; j++)
                {
                    int index1 = i * settings.SegmentCount + j;
                    int index2 = (i + 1) * settings.SegmentCount + j;
                    int index3 = (i + 1) * settings.SegmentCount + j + 1;
                    int index4 = i * settings.SegmentCount + j + 1;

                    if (j + 1 >= settings.SegmentCount)
                    {
                        index4 = i * settings.SegmentCount;
                        index3 = (i + 1) * settings.SegmentCount;
                    }

                    tris[trisIndex++] = index1;
                    tris[trisIndex++] = index4;
                    tris[trisIndex++] = index3;

                    tris[trisIndex++] = index1;
                    tris[trisIndex++] = index3;
                    tris[trisIndex++] = index2;
                }
            }

            mesh.Clear();

            data.subMeshCount = 1;
            data.SetSubMesh(0, new SubMeshDescriptor(0, trisCount));
            Mesh.ApplyAndDisposeWritableMeshData(dataArray, mesh);

            mesh.SetUVs(0, uvs);
            uvs.Dispose();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}