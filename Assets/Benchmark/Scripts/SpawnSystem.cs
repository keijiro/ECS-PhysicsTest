using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Config>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        var totalCount = config.Dimensions.x * config.Dimensions.y * config.Dimensions.z;
        var instances = state.EntityManager.Instantiate(config.Prefab, totalCount, Allocator.Temp);

        var rand = new Random(config.RandomSeed);
        var offset = ((float3)config.Dimensions - 1) / 2;
        var index = 0;

        for (var xi = 0; xi < config.Dimensions.x; xi++)
            for (var yi = 0; yi < config.Dimensions.y; yi++)
                for (var zi = 0; zi < config.Dimensions.z; zi++)
                {
                    var p = math.float3(xi, yi, zi);
                    p += rand.NextFloat3(-0.1f, 0.1f) - offset;
                    p *= config.Interval;

                    var r = rand.NextQuaternionRotation();

                    var xform = SystemAPI.GetComponentRW<LocalTransform>(instances[index++]);
                    xform.ValueRW = LocalTransform.FromPositionRotation(p, r);
                }

        state.Enabled = false;
    }
}
