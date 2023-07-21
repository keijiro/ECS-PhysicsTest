using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.Lottery {

public struct Lottery : IComponentData
{
    public Entity Prefab;
    public int SpawnCount;
    public float SpawnRadius;
    public uint RandomSeed;
}

public class LotteryAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public int SpawnCount = 10;
    public float SpawnRadius = 1;
    public uint RandomSeed = 12345;

    class Baker : Baker<LotteryAuthoring>
    {
        public override void Bake(LotteryAuthoring src)
        {
            var data = new Lottery()
            {
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                SpawnCount = src.SpawnCount,
                SpawnRadius = src.SpawnRadius,
                RandomSeed = src.RandomSeed
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.Lottery
