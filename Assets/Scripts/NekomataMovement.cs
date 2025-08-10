using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class NekomataMovement : StageManager
{
    private float powerUpRatio = 1;
    private float invincibleTimer = 0;
    public SpriteRenderer nekoImage;
    public GameObject nekoOniPref;
    public float interactableZone = 2;
    public float speed = 5, jumppower = 5;
    [Header("health")]
    public int hp = 25;
    public Slider hpDisplay;
    public LayerMask groundLayer;
    private Animator animator;
    public Color color;
    private int moveMode = 0;
    Vector3 relativePos = Vector3.zero;
    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        base.MainStart();
        hpDisplay.minValue = 0;
        hpDisplay.maxValue = hp;
        hpDisplay.value = hp;
        animator = GetComponent<Animator>();
        StartCoroutine(NekomataAnimation());

        int stageLevel = sceneData.GetStageLevel();
        powerUpRatio = Random.Range(sceneData.ratios[stageLevel].x, sceneData.ratios[stageLevel].y);
    }

    void Update()
    {
        hpDisplay.value = hp;
        relativePos = transform.position - GameManager.player.transform.position;
        if (Mathf.Abs(relativePos.y) <= 2)
        {
            transform.localScale = (relativePos.x > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        invincibleTimer -= Time.deltaTime;
        nekoImage.color = (invincibleTimer > 0) ? new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);

        if (Mathf.Abs(relativePos.y) < interactableZone)
        {
            moveMode = 1;
            if (Mathf.Abs(relativePos.x) < interactableZone) moveMode = 2;
        }
        else if (Mathf.Abs(relativePos.x) < 5) moveMode = 3;
        else moveMode = 0;

    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (GameManager.playerWeaponTag.Contains(other.gameObject.tag) && invincibleTimer < 0)
        {
            invincibleTimer = 0.1f;
            int damage = 0;
            switch (other.gameObject.tag)
            {
                case "bat":
                    damage = PlayerData.main.batAttack;
                    break;
                case "spear":
                    damage = PlayerData.main.spearAttack;
                    break;
                case "bow":
                    damage = PlayerData.main.bowAttack;
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
            hp -= damage;
            SceneTransition.main.DamageAmount(damage);
            hpDisplay.value = hp;
            if (hp <= 0)
            {
                SceneTransition.main.StageClearReciever();
                SceneTransition.main.GetKill();
                Destroy(this.gameObject);
            }
        }
    }
    Vector2 RandomPos()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Random.Range(-3f, 10f), Random.Range(-10f, 10f));
            if (Physics2D.OverlapCircle(pos, 0.5f, groundLayer)) continue;
            if (!Physics2D.OverlapCircle(pos - new Vector2(0, 0.1f), 0.5f, groundLayer)) continue;
            if (Vector3.Distance(transform.position, pos) < 7) continue;
            return pos + new Vector2(0, 0.8f);
        }
    }
    List<int> selectableMovementIndex0 = new List<int>() { 0, 1, 2, 3 };
    IEnumerator NekomataAnimation()
    {
        while (true)
        {
            switch (moveMode)
            {
                case 0:
                    int randomMove = UnityEngine.Random.Range(0, 8);
                    if (randomMove >= 2) randomMove = UnityEngine.Random.Range(0, 8);
                    switch (randomMove)
                    {
                        case 0:
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                AnimeState(1);
                                rb2d.linearVelocity = new Vector2(speed, rb2d.linearVelocity.y);
                                yield return null;
                            }
                            break;
                        case 1:
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                AnimeState(1);
                                rb2d.linearVelocity = new Vector2(-speed, rb2d.linearVelocity.y);
                                yield return null;
                            }
                            break;
                        case 2:
                            rb2d.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                AnimeState(1);
                                rb2d.linearVelocity = new Vector2(speed, rb2d.linearVelocity.y);

                                yield return null;
                            }
                            break;
                        case 3:
                            rb2d.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                AnimeState(1);
                                rb2d.linearVelocity = new Vector2(-speed, rb2d.linearVelocity.y);

                                yield return null;
                            }
                            break;
                        case 4:
                            rb2d.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                AnimeState(0);
                                yield return null;
                            }
                            break;
                        case 5:
                            for (float elapsedTimer = 0; elapsedTimer < 1; elapsedTimer += Time.deltaTime)
                            {
                                AnimeState(0);
                                yield return null;
                            }
                            break;
                        case 6:
                            AnimeState(0);
                            yield return SummonNekoOni(Random.Range(2, 6));
                            break;
                        case 7:
                            AnimeState(3);
                            yield return new WaitForSeconds(1.0f);
                            transform.position = RandomPos().ToVector3();
                            AnimeState(4);
                            yield return new WaitForSeconds(1.0f);
                            AnimeState(0);
                            break;
                        default:
                            yield return null;
                            break;
                    }
                    break;
                case 1:
                    rb2d.linearVelocity = new Vector2((relativePos.x > 0) ? -speed : speed, rb2d.linearVelocity.y);
                    //ÉvÉåÉCÉÑÅ[ï˚å¸Ç…à⁄ìÆÇ∑ÇÈ
                    AnimeState(1);
                    yield return null;
                    break;
                case 2:
                    for (float elapsedTimer = 0; elapsedTimer < 0.4f; elapsedTimer += Time.deltaTime)
                    {
                        AnimeState(2);
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case 3:
                    AnimeState(3);
                    yield return new WaitForSeconds(1.0f);
                    transform.position = RandomPos().ToVector3();
                    AnimeState(4);
                    yield return new WaitForSeconds(1.0f);
                    AnimeState(0);
                    break;
                default:
                    break;
            }
        }
    }
    IEnumerator SummonNekoOni(int summonNumber)
    {
        for (int i = 0; i < summonNumber; i++)
        {
            GameObject generated = Instantiate(nekoOniPref, RandomPos().ToVector3(),Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }
    void AnimeState(int index)
    {
        animator.SetInteger("Mode", index);
    }
}