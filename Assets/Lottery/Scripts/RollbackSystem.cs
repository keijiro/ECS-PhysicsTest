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

    NativeArray<(LocalTransform xform, PhysicsVelocity velocity)> _record;

    void LoadState()
    {
        if (!_record.IsCreated) return;

        var (record, i) = (_record, 0);
        Entities.ForEach( (ref LocalTransform xform,
                           ref PhysicsVelocity velocity) =>
                          (xform, velocity) = record[i++] ).Schedule();
    }

    void SaveState()
    {
        if (_record.IsCreated) _record.Dispose();

        var count = 0;
        Entities.ForEach( (in LocalTransform xform,
                           in PhysicsVelocity velocity) =>
                          count++ ).Run();

        _record = new NativeArray<(LocalTransform, PhysicsVelocity)>
          (count, Allocator.Persistent);

        var (record, i) = (_record, 0);
        Entities.ForEach( (in LocalTransform xform,
                           in PhysicsVelocity velocity) =>
                          record[i++] = (xform, velocity) ).Schedule();
    }

    #endregion
}

} // namespace EcsPhysicsTest.Lottery
