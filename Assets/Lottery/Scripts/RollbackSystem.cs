using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Transforms;

namespace EcsPhysicsTest.Lottery {

[UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
public partial class RollbackSystem : SystemBase
{
    #region SystemBase overrides

    protected override void OnCreate()
      => RequireForUpdate<Rollback>();

    protected override void OnDestroy()
      => _record.Dispose();

    protected override void OnUpdate()
    {
        var rollback = SystemAPI.ManagedAPI.GetSingleton<Rollback>();
        if (!rollback.SaveAction.enabled)
        {
            rollback.SaveAction.performed += _ => SaveState();
            rollback.LoadAction.performed += _ => LoadState();
            rollback.SaveAction.Enable();
            rollback.LoadAction.Enable();
        }
    }

    #endregion

    #region Rollback system implementation

    NativeArray<(Entity entity, LocalTransform xform, PhysicsVelocity velocity)> _record;

    void LoadState()
    {
        if (!_record.IsCreated) return;

        Dependency.Complete();

        foreach (var (entity, xform, velocity) in _record)
        {
            EntityManager.SetComponentData(entity, xform);
            EntityManager.SetComponentData(entity, velocity);
        }
    }

    void SaveState()
    {
        Dependency.Complete();

        if (_record.IsCreated) _record.Dispose();

        var query = SystemAPI.QueryBuilder()
          .WithAll<LocalTransform, PhysicsVelocity>()
          .Build();

        _record = new NativeArray<(Entity, LocalTransform, PhysicsVelocity)>
          (query.CalculateEntityCount(), Allocator.Persistent);

        var i = 0;
        foreach (var (xform, velocity, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<PhysicsVelocity>>()
                          .WithEntityAccess())
            _record[i++] = (entity, xform.ValueRO, velocity.ValueRO);
    }

    #endregion
}

} // namespace EcsPhysicsTest.Lottery
