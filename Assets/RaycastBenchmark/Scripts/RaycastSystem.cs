using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

namespace EcsPhysicsTest.RaycastBenchmark {

public partial struct RaycastSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Benchmark>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
        var palette = (hit: math.float4(1, 0, 0, 1), miss: 0.3f);

        foreach (var (xform, raycast, color) in
                 SystemAPI.Query<RefRO<LocalTransform>,
                                 RefRO<Raycast>,
                                 RefRW<URPMaterialPropertyBaseColor>>())
        {
            var ray = new RaycastInput()
              { Start = xform.ValueRO.TransformPoint(math.float3(0, 0, 0)),
                End   = xform.ValueRO.TransformPoint(math.float3(0, 0, 1)),
                Filter = CollisionFilter.Default };
            color.ValueRW.Value =
              world.CastRay(ray) ? palette.hit : palette.miss;
        }
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
