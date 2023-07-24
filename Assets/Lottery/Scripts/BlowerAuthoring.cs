using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsPhysicsTest.Lottery {

public struct Blower : IComponentData
{
    public float3 Force;
}

public class BlowerAuthoring : MonoBehaviour
{
    public float3 Force = math.float3(0, 100, 0);

    class Baker : Baker<BlowerAuthoring>
    {
        public override void Bake(BlowerAuthoring src)
        {
            var data = new Blower() { Force = src.Force };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.Lottery
