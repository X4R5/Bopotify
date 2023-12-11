using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrade", order = 2)]
public class UpgradeScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class DashUpgrade
    {
        public float _perfectDashDistanceIncreasePercent;
        public float _goodDashDistanceIncreasePercent;
        public GameObject _explosionPrefabOnEnd;
        public float _explosionPrefabOnEndDamage;
        public GameObject _explosionPrefabOnStart;
        public float _explosionPrefabOnStartDamage;
    }

    [System.Serializable]
    public class AttackUpgrade
    {
        public float _perfectAttackIncreasePercent;
        public float _goodAttackIncreasePercent;
        public float _perfectAttackRangeIncreasePercent;
        public float _goodAttackRangeIncreasePercent;
    }

    public DashUpgrade _dashUpgrade;
    [Space(10)]
    public AttackUpgrade _attackUpgrade;
    [Space(10)]
    public float _damageDecreasePercent;
    public float _damageDecrease;
    public float _moveSpeedInreasePercent;

    [Space(10)]
    [TextArea(10, 12)]
    public string _upgradeDescription;
}
