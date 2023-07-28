using Unity.Mathematics;

namespace EcsPhysicsTest.StaticBenchmark {

public static class Common
{
    // "Sunflower seed" sampling pattern
    public static float3 GetSeedPoint(int i)
    {
        const float Phi = 1.618033988749894f;
        var t = math.PI * 2 / Phi * i;
        var l = math.sqrt(i);
        return math.float3(math.cos(t) * l, 0.5f, math.sin(t) * l);
    }

    // Stacked box position
    public static float3 GetBoxPosition(int i, int perStack)
      => GetSeedPoint(i / perStack) + math.float3(0, i % perStack, 0);

    // Stacked box rotation
    public static quaternion GetBoxRotation(int i, int perStack)
      => quaternion.RotateY(i * 0.7f);
}

} // namespace EcsPhysicsTest.StaticBenchmark
