using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsPhysicsTest.StaticBenchmark {

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    #region ISystem implementation

    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<StaticBenchmark>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<StaticBenchmark>();

        var instances = state.EntityManager.Instantiate
          (config.Prefab, config.SpawnCount, Allocator.Temp);

        for (var i = 0; i < config.SpawnCount; i++)
            SetTransform(ref state, instances[i],
                         GetBoxPosition(i, config.StackCount),
                         quaternion.RotateY(i * 0.1f));

        state.Enabled = false;
    }

    #endregion

    #region Local methods

    // "Sunflower seed" sampling pattern
    float3 GetSeedPoint(int i)
    {
        const float Phi = 1.618033988749894f;
        var t = math.PI * 2 / Phi * i;
        var l = math.sqrt(i);
        return math.float3(math.cos(t) * l, 0.5f, math.sin(t) * l);
    }

    // Stacked box position
    float3 GetBoxPosition(int i, int perStack)
      => GetSeedPoint(i / perStack) + math.float3(0, i % perStack, 0);

    // Transform setter utility
    void SetTransform(ref SystemState state,
                      Entity entity, float3 pos, quaternion rot)
      => SystemAPI.GetComponentRW<LocalTransform>(entity).ValueRW =
           LocalTransform.FromPositionRotation(pos, rot);

    #endregion
}

} // namespace EcsPhysicsTest.Static
