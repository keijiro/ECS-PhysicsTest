using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace EcsPhysicsTest.StaticBenchmark {

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<StaticBenchmark>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<StaticBenchmark>();

        var instances = state.EntityManager.Instantiate
          (config.Prefab, config.SpawnCount, Allocator.Temp);

        for (var i = 0; i < config.SpawnCount; i++)
            SystemAPI.GetComponentRW<LocalTransform>(instances[i]).ValueRW =
              LocalTransform.FromPositionRotation
                (Common.GetBoxPosition(i, config.StackCount),
                 Common.GetBoxRotation(i, config.StackCount));

        state.Enabled = false;
    }
}

} // namespace EcsPhysicsTest.StaticBenchmark
