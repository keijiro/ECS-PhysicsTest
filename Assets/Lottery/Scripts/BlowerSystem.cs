using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;

namespace EcsPhysicsTest.Lottery {

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct BlowerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var job = new BlowerJob
        {
            BlowerGroup = SystemAPI.GetComponentLookup<Blower>(),
            MassGroup = SystemAPI.GetComponentLookup<PhysicsMass>(),
            VelocityGroup = SystemAPI.GetComponentLookup<PhysicsVelocity>()
        };
        state.Dependency = job.Schedule(simulation, state.Dependency);
    }
}

[BurstCompile]
struct BlowerJob : ITriggerEventsJob
{
    [ReadOnly] public ComponentLookup<Blower> BlowerGroup;
    [ReadOnly] public ComponentLookup<PhysicsMass> MassGroup;
    public ComponentLookup<PhysicsVelocity> VelocityGroup;

    [BurstCompile]
    public void Execute(TriggerEvent ev)
    {
        var aIsBlower = BlowerGroup.HasComponent(ev.EntityA);
        var bIsBlower = BlowerGroup.HasComponent(ev.EntityB);

        var aIsObject = VelocityGroup.HasComponent(ev.EntityA);
        var bIsObject = VelocityGroup.HasComponent(ev.EntityB);

        // We only process blower/object case.
        // (Reject blower/blower and object/object cases.)
        if (!(aIsBlower ^ bIsBlower)) return;
        if (!(aIsObject ^ bIsObject)) return;

        var (blowerEntity, objectEntity) =
          aIsBlower ? (ev.EntityA, ev.EntityB) : (ev.EntityB, ev.EntityA);

        var blower = BlowerGroup[blowerEntity];
        var mass = MassGroup[objectEntity];
        var velocity = VelocityGroup.GetRefRW(objectEntity);

        velocity.ValueRW.ApplyLinearImpulse(mass, blower.Impulse);
    }
}

} // namespace EcsPhysicsTest.Lottery
