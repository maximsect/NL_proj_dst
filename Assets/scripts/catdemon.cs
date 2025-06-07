using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catdemon : MonoBehaviour
{

    public GameObject taregtObject;
    public float speed = 5f;
    public float jumppower = 8f;
    public float checkDistance = 0.1f;
    public float footOffset = 0.01f;


    Rigidbody2D rbody;
    bool isGrounded;
    bool isJumping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
    }

    // Update is called once per frame
    void Update()
    {

        //自分から見たプレイヤーの方向ベクトルを取得
        Vector3 dir = (taregtObject.transform.position - transform.position).normalized;

        //x方向の移動
        float vx = dir.x * speed;
        rbody.linearVelocity = new Vector2(vx, rbody.linearVelocity.y);
        GetComponent<SpriteRenderer>().flipX = vx < 0;
    }
}
