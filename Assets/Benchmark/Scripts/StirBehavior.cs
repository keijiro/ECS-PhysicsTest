using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

namespace EcsPhysicsTest.Benchmark {

sealed class StirBehavior : MonoBehaviour
{
    [field:SerializeField] public float3 Angles = 10;
    [field:SerializeField] public float Frequency = 1;
    [field:SerializeField] public uint RandomSeed = 12345;

    void FixedUpdate()
    {
        var elapsed = Time.time;

        var freq = Frequency * math.float3(0.8f, 0.9f, 1.1f);
        var time = freq * (elapsed + 1000);

        var rot = Noise.Float3(time, RandomSeed);
        rot *= math.radians(Angles);

        GetComponent<Rigidbody>().MoveRotation(quaternion.Euler(rot));
    }
}

} // namespace EcsPhysicsTest.Benchmark
