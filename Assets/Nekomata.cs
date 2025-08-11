using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nekomata : MonoBehaviour
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

    Rigidbody2D rbody;
    bool isGrounded;
    bool isJumping = false;

    float re_x;

    short invincible = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SubStart()
    {

        StartTime = -coolEndTime * 2;
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        
        StartCoroutine(NekomataMovement());

    }

   
    int movementMode = 0;
    Vector3 relativePos;

    public void SubUpdate()
    {
        animator.SetInteger("Nekomatabehave", this.behavior);
        relativePos = GameManager.player.transform.position - transform.position;
        if (Mathf.Abs(relativePos.y) < interactionZone)
        {
            movementMode = 0;
            if (Mathf.Abs(relativePos.x) < interactionZone)
                movementMode = 2;
        }
        else movementMode = 1;
    }
    IEnumerator NekomataMovement()
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
                    int randomMove = UnityEngine.Random.Range(0, 5);
                    if (randomMove >= 2) randomMove = UnityEngine.Random.Range(0, 5);
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
                    for (float elapsedTimer = 0; elapsedTimer < coolEndTime; elapsedTimer += Time.deltaTime)
                    {
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

}
