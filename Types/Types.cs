using UnityEngine;

public static class Types
{
    public enum EBlockState
    {
        E_BlockFront,
        E_BlockAbove,
        E_BlockAll,
        E_BlockNone
    }

    [System.Serializable]
    public struct DamageData
    {
        public GameObject source, target;
        public bool ranged, canBeParried, canBeBlocked, isKillZone;
        public int damageAmount;
        public float stunDamageAmount, knockbackStrength;
        public Vector2 knockbackAngle, direction;
        public GameObject sfxPrefab;
    }

    [System.Serializable]
    public struct DialogueData
    {
        [TextArea] public string[] paragraphs;
    }
}
