using UnityEngine;

public static class Types
{
    public enum DamageType
    {
        ENVIRONMENT,
        PHYSICAL,
        MAGICAL
    }

    [System.Serializable]
    public struct BlockData
    {
        [Range(-180f, 180f)] public float minAngle;
        [Range(-180f, 180f)] public float maxAngle;
        public bool showDebugVisuals;
    }

    [System.Serializable]
    public struct DamageData
    {
        public GameObject source, target;
        public bool ranged, canBeParried;
        public bool breaksHealth; // Used to break player's health to prevent recovery.
        public int damageAmount;
        public float damageRadius, stunDamageAmount, knockbackStrength;
        public Vector2 knockbackAngle, direction;
        public DamageType damageType;
        public GameObject sfxPrefab;
    }
}
