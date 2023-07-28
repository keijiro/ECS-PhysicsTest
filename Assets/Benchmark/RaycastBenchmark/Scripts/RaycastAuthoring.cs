using Unity.Entities;
using UnityEngine;

namespace EcsPhysicsTest.RaycastBenchmark {

public struct Raycast : IComponentData {}

public class RaycastAuthoring : MonoBehaviour
{
    class Baker : Baker<RaycastAuthoring>
    {
        public override void Bake(RaycastAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Raycast());
    }
}

} // namespace EcsPhysicsTest.RaycastBenchmark
