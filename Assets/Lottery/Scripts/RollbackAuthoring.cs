using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcsPhysicsTest.Lottery {

public class Rollback : IComponentData
{
    public InputAction SaveAction;
    public InputAction LoadAction;
}

public class RollbackAuthoring : MonoBehaviour
{
    public InputAction SaveAction = null;
    public InputAction LoadAction = null;

    class Baker : Baker<RollbackAuthoring>
    {
        public override void Bake(RollbackAuthoring src)
        {
            var data = new Rollback()
            {
                SaveAction = src.SaveAction,
                LoadAction = src.LoadAction
            };
            AddComponentObject(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

} // namespace EcsPhysicsTest.Lottery
