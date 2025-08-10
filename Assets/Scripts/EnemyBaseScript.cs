using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyBaseScript : MonoBehaviour
{
    public SceneData sceneData;
    protected float powerUpRatio = 1;
    public int enemyHp = 30;
    protected float invincibleTimer = 0;
    void OnTriggerStay2D(Collider2D collider)
    {
        if (GameManager.playerWeaponTag.Contains(collider.gameObject.tag) && invincibleTimer < 0)
        {
            invincibleTimer = 0.1f;
            int damage = 0;
            switch (collider.gameObject.tag)
            {
                case "bat":
                    damage = PlayerData.main.batAttack;
                    break;
                case "spear":
                    damage = PlayerData.main.spearAttack;
                    break;
                case "bow":
                    damage= PlayerData.main.bowAttack;
                    break;
                case "hammer":
                    damage = PlayerData.main.hammerAttack;
                    break;
                case "arrow":
                    damage = PlayerData.main.arrowAttack;
                    break;
                case "skillattack":
                    damage = PlayerData.main.skillAttack;
                    break;
                default:
                    break;
            }
            enemyHp -= damage;
            SceneTransition.main.DamageAmount(damage);
            if (enemyHp <= 0)
            {
                SceneTransition.main.GetKill();
                Destroy(this.gameObject);
            }
        }

    }
    void Start()
    {
        int stageLevel = sceneData.GetStageLevel();
        powerUpRatio = Random.Range(sceneData.ratios[stageLevel].x, sceneData.ratios[stageLevel].y);
        SubStart();
    }
    void Update()
    {
        invincibleTimer -= Time.deltaTime;
        GetComponent<SpriteRenderer>().color = (invincibleTimer > 0) ? new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);
        SubUpdate();
    }
    public virtual void SubStart() { }
    public virtual void SubUpdate() { }
}
