using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Klak.Math;

namespace EcsPhysicsTest.RaycastBenchmark {

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Benchmark>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Benchmark>();

        var instances = state.EntityManager.Instantiate
          (config.Prefab, config.SpawnCount, Allocator.Temp);

        var rand = new Random(config.RandomSeed);
        foreach (var entity in instances)
        {
            var pos = rand.NextFloat3InSphere() * config.SpawnRadius;
            var rot = rand.NextQuaternionRotation();
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            xform.ValueRW = LocalTransform.FromPositionRotation(pos, rot);
        }

        state.Enabled = false;
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
