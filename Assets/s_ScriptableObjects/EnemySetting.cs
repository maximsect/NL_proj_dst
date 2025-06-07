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
    public float moveSpeed = 5;//�ړ����x
    public float attackPower;//�G�U����
    public float colAttackPower;//�G�ɂԂ��������̃_���[�W
    public float attackXP;//�U�������Ƃ��̓��_
    public float destroyXP;//�|�������̓��_
}
