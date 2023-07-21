using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Klak.Math;

namespace EcsPhysicsTest.Lottery {

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Lottery>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Lottery>();

        var instances = state.EntityManager.Instantiate
          (config.Prefab, config.SpawnCount, Allocator.Temp);

        var rand = new Random(config.RandomSeed);
        foreach (var entity in instances)
        {
            var pos = rand.NextFloat3InSphere() * config.SpawnRadius;
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            xform.ValueRW = LocalTransform.FromPosition(pos);
        }

        state.Enabled = false;
    }
}

} // namespace EcsPhysicsTest.Lottery
