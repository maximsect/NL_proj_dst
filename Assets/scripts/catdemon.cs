using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catdemon : EnemyBaseScript
{

    public float speed = 5f;
    public float jumppower = 8f;
    public float checkDistance = 0.1f;
    public float footOffset = 0.01f;
    public int attackPower = 10;
    public float interactionZone = 0.35f;

    public float attStartTime = 0.1f;
    public float attEndTime = 0.2f;
    public float animEndTime = 0.6f;
    public float coolEndTime = 0.8f;
    private float StartTime = 0;

    public float tpPower = 24f;
    public GameObject attackObj;
    int tpCount = 300;

    public Animator animator;
    int behavior = 0;
    public int color = 0;

    Rigidbody2D rbody;
    bool isGrounded;
    bool isJumping = false;

    float re_x;

    short invincible = 0;
    Coroutine coroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void SubStart()
    {

        StartTime = -coolEndTime * 2;
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        this.animator.SetInteger("color", color);
        coroutine = StartCoroutine(CatDemonMovement());

        enemyHp = (int)(enemyHp * powerUpRatio);
        speed *= powerUpRatio;
    }

    // Update is called once per frame

    /*
    public override void SubUpdate()
    {
        /*
        animator.SetInteger("catbehave", this.behavior);

        //�������猩���v���C���[�̕����x�N�g�����擾
        Vector3 dir = (GameManager.main.player.transform.position - transform.position);

        //x�����̈ړ�

        re_x = dir.x;
        float vx = re_x * speed;
        float vy = dir.y * speed;



        if ((coolEndTime - (Time.time - StartTime) <= 0))//攻撃中じゃない
        {
            if (Mathf.Abs(re_x) > interactionZone)//もしプレイヤーとのX軸方向の距離がinteractionZoneより大きい場合
            {
                rbody.linearVelocity = new Vector2(vx, rbody.linearVelocity.y);
                //プレイヤー方向に移動する

                if (re_x < 0)
                {
                    rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);
                    this.transform.localScale = new Vector3(1, 1, 1);


                }
                else if (re_x > 0)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);

                }

                behavior = 0;
            }
            else//もしプレイヤーとのX軸方向の距離がinteractionZone以下の場合
            {
                //停止する
                StartTime = Time.time;//攻撃開始
                behavior = 1;

            }
        }
        else//攻撃中
        {

            if (animEndTime - (Time.time - StartTime) > 0)
            {
                behavior = 1;

            }
            else { behavior = 2; }


            if (attStartTime < (Time.time - StartTime) && (Time.time - StartTime) < attEndTime)
            {
                this.attackObj.SetActive(true);
            }
            else
            {
                this.attackObj.SetActive(false);
            }

        }
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkDistance + footOffset, LayerMask.GetMask("Ground"));

        if (isGrounded && !isJumping)
            if (vy < 0)
            {
                isJumping = true;
                rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
            }
    }/**/
    /// <summary>
    /// movementMode 
    /// 0: playerの方向へ歩く
    /// 1: ランダムに行動する　歩く、ジャンプ、とびかかる。
    /// 2:　playerに攻撃する
    /// 
    /// behavior
    /// 0:歩く
    /// 1:攻撃する
    /// 2:攻撃停止
    /// </summary>
    int movementMode = 0;
    Vector3 relativePos;

    public override void SubUpdate()
    {
        animator.SetInteger("catbehave", this.behavior);
        relativePos = GameManager.player.transform.position - transform.position;
        if (Mathf.Abs(relativePos.y) < interactionZone)
        {
            movementMode = 0;
            if (Mathf.Abs(relativePos.x) < interactionZone)
                movementMode = 2;
        }
        else movementMode = 1;
    }
    IEnumerator CatDemonMovement()
    {
        while (true)
        {
            switch (movementMode)
            {
                case 0:
                    rbody.linearVelocity = new Vector2((relativePos.x > 0) ? speed : -speed, rbody.linearVelocity.y);
                    //プレイヤー方向に移動する
                    this.transform.localScale = new Vector3((relativePos.x > 0) ? -1 : 1, 1, 1);
                    behavior = 0;
                    yield return null;
                    break;
                case 1:
                    int randomMove = UnityEngine.Random.Range(0, 6);
                    if (randomMove >= 2) randomMove = UnityEngine.Random.Range(0, 6);
                    switch (randomMove)
                    {
                        case 0:
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                behavior = 0;
                                this.transform.localScale = new Vector3(-1, 1, 1);
                                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);
                                yield return null;
                            }
                            break;
                        case 1:
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                behavior = 0;
                                this.transform.localScale = new Vector3(1, 1, 1);
                                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);
                                yield return null;
                            }
                            break;
                        case 2:
                            rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                behavior = 0;
                                this.transform.localScale = new Vector3(-1, 1, 1);
                                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);

                                yield return null;
                            }
                            break;
                        case 3:
                            rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                behavior = 0;
                                this.transform.localScale = new Vector3(1, 1, 1);
                                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);

                                yield return null;
                            }
                            break;
                        case 4:
                            rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                behavior = 0;
                                yield return null;
                            }
                            break;
                        case 5:
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                yield return null;
                            }
                            break;
                        default:
                            yield return null;
                            break;
                    }
                    break;
                case 2:
                    GameManager.main.PlayOneShot(attackSound);
                    for (float elapsedTimer = 0; elapsedTimer < coolEndTime; elapsedTimer += Time.deltaTime)
                    {
                        rbody.linearVelocity = new Vector2(0, rbody.linearVelocity.y);
                        behavior = (elapsedTimer < animEndTime) ? 1 : 2;
                        this.attackObj.SetActive(elapsedTimer == Mathf.Clamp(elapsedTimer, attStartTime, attEndTime));//経過時間がattStartTimeとattEndTimeの途中である場合

                        yield return null;
                    }
                    break;
                default:
                    yield return null;
                    break;
            }
        }
    }
    float knockBackTimer = 1f;
    public override void KnockBack()
    {
        StopCoroutine(coroutine);                   
        rbody.AddForce(new Vector2((transform.position.x - GameManager.player.transform.position.x) > 0 ? 4 : -4, 4), ForceMode2D.Impulse);

        StartCoroutine(KnockBackCoroutine());
    }
    IEnumerator KnockBackCoroutine()
    {
        yield return new WaitForSeconds(knockBackTimer);
        StartCoroutine(CatDemonMovement());
        yield break;
    }
}