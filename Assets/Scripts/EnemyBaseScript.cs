using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseScript : MonoBehaviour
{
    public int enemyHp = 30;
    protected float invincibleTimer = 0;
    void OnTriggerStay2D(Collider2D collider)
    {
        if (GameManager.playerWeaponTag.Contains(collider.gameObject.tag) && invincibleTimer < 0)
        {
            invincibleTimer = 0.1f;
            switch (collider.gameObject.tag)
            {
                case "bat":
                    enemyHp -= PlayerData.main.batAttack;
                    break;
                case "spear":
                    enemyHp -= PlayerData.main.spearAttack;
                    break;
                case "bow":
                    enemyHp -= PlayerData.main.bowAttack;
                    break;
                case "hammer":
                    enemyHp -= PlayerData.main.hammerAttack;
                    break;
                case "arrow":
                    enemyHp -= PlayerData.main.arrowAttack;
                    break;
                case "skillattack":
                    enemyHp -= PlayerData.main.skillAttack;
                    break;
                default:
                    break;
            }
            if (enemyHp <= 0)
                Destroy(this.gameObject);
        }

    }
    void Update()
    {
        invincibleTimer -= Time.deltaTime;
        GetComponent<SpriteRenderer>().color = (invincibleTimer > 0) ? new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);
        SubUpdate();
    }
    public virtual void SubUpdate() { }
}
