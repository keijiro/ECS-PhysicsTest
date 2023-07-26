using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.StaticBenchmark {

public struct StaticBenchmark : IComponentData
{
    public Entity Prefab;
    public int SpawnCount;
    public int StackCount;
}

public class StaticBenchmarkAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public int SpawnCount = 100;
    public int StackCount = 5;

    class Baker : Baker<StaticBenchmarkAuthoring>
    {
        public override void Bake(StaticBenchmarkAuthoring src)
        {
            var data = new StaticBenchmark()
            {
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                SpawnCount = src.SpawnCount,
                StackCount = src.StackCount
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.Static
