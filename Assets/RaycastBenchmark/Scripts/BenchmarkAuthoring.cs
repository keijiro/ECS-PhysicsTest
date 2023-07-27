using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.RaycastBenchmark {

public struct Benchmark : IComponentData
{
    public Entity BoardPrefab;
    public int BoardCount;

    public Entity LinePrefab;
    public int LineCount;

    public float SpawnRadius;
    public uint RandomSeed;
}

public class BenchmarkAuthoring : MonoBehaviour
{
    public GameObject BoardPrefab = null;
    public int BoardCount = 100;

    public GameObject LinePrefab = null;
    public int LineCount = 100;

    public float SpawnRadius = 1;
    public uint RandomSeed = 12345;

    class Baker : Baker<BenchmarkAuthoring>
    {
        public override void Bake(BenchmarkAuthoring src)
        {
            var data = new Benchmark()
            {
                BoardPrefab = GetEntity
                  (src.BoardPrefab, TransformUsageFlags.Dynamic),
                BoardCount = src.BoardCount,

                LinePrefab = GetEntity
                  (src.LinePrefab, TransformUsageFlags.Dynamic),
                LineCount = src.LineCount,

                SpawnRadius = src.SpawnRadius,
                RandomSeed = src.RandomSeed,
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
