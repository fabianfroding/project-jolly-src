using UnityEngine;

public static class Types
{
    public enum DamageType
    {
        ENVIRONMENT,
        PHYSICAL,
        MAGICAL
    }

    public enum World
    {
        TROLL_WORLD,
        ATLANTIS_WORLD,
        UNDEAD_WORLD,
        MOON_WORLD,
        SNOW_WORLD,
        DESERT_WORLD,
        DRAGON_WORLD,
        AUTUMN_WORLD,
        FINAL_WORLD,
        NONE
    }

    [System.Serializable]
    public struct BlockData
    {
        public float minBlockAngle, maxBlockAngle;
    }

    [System.Serializable]
    public struct DamageData
    {
        public GameObject source, target;
        public bool ranged, canBeParried;
        public int damageAmount;
        public float damageRadius, stunDamageAmount, knockbackStrength;
        public Vector2 knockbackAngle, direction;
        public DamageType damageType;
    }
}
