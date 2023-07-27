using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace EcsPhysicsTest.DynamicBenchmark {

sealed class SpawnBehavior : MonoBehaviour
{
    [field:SerializeField] public GameObject Prefab = null;
    [field:SerializeField] public int3 Dimensions = 10;
    [field:SerializeField] public float Interval = 0.1f;
    [field:SerializeField] public uint RandomSeed = 12345;

    void Start()
    {
        var totalCount = Dimensions.x * Dimensions.y * Dimensions.z;

        var rand = new Random(RandomSeed);
        var offset = ((float3)Dimensions - 1) / 2;

        for (var xi = 0; xi < Dimensions.x; xi++)
            for (var yi = 0; yi < Dimensions.y; yi++)
                for (var zi = 0; zi < Dimensions.z; zi++)
                {
                    var p = math.float3(xi, yi, zi);
                    p += rand.NextFloat3(-0.1f, 0.1f) - offset;
                    p *= Interval;

                    var r = rand.NextQuaternionRotation();

                    var go = Instantiate(Prefab, p, r);
                }
    }
}

} // namespace EcsPhysicsTest.DynamicBenchmark
