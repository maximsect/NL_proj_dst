using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catdemon : MonoBehaviour
{

    public GameObject taregtObject;
    public int catdemon_hp = 10;

    public float speed = 5f;
    public float jumppower = 8f;
    public float checkDistance = 0.1f;
    public float footOffset = 0.01f;

    public float tpPower = 24f;

    int tpCount = 300;//テレポートを管理するカウンター　

    Rigidbody2D rbody;
    bool isGrounded;
    bool isJumping = false;

    float re_x;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        // "Player"タグが付いたGameObjectを取得
        taregtObject = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {

        //自分から見たプレイヤーの方向ベクトルを取得
        Vector3 dir = (taregtObject.transform.position - transform.position).normalized;

        //x方向の移動

        re_x = dir.x;
        float vx = dir.x * speed;
        float vy = dir.y * speed;
        rbody.linearVelocity = new Vector2(vx, rbody.linearVelocity.y);
        GetComponent<SpriteRenderer>().flipX = vx < 0;


        // 地面に接触しているかを確認
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkDistance + footOffset, LayerMask.GetMask("Ground"));
        // ジャンプ処理
        if (isGrounded && !isJumping)
            if (vy < 0)
            {
                isJumping = true;
                rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
            }
        
    }

    private void FixedUpdate()
    {
        // テレポートカウンターを減らす
        if (tpCount > 0)
        {
            tpCount--;
        }
        else
        {
            gameObject.SetActive(false); // テレポートカウンターが0になったらオブジェクトを非表示にする
            if (re_x < 0)
            { // プレイヤーが左側にいる場合
                rbody.AddForce(Vector2.right * tpPower, ForceMode2D.Impulse);
            }
            else // プレイヤーが右側にいる場合
            {
                rbody.AddForce(Vector2.left * tpPower, ForceMode2D.Impulse);

                tpCount = UnityEngine.Random.Range(300, 600); // ランダムなカウントを設定
            }
        }
    }
}


