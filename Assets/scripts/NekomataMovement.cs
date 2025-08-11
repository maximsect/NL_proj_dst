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
    public float x_velocity = 0;
    public bool lookdirectionEnabled = false;
    [Header("health")]
    public int hp = 25;
    public Slider hpDisplay;
    public LayerMask groundLayer;
    private Animator animator;
    public Color color;
    public float scale = 1.5f;
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
        rb2d.linearVelocity = new Vector3(x_velocity, rb2d.linearVelocity.y);
        if (Mathf.Abs(relativePos.y) <= 2 && lookdirectionEnabled)
        {
            transform.localScale = ((relativePos.x > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1)) * scale;
        }

        invincibleTimer -= Time.deltaTime;
        nekoImage.color = (invincibleTimer > 0) ? new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);

        if (Mathf.Abs(relativePos.y) < interactableZone)
        {
            moveMode = 1;
            if (Mathf.Abs(relativePos.x) < interactableZone) moveMode = 2;
        }
        else if (Mathf.Abs(relativePos.x) < 5) moveMode = 0;
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
    List<int> selectableMovementIndex0 = new List<int>() { 0, 0, 0, 1, 1, 1, 2, 3, 4, 5, 6, 7 };
    List<int> selectableMovementIndex1 = new List<int>() { 0, 0, 0, 1, };
    int moveIndex = 0;
    IEnumerator NekomataAnimation()
    {
        while (true)
        {
            switch (moveMode)
            {
                case 0:
                    lookdirectionEnabled = false;
                    int randomMove = selectableMovementIndex0[UnityEngine.Random.Range(0, selectableMovementIndex0.Count - 2)];
                    selectableMovementIndex0.Remove(randomMove);
                    selectableMovementIndex0.Add(randomMove);
                    switch (randomMove)
                    {
                        case 0:
                            AnimeState(1);
                            x_velocity = speed;
                            transform.localScale = new Vector3(-1, 1, 1) * scale;
                            yield return new WaitForSeconds(1f);
                            break;
                        case 1:
                            AnimeState(1);
                            x_velocity = -speed;
                            transform.localScale = new Vector3(1, 1, 1) * scale;
                            yield return new WaitForSeconds(1f);
                            break;
                        case 2:
                            rb2d.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            transform.localScale = new Vector3(-1, 1, 1) * scale;
                            AnimeState(1);
                            x_velocity = speed;
                            yield return new WaitForSeconds(1f);
                            break;
                        case 3:
                            rb2d.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            transform.localScale = new Vector3(1, 1, 1) * scale;
                            AnimeState(1);
                            x_velocity = -speed;
                            yield return new WaitForSeconds(1f);
                            break;
                        case 4:
                            rb2d.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                            AnimeState(0);
                            yield return new WaitForSeconds(1f);
                            break;
                        case 5:
                            AnimeState(0);
                            yield return new WaitForSeconds(0.4f);
                            break;
                        case 6:
                            AnimeState(0);
                            lookdirectionEnabled = true;
                            yield return SummonNekoOni(Random.Range(2, 6));
                            break;
                        case 7:
                            x_velocity = 0;
                            AnimeState(3);
                            yield return null;
                            AnimeState(4);
                            yield return new WaitForSeconds(1f);
                            break;
                        default:
                            yield return null;
                            break;
                    }
                    break;
                case 1:
                    lookdirectionEnabled = true;
                    x_velocity = (relativePos.x > 0) ? -speed : speed;
                    //
                    AnimeState(1);
                    yield return null;
                    break;
                case 2:
                    switch (selectableMovementIndex1[moveIndex++ % selectableMovementIndex1.Count])
                    {
                        default:
                            lookdirectionEnabled = false;
                            x_velocity = 0;
                            AnimeState(2);
                            yield return new WaitForSeconds(1.3f);
                            break;
                        case 1:
                            x_velocity = 0;
                            yield return SummonNekoOni(Random.Range(2,4));
                            AnimeState(3);
                            yield return null;
                            AnimeState(4);
                            yield return new WaitForSeconds(1f);
                            break;
                    }

                    break;
                /*case 3:
                    x_velocity = 0;
                    AnimeState(3);
                    yield return null;
                    AnimeState(4);
                    yield return new WaitForSeconds(1f);
                    yield return SummonNekoOni(Random.Range(2, 6));
                    break;/**/
                default:
                    yield return null;
                    break;
            }
        }
    }

    public void Warp()
    {
        transform.position = RandomPos().ToVector3(0);
    }
    List<GameObject> nekoOni = new List<GameObject>();
    IEnumerator SummonNekoOni(int summonNumber)
    {
        for (int i = 0; i < summonNumber; i++)
        {
            nekoOni.RemoveAll(item => item == null);
            if (nekoOni.Count < summonNumber)
            {
                GameObject generated = Instantiate(nekoOniPref, RandomPos().ToVector3(), Quaternion.identity);
                nekoOni.Add(generated);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    void AnimeState(int index)
    {
        animator.SetInteger("Mode", index);
    }
}