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

    public void Execute(TriggerEvent triggerEvent)
    {
        var a_entity = triggerEvent.EntityA;
        var b_entity = triggerEvent.EntityB;

        var a_hasBlower = BlowerGroup.HasComponent(a_entity);
        var b_hasBlower = BlowerGroup.HasComponent(b_entity);

        var a_hasVelocity = VelocityGroup.HasComponent(a_entity);
        var b_hasVelocity = VelocityGroup.HasComponent(b_entity);

        if (!(a_hasBlower ^ b_hasBlower)) return;
        if (!(a_hasVelocity ^ b_hasVelocity)) return;

        var blower = BlowerGroup[a_hasBlower ? a_entity : b_entity];

        var v_entity = a_hasVelocity ? a_entity : b_entity;

        VelocityGroup.GetRefRW(v_entity).ValueRW.ApplyLinearImpulse
          (MassGroup[v_entity], math.float3(0, blower.Force, 0));
    }
}

} // namespace EcsPhysicsTest.Lottery
