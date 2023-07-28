using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.StaticBenchmark {

public struct Benchmark : IComponentData
{
    public Entity Prefab;
    public int SpawnCount;
    public int StackCount;
}

public class BenchmarkAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public int SpawnCount = 100;
    public int StackCount = 5;

    class Baker : Baker<BenchmarkAuthoring>
    {
        public override void Bake(BenchmarkAuthoring src)
        {
            var data = new Benchmark()
            {
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                SpawnCount = src.SpawnCount,
                StackCount = src.StackCount
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.StaticBenchmark
