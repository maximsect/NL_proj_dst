using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catdemon : MonoBehaviour
{

    public GameObject taregtObject;
    public int catdemon_hp = 30;

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
    public float StartTime = 0;

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

    short invincible=0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartTime = -coolEndTime * 2; 
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        // "Player"�^�O���t����GameObject���擾
        taregtObject = GameObject.Find("player");
        this.animator.SetInteger("color", color);
    }

    // Update is called once per frame


    private void Update()
    {

        animator.SetInteger("catbehave", this.behavior);

        //�������猩���v���C���[�̕����x�N�g�����擾
        Vector3 dir = (taregtObject.transform.position - transform.position).normalized;

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

                if (vx < 0) { this.transform.localScale = new Vector3(1, 1, 1); }
                else if (vx > 0) { this.transform.localScale = new Vector3(-1, 1, 1); }

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
            else { behavior = 0; }


            if (attStartTime < (Time.time - StartTime) && (Time.time - StartTime) < attEndTime)
            {
                this.attackObj.SetActive(true);
            }
            else
            {
                this.attackObj.SetActive(false);
            }

        }

        

               
     

            // �n�ʂɐڐG���Ă��邩���m�F
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkDistance + footOffset, LayerMask.GetMask("Ground"));
        // �W�����v����
        if (isGrounded && !isJumping)
            if (vy < 0)
            {
                isJumping = true;
                rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
            }
       
        /*
        // �e���|�[�g�J�E���^�[�����炷
        if (tpCount > 0)
        {
            tpCount--;
        }
        else
        {
            gameObject.SetActive(false); // �e���|�[�g�J�E���^�[��0�ɂȂ�����I�u�W�F�N�g���\���ɂ���
            if (re_x < 0)
            { // �v���C���[�������ɂ���ꍇ
                rbody.AddForce(Vector2.right * tpPower, ForceMode2D.Impulse);
            }
            else // �v���C���[���E���ɂ���ꍇ
            {
                rbody.AddForce(Vector2.left * tpPower, ForceMode2D.Impulse);

                tpCount = UnityEngine.Random.Range(300, 600); // �����_���ȃJ�E���g��ݒ�
            }
        }
        */
    }

    

    void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.CompareTag("attack") && this.invincible==0){
            this.catdemon_hp-=10;
            this.invincible=1;
            if(this.catdemon_hp<=0)
                Destroy(this.gameObject);
        }
        if(collider.gameObject.CompareTag("skillattack") && this.invincible==0){
            this.catdemon_hp-=30;
            this.invincible=1;
            if(this.catdemon_hp<=0)
                Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("arrow") && this.invincible==0){
            this.catdemon_hp-=5;
            if(this.catdemon_hp<=0)
                Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if((collider.gameObject.CompareTag("attack") || collider.gameObject.CompareTag("skillattack")) && this.invincible==1){
            this.invincible=0;
        }
    }
}


