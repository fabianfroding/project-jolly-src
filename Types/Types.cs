using UnityEngine;

public static class Types
{
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
        public int damageAmount;
        public float damageRadius, stunDamageAmount, knockbackStrength;
        public Vector2 knockbackAngle, direction;
        public GameObject sfxPrefab;
    }
}
