using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public interface EnemyStatus //敵のスクリプトの中に必ず仕込んで！
{
    int enemyHP { get; }
    int attackPower { get; }
    int colAttackPower { get; }
    int attackXP { get; }
    int destroyXP { get; }
}

public class PlayerScript : MonoBehaviour
{
    public PlayerSetting playerSetting;//主人公の基礎能力
    public EnemyManager enemyManager;
    private Rigidbody2D rb2d;
    public LayerMask groundLayer;
    private int jumpCount;//ジャンプ回数
    private bool isGround;//接地
    private bool isGrapping;//引き寄せ
    private bool hitflag;//被ダメ判定


    private int dashCount = 0;
    public float dashInterval = 0.3f;
    private float dashTimer = 0;
    bool isDashing = false;
    private float h_input;

    private float chargeMultiplier, chargeTimer, chargeInterval; //ためる動作
    private bool chargeLimit = false;
    //
    public float sufferDamageInterval = 0.3f;
    private float invincibleTimer;
    bool isInvincible = false;
    private bool isKBing = false;
    public GameObject mainSprite;

    private float recoveryTimer;
    [Header("Sword")]
    public GameObject sword_AttackRegion;
    private bool isRegionOnDisplay = false;
    [Space(10)]
    public string dashKeyCode = "z";
    public string attackKeyCode = "x";
    public string specialKeyCode = "c";
    public string chargeKeyCode = "s";


    readonly Vector2 KBVEC = new Vector2(-7f, 7f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jumpCount = 0;//ジャンプ初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.isGrapping && !isKBing)
        {
            //!!!!!!!!!!!!!!!!!!

            float hor_input = Input.GetAxis("Horizontal");
            float radius = playerSetting.gravity.radius;
            float angle = playerSetting.gravity.angle * Mathf.PI / 180;
            float linearVel = rb2d.linearVelocity.x * Mathf.Cos(angle) + rb2d.linearVelocity.y * Mathf.Sin(angle);
            rb2d.linearVelocity = new Vector2(-hor_input * playerSetting.movementSpeed * Mathf.Sin(angle) + linearVel * Mathf.Cos(angle), hor_input * playerSetting.movementSpeed * Mathf.Cos(angle) + linearVel * Mathf.Sin(angle));
            HorizontalFlip(hor_input);
            //Charge---------------------------------------------------
            if (Input.GetKeyDown(chargeKeyCode))
            {
                chargeLimit = false;
            }
            if (Input.GetKey(chargeKeyCode) && !chargeLimit)
            {
                chargeTimer = Mathf.Clamp(chargeTimer + Time.deltaTime, 0, 3);
                chargeMultiplier = (chargeTimer + 1) / chargeInterval;
            }

            //jump--------------
            if (jumpCount < playerSetting.numberOfJump && (Input.GetKeyDown("w") || Input.GetKeyDown("space")))
            {
                rb2d.linearVelocity = Vector2.zero;

                if (playerSetting.chargedJump)
                {
                    rb2d.AddForce(new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * playerSetting.jumpForce * chargeMultiplier, ForceMode2D.Impulse);
                    chargeMultiplier = 1;
                    chargeLimit = true;
                    chargeTimer = 0;
                }
                else
                {
                    rb2d.AddForce(new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * playerSetting.jumpForce, ForceMode2D.Impulse);
                }
                jumpCount++;
            }

            //dash-----------------
            if ((Input.GetKeyDown(dashKeyCode) && !isDashing) && (hor_input != 0 && dashCount < playerSetting.numberOfDash))
            {
                if (hor_input > 0) h_input = 1;
                else h_input = -1;
                isDashing = true;
                dashCount++;
            }
            //grapping-----------------
            if ((Input.GetKeyDown(specialKeyCode)) && !isKBing)
            {
                StartCoroutine(GrappingCoroutine(enemyManager.TheMostDistantObjectOnScreen()));
            }
            //dash--------------------
            if (isDashing)
            {
                StartCoroutine(InvincibleCoroutine());
                if (playerSetting.chargedDash)
                {
                    rb2d.linearVelocity = new Vector2(h_input * playerSetting.dashSpeed * -Mathf.Sin(angle), h_input * playerSetting.dashSpeed * Mathf.Cos(angle)) * chargeMultiplier;
                }
                else
                {
                    rb2d.linearVelocity = new Vector2(h_input * playerSetting.dashSpeed * -Mathf.Sin(angle), h_input * playerSetting.dashSpeed * Mathf.Cos(angle));
                }
                dashTimer += Time.deltaTime;
                if (dashTimer > dashInterval)
                {
                    dashTimer = 0;
                    isDashing = false;
                    if (playerSetting.chargedDash)
                    {
                        chargeMultiplier = 1;
                        chargeLimit = true;
                        chargeTimer = 0;
                    }
                }
            }
            //attack------------------
            switch (playerSetting.weaponIndex)
            {
                case 0:
                    if (Input.GetKeyDown(attackKeyCode))
                    {
                        StartCoroutine(SwordCoroutine());
                    }
                    break;
                default:
                    break;
            }

        }
        //AutoHeal------------------------------------------------------
        recoveryTimer += Time.deltaTime;
        if (recoveryTimer > playerSetting.autoHealInterval)
        {
            recoveryTimer = 0;
            playerSetting.health = Mathf.Clamp(playerSetting.health + playerSetting.autoHealAmount, 0, playerSetting.maxHealth);
        }
        GravityChange();
    }
    IEnumerator SwordCoroutine()
    {
        if (isRegionOnDisplay) { yield break; }
        isRegionOnDisplay = true;
        sword_AttackRegion.SetActive(true);
        yield return new WaitForSeconds(playerSetting.attackSetting[0].attackInterval);
        sword_AttackRegion.SetActive(false);
        isRegionOnDisplay = false;
        yield break;
    }
    IEnumerator InvincibleCoroutine()
    {
        if (isInvincible) { yield break; }
        isInvincible = true;
        for (float _timer = 0; _timer < sufferDamageInterval; _timer += Time.deltaTime)
        {
            SpriteRenderer sprite = mainSprite.GetComponent<SpriteRenderer>();
            if (Mathf.Sin(_timer * 200) > 0) sprite.enabled = false;
            else sprite.enabled = true;
            yield return null;
        }
        mainSprite.GetComponent<SpriteRenderer>().enabled = true;
        mainSprite.GetComponent<SpriteRenderer>().color = Color.white;
        isInvincible = false;
        yield break;
    }
    IEnumerator KnockBackCoroutine(GameObject enemyObj)
    {
        if (isKBing) { yield break; }
        isKBing = true;
        //Vector3 relativePos = (enemyObj.transform.position - transform.position).normalized;
        //rb2d.AddForce(new Vector2(relativePos.x, relativePos.y) * -3f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isKBing = false;
        yield break;
    }
    IEnumerator GrappingCoroutine(GameObject enemyObj)
    {

        yield return new WaitForSeconds(0.4f);
        if (isGrapping) { yield break; }
        isGrapping = true;
        Vector3 playerBasePosition = transform.position;
        if (enemyObj == null) { isGrapping = false; yield break; }
        Vector3 enemyBasePosition = enemyObj.transform.position;
        Vector3 relativePos = (enemyBasePosition - playerBasePosition);
        if (relativePos.x != 0)
        {
            relativePos -= 2 * new Vector3(relativePos.x / Mathf.Abs(relativePos.x), 0, 0);
        }
        for (float _timer = 0; _timer < relativePos.magnitude / playerSetting.grappingSpeed; _timer += Time.deltaTime)
        {
            float t = _timer * playerSetting.grappingSpeed / relativePos.magnitude;
            if (enemyObj == null) { rb2d.linearVelocity = Vector2.zero; isGrapping = false; yield break; }
            enemyObj.transform.position = enemyBasePosition;
            transform.position = playerBasePosition + t * relativePos;
            yield return null;
        }
        rb2d.linearVelocity = Vector2.zero;
        isGrapping = false;
        yield break;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        // 接地判定
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("lava"))
        {
            jumpCount = 0;
            isGround = true;
        }
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("lava")) && !isInvincible)
        {
            mainSprite.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(KnockBackCoroutine(collision.gameObject));
            StartCoroutine(InvincibleCoroutine());
            rb2d.AddForce(transform.position.x <= collision.transform.position.x ? KBVEC : Vector2.Reflect(KBVEC, Vector2.right), ForceMode2D.Impulse);
            EnemyStatus es = collision.gameObject.GetComponent<EnemyStatus>();
            if (es != null)
            {

                int colDamage = es.colAttackPower;
                playerSetting.health = Mathf.Clamp(playerSetting.health - Mathf.Clamp(colDamage - playerSetting.defense, 0, colDamage), 0, playerSetting.maxHealth);
            }
            if (playerSetting.health == 0)
            {

            }
        }
        dashCount = 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("healItem"))
        {
            RecoveryItem(playerSetting.itemHealAmount);
            Destroy(collision.gameObject);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            jumpCount = Mathf.Clamp(jumpCount, 1, playerSetting.numberOfJump);
            isGround = false;
        }
    }
    void HorizontalFlip(float inputVal)
    {
        if (inputVal > 0.1) mainSprite.transform.localScale = new Vector3(-1, 1, 1);
        else if (inputVal < -0.1) mainSprite.transform.localScale = new Vector3(1, 1, 1);
    }
    public void RecoveryItem(int amount)
    {
        playerSetting.health = Mathf.Clamp(playerSetting.health + amount, 0, playerSetting.maxHealth);

    }
    public void ScoreChange(int delta)
    {
        playerSetting.playerScore += delta;
    }
    void GravityChange()
    {
        Physics2D.gravity = new Vector2(Mathf.Cos(playerSetting.gravity.angle * Mathf.PI / 180), Mathf.Sin(playerSetting.gravity.angle * Mathf.PI / 180)) * playerSetting.gravity.radius;
    }
}
