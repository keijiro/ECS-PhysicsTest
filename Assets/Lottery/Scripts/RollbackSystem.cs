using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Transforms;

namespace EcsPhysicsTest.Lottery {

[UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
public partial class RollbackSystem : SystemBase
{
    NativeArray<(LocalTransform xform, PhysicsVelocity velocity)> _record;

    public static void RequestSave() => _request.save = true;
    public static void RequestLoad() => _request.load = true;

    static (bool save, bool load) _request;

    protected override void OnCreate()
      => RequireForUpdate<Lottery>();

    void LoadState()
    {
        if (!_record.IsCreated) return;

        var record = _record;
        var i = 0;

        Entities.ForEach((ref LocalTransform xform,
                          ref PhysicsVelocity velocity) =>
            {
                xform = record[i].xform;
                velocity = record[i].velocity;
                i++;
            }
        ).Schedule();
    }

    void SaveState()
    {
        var count = 0;
        Entities.ForEach((in LocalTransform xform,
                          in PhysicsVelocity velocity) => count++).Run();

        if (_record.IsCreated) _record.Dispose();

        _record = new NativeArray<(LocalTransform, PhysicsVelocity)>
          (count, Allocator.Persistent);
        var record = _record;

        var i = 0;
        Entities.ForEach((in LocalTransform xform,
                          in PhysicsVelocity velocity) =>
            {
                record[i++] = (xform, velocity);
            }
        ).Run();
    }

    protected override void OnUpdate()
    {
        if (_request.save|| true) SaveState();
        if (_request.load) LoadState();
        _request = (false, false);
    }
}

/*
[UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
public partial struct RollbackSystem : ISystem
{
    NativeArray<(LocalTransform xform, PhysicsVelocity velocity)> _record;

    public static void RequestSave() => _saveRequested = true;
    public static void RequestLoad() => _loadRequested = true;

    static bool _saveRequested;
    static bool _loadRequested;

    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Lottery>();

    void LoadState(ref SystemState state)
    {
        if (!_record.IsCreated) return;

        var i = 0;
        foreach (var (xform, velocity) in
                 SystemAPI.Query<RefRW<LocalTransform>,
                                 RefRW<PhysicsVelocity>>())
        {
            xform.ValueRW = _record[i].xform;
            velocity.ValueRW = _record[i].velocity;
            i++;
        }
    }

    void SaveState(ref SystemState state)
    {
        var count = 0;
        foreach (var (xform, velocity) in
                 SystemAPI.Query<RefRO<LocalTransform>,
                                 RefRO<PhysicsVelocity>>()) count++;

        if (_record.IsCreated) _record.Dispose();

        _record = new NativeArray<(LocalTransform, PhysicsVelocity)>
          (count, Allocator.Persistent);

        var i = 0;
        foreach (var (xform, velocity) in
                 SystemAPI.Query<RefRO<LocalTransform>,
                                 RefRO<PhysicsVelocity>>())
        {
            _record[i++] = (xform.ValueRO, velocity.ValueRO);
        }
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (_saveRequested) SaveState(ref state);
        if (_loadRequested) LoadState(ref state);
        _saveRequested = _loadRequested = false;
    }
}
*/

} // namespace EcsPhysicsTest.Lottery
