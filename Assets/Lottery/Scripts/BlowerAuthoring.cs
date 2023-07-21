using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.Lottery {

public struct Blower : IComponentData
{
    public float3 Impulse;
}

public class BlowerAuthoring : MonoBehaviour
{
    public float3 Impulse = math.float3(0, 1, 0);

    class Baker : Baker<BlowerAuthoring>
    {
        public override void Bake(BlowerAuthoring src)
        {
            var data = new Blower() { Impulse = src.Impulse };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.Lottery
