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

    public float interactionZone = 0.5f;

    public float tpPower = 24f;

    int tpCount = 300;//�e���|�[�g���Ǘ�����J�E���^�[�@

    public Animator animator;
    int behavior = 0;

    Rigidbody2D rbody;
    bool isGrounded;
    bool isJumping = false;

    float re_x;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation

        // "Player"�^�O���t����GameObject���擾
        taregtObject = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("catbehave", this.behavior);


            }

    private void FixedUpdate()
    {
        //�������猩���v���C���[�̕����x�N�g�����擾
        Vector3 dir = (taregtObject.transform.position - transform.position).normalized;

        //x�����̈ړ�

        re_x = dir.x;
        float vx = re_x * speed;
        float vy = dir.y * speed;

        if (Mathf.Abs(re_x) > interactionZone)
        {
            rbody.linearVelocity = new Vector2(vx, rbody.linearVelocity.y);
            GetComponent<SpriteRenderer>().flipX = vx > 0;
            behavior = 0;
        }
        else
        {
            behavior = 1;

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
}


