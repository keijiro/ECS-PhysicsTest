using UnityEngine;

namespace EcsPhysicsTest.StaticBenchmark {

sealed class SpawnBehavior : MonoBehaviour
{
    [field:SerializeField] public GameObject Prefab = null;
    [field:SerializeField] public int SpawnCount = 100;
    [field:SerializeField] public int StackCount = 5;

    void Start()
    {
        for (var i = 0; i < SpawnCount; i++)
            Instantiate(Prefab,
                        Common.GetBoxPosition(i, StackCount),
                        Common.GetBoxRotation(i, StackCount));
    }
}

} // namespace EcsPhysicsTest.StaticBenchmark
