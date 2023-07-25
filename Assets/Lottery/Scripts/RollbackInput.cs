using UnityEngine;
using UnityEngine.InputSystem;

namespace EcsPhysicsTest.Lottery {

public sealed class RollbackInput : MonoBehaviour
{
    [field:SerializeField] public InputAction SaveAction = null;
    [field:SerializeField] public InputAction LoadAction = null;

    void OnEnable()
    {
        SaveAction.performed += _ => RollbackSystem.RequestSave();
        LoadAction.performed += _ => RollbackSystem.RequestLoad();
        SaveAction.Enable();
        LoadAction.Enable();
    }
}

} // namespace EcsPhysicsTest.Lottery
