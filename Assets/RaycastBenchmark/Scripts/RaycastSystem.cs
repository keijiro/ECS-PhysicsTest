using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace EcsPhysicsTest.RaycastBenchmark {

public partial struct RaycastSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Benchmark>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Benchmark>();
        var extent = config.SpawnRadius;

        var world =
          SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;

        var rand = new Random(config.RandomSeed + 100);
        for (var i = 0; i < config.Iteration; i++)
        {
            var p0 = rand.NextFloat2(-extent, extent);

            var ray = new RaycastInput()
              { Start = math.float3(-extent, p0),
                End   = math.float3( extent, p0),
                Filter = CollisionFilter.Default };

            var hit = new RaycastHit();
            world.CastRay(ray, out hit);
        }
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
