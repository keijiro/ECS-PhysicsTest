using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Transforms;

namespace EcsPhysicsTest.Lottery {

[UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
public partial class RollbackSystem : SystemBase
{
    public static void RequestSave() => _request.save = true;
    public static void RequestLoad() => _request.load = true;

    static (bool save, bool load) _request;

    NativeArray<(LocalTransform xform, PhysicsVelocity velocity)> _record;

    protected override void OnCreate()
      => RequireForUpdate<Lottery>();

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

    protected override void OnUpdate()
    {
        if (_request.save) SaveState();
        if (_request.load) LoadState();
        _request = (false, false);
    }
}

} // namespace EcsPhysicsTest.Lottery
