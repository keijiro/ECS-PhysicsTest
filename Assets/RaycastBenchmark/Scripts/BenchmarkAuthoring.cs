using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.RaycastBenchmark {

public struct Benchmark : IComponentData
{
    public Entity Prefab;
    public int SpawnCount;
    public float SpawnRadius;
    public uint RandomSeed;
    public int Iteration;
}

public class BenchmarkAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public int SpawnCount = 10;
    public float SpawnRadius = 1;
    public uint RandomSeed = 12345;
    public int Iteration = 100;

    class Baker : Baker<BenchmarkAuthoring>
    {
        public override void Bake(BenchmarkAuthoring src)
        {
            var data = new Benchmark()
            {
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                SpawnCount = src.SpawnCount,
                SpawnRadius = src.SpawnRadius,
                RandomSeed = src.RandomSeed,
                Iteration = src.Iteration
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
