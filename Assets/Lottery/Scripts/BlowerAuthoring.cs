using Unity.Entities;
using UnityEngine;

namespace EcsPhysicsTest.Lottery {

public struct Blower : IComponentData
{
    public float Force;
}

public class BlowerAuthoring : MonoBehaviour
{
    public float Force = 1;

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
