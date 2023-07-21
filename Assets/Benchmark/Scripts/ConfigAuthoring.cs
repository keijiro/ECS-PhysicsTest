using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Config : IComponentData
{
    public Entity Prefab;
    public int3 Dimensions;
    public float Interval;
    public uint RandomSeed;
}

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject Prefab = null;
    public int3 Dimensions = 10;
    public float Interval = 0.1f;
    public uint RandomSeed = 12345;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring src)
        {
            var data = new Config()
            {
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                Dimensions = src.Dimensions,
                Interval = src.Interval,
                RandomSeed = src.RandomSeed
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}
