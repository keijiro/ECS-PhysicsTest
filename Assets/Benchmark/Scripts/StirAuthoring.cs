using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Stir : IComponentData
{
    public float3 Angles;
    public float Frequency;
    public uint RandomSeed;
}

public class StirAuthoring : MonoBehaviour
{
    public float3 Angles = 10;
    public float Frequency = 1;
    public uint RandomSeed = 12345;

    class Baker : Baker<StirAuthoring>
    {
        public override void Bake(StirAuthoring src)
        {
            var data = new Stir()
            {
                Angles = src.Angles,
                Frequency = src.Frequency,
                RandomSeed = src.RandomSeed
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}
