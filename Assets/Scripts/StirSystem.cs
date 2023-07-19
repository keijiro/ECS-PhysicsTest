using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Klak.Math;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
public partial struct StirSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var elapsed = (float)SystemAPI.Time.ElapsedTime;
        var tickSpeed = 1.0f / SystemAPI.Time.DeltaTime;

        foreach (var (xform, vel, mass, stir) in
                 SystemAPI.Query<RefRO<LocalTransform>,
                                 RefRW<PhysicsVelocity>,
                                 RefRO<PhysicsMass>,
                                 RefRO<Stir>>())
        {
            var freq = stir.ValueRO.Frequency * math.float3(0.8f, 0.9f, 1.1f);
            var time = freq * (elapsed + 1000);

            var rot = Noise.Float3(time, stir.ValueRO.RandomSeed);
            rot *= math.radians(stir.ValueRO.Angles);

            var target = new RigidTransform
              (quaternion.Euler(rot), xform.ValueRO.Position);

            vel.ValueRW = PhysicsVelocity.CalculateVelocityToTarget
              (mass.ValueRO, xform.ValueRO.Position, xform.ValueRO.Rotation,
               target, tickSpeed);
        }
    }
}
