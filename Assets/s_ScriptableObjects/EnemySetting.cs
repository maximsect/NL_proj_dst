using UnityEngine;

[CreateAssetMenu(fileName = "EnemySetting", menuName = "Scriptable Objects/EnemySetting")]
public class EnemySetting : ScriptableObject
{
    public EnemyStatus enemyStatus;
}
[System.Serializable]
public class EnemyStatus
{
    public float health = 5;
    public float moveSpeed = 5;//ˆÚ“®‘¬“x
    public float attackPower;//“GUŒ‚—Í
    public float colAttackPower;//“G‚É‚Ô‚Â‚©‚Á‚½‚Ìƒ_ƒ[ƒW
    public float attackXP;//UŒ‚‚µ‚½‚Æ‚«‚Ì“¾“_
    public float destroyXP;//“|‚µ‚½‚Ì“¾“_
}
