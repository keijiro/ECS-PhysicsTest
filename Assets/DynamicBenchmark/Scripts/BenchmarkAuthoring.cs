using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.DynamicBenchmark {

public struct Benchmark : IComponentData
{
    public Entity Prefab;
    public int3 Dimensions;
    public float Interval;
    public uint RandomSeed;
}

public class BenchmarkAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public int3 Dimensions = 10;
    public float Interval = 0.1f;
    public uint RandomSeed = 12345;

    class Baker : Baker<BenchmarkAuthoring>
    {
        public override void Bake(BenchmarkAuthoring src)
        {
            var data = new Benchmark()
            {
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                Dimensions = src.Dimensions,
                Interval = src.Interval,
                RandomSeed = src.RandomSeed
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.DynamicBenchmark
