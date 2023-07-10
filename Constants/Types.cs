using UnityEngine;

public static class Types
{
    public enum DamageType
    {
        ENVIRONMENT,
        PHYSICAL,
        MAGICAL
    }

    public enum SceneTransitionPoint
    {
        EAST,
        NORTH,
        NORTHEAST,
        NORTHWEST,
        SOUTH,
        SOUTHEAST,
        SOUTHWEST,
        WEST,
        NONE
    }

    [System.Serializable]
    public struct DamageData
    {
        public GameObject source;
        public GameObject target;
        public bool ranged;
        public bool canBeParried;
        public int damageAmount;
        public float stunDamageAmount;
        public float knockbackStrength;
        public Vector2 knockbackAngle;
        public Vector2 direction;
        public DamageType damageType;
    }
}
