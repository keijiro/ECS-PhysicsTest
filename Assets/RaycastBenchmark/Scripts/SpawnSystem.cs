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
        var radius = config.SpawnRadius;
        var rand = new Random(config.RandomSeed);

        // Board instantiation
        var instances = state.EntityManager.Instantiate
          (config.BoardPrefab, config.BoardCount, Allocator.Temp);

        foreach (var entity in instances)
        {
            var pos = rand.NextFloat3InSphere() * radius;
            var rot = rand.NextQuaternionRotation();
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            xform.ValueRW = LocalTransform.FromPositionRotation(pos, rot);
        }

        // Line instantiation
        instances = state.EntityManager.Instantiate
          (config.LinePrefab, config.LineCount, Allocator.Temp);

        foreach (var entity in instances)
        {
            var pos = rand.NextFloat3(-1, 1) * radius * 1.5f;
            var rot = rand.NextQuaternionRotation();
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            xform.ValueRW = LocalTransform.FromPositionRotationScale(pos, rot, radius);
        }

        state.Enabled = false;
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
