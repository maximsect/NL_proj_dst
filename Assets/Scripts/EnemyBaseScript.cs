using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseScript : MonoBehaviour
{
    public int hp = 30;
    private bool isInvincible = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return;
        switch (collision.collider.gameObject.tag)
        {
            case "bat":
                hp -= PlayerData.main.batAttack;
                break;
            case "spear":
                hp -= PlayerData.main.spearAttack;
                break;
            case "bow":
                hp -= PlayerData.main.bowAttack;
                break;
            case "hammer":
                hp -= PlayerData.main.hammerAttack;
                break;
            case "arrow":
                hp -= PlayerData.main.arrowAttack;
                break;
            case "skillattack":
                hp -= PlayerData.main.skillAttack;
                break;
            default:
                break;
        }
        isInvincible = true;
        if (hp <= 0)
            Destroy(this.gameObject);
        KnockBack();

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (GameManager.main.playerTag.Contains(collision.collider.gameObject.tag))
        {
            isInvincible = false;
        }
    }
    public virtual void KnockBack() { }
}
