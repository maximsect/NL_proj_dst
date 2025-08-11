using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class player_main : MonoBehaviour
{
    public static player_main main;
    Rigidbody2D rigid;
    float hor_input = 0f;
    float ver_input = 0f;
    Vector2 velcopy_x;
    Vector2 velcopy_y;

    GameObject enemy;
    bool arrowkeyhold = false;
    bool isgrapping = false;
    Vector3 pos_tomove;
    Vector3 grapvelocity;
    int dashtime = 0;
    bool hitflag = false;
    bool isground = false;
    bool istwicejumpused = false;
    short keyputting = 0;
    int invincibletime = 0;
    int kb_time = 0;
    int kbtimer = 0;
    float last_velocity = 0f;
    bool dashflag = false;
    int behavior = 0;
    int attacktime = 0;
    int arrowtime = 0;
    //[System.NonSerialized] public int direction = 1;
    [System.NonSerialized] public int skillcooldown = 0;

    private Slider hpSlider;
    public GameObject batObj,spearObj,hammerObj;
    public GameObject skill;
    private arrowmaker arrowProducer;
    public LayerMask ground;
    public int xp = 0;
    public float grappingSpeed = 25;
    public Animator animator;
    public Animator skillanimator;
    public BoxCollider2D attackcollider;
    public BoxCollider2D skillcollider;
    private UI_skillcooldown UI_cooldowndisplay;

    readonly float maxspeedX = 5.0f;
    readonly Vector3 hide = new Vector3(0f, 100f, 0f);
    readonly Vector2 hide2D = new Vector3(0f, 100f);
    readonly Vector2 KBVEC = new Vector2(-7f, 7f);
    readonly Vector2 DASHSPEED = new Vector2(12f, 0f);
    readonly int SKILLINIT = 124;
    readonly int SKILLBEGIN = 94;
    readonly int SKILLEND = 64;
    readonly int BATINIT = 20;
    readonly int BATBEGIN = 19;
    readonly int BATEND = 14;
    readonly int SPEARINIT = 20;
    readonly int SPEARBEGIN = 19;
    readonly int SPEAREND = 14;
    readonly int HAMMERINIT = 32;
    readonly int HAMMERBEGIN = 20;
    readonly int HAMMEREND = 10;
    readonly int ARROWINIT = 19;
    readonly int ARROWBEGIN = 4;
    void OnEnable()
    {
        if (main == null) main = this;
        else Destroy(this.gameObject);
    }
    void Start()
    {
        hpSlider = GameObject.Find("Slider").GetComponent<Slider>();
        UI_cooldowndisplay = GameObject.Find("skillcooldown").GetComponent<UI_skillcooldown>();
        arrowProducer = GetComponent<arrowmaker>();
        rigid = GetComponent<Rigidbody2D>();

        hpSlider.minValue = 0;
        hpSlider.maxValue = PlayerData.main.maxHp;
        hpSlider.value = PlayerData.main.hp;
    }
    /*void Update(){
        if(this.kbtimer>0)
            this.kbtimer--;
        else
            Time.timeScale=1f;
    }*/
    void LateUpdate()
    {
        hpSlider.maxValue = PlayerData.main.maxHp;
        hpSlider.value = PlayerData.main.hp;
    }

    void FixedUpdate()
    {
        this.hor_input = Input.GetAxisRaw("Horizontal");
        this.ver_input = Input.GetAxisRaw("Vertical");

        if (this.rigid.IsTouchingLayers(this.ground) && this.rigid.linearVelocity.y < 0.01f && this.rigid.linearVelocity.y > -0.01f && this.last_velocity < 0.01f && this.last_velocity > -0.01f)
        {
            //接地判定
            this.isground = true;
            this.istwicejumpused = false;
            this.dashflag = false;
        }
        else
        {
            this.isground = false;
        }

        if (!this.iskbing() && !this.isgrapping)
        {

            if (this.hor_input > 0.1f && this.dashtime <= 8 && !this.isskillusing(2) && this.arrowtime <= 0)
            {
                //右移動
                this.velcopy_x = this.rigid.linearVelocity;
                this.velcopy_x.x = PlayerData.main.moveSpeed * ((attacktime > 0) ? 0.3f : 1);//this.maxspeedX;
                this.rigid.linearVelocity = this.velcopy_x;
                if (this.attacktime < 8 && this.skillcooldown <= 65)
                    PlayerData.main.direction = 1;
                this.behavior = 1;
            }
            if (this.hor_input < -0.1f && this.dashtime <= 8 && !this.isskillusing(2) && this.arrowtime <= 0)
            {
                //左移動
                this.velcopy_x = this.rigid.linearVelocity;
                this.velcopy_x.x = -PlayerData.main.moveSpeed * ((attacktime > 0) ? 0.3f : 1);// -this.maxspeedX;
                this.rigid.linearVelocity = this.velcopy_x;
                if (this.attacktime < 8 && this.skillcooldown <= 65)
                    PlayerData.main.direction = -1;
                this.behavior = 1;
            }

            if (this.hor_input <= 0.1f && this.hor_input >= -0.1f)
            {
                //停止
                this.velcopy_x = this.rigid.linearVelocity;
                this.velcopy_x.x = 0f;
                this.rigid.linearVelocity = this.velcopy_x;
                this.behavior = 0;
            }

            if (Input.GetKey(KeyCode.C) && this.isground && this.keyputting % 2 == 0 && this.dashtime <= 8 && !this.isskillusing(2) && this.arrowtime <= 0)
            {
                //ジャンプ
                this.velcopy_y = this.rigid.linearVelocity;
                this.velcopy_y.y = PlayerData.main.jumpSpeed;
                this.rigid.linearVelocity = this.velcopy_y;
                this.keyputting = 1;
            }

            if (Input.GetKey(KeyCode.C) && !this.isground && !this.istwicejumpused && this.keyputting % 2 == 0 && this.dashtime <= 8 && !this.isskillusing(2) && this.arrowtime <= 0)
            {
                //空中ジャンプ
                this.velcopy_y = this.rigid.linearVelocity;
                this.velcopy_y.y = PlayerData.main.airJumpSpeed;
                this.rigid.linearVelocity = this.velcopy_y;
                this.istwicejumpused = true;
                this.keyputting = 1;
            }

            if (Input.GetKey(KeyCode.Z) && this.dashtime <= 0 && !this.dashflag && !this.isskillusing(2) && this.arrowtime <= 0)
            {
                this.dashtime = 20;
                this.dashflag = true;
            }

            if (Input.GetKey(KeyCode.X) && this.arrowtime <= 0)
            {
                if (this.attacktime <= 0)
                {
                    switch (PlayerData.main.weapon)
                    {
                        case Weapon.Bat:
                            attacktime = BATINIT;
                            break;
                        case Weapon.Spear:
                            attacktime = SPEARINIT;
                            break;
                        case Weapon.Hammer:
                            attacktime = HAMMERINIT;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (Input.GetKey(KeyCode.D) && this.skillcooldown <= 0 && this.arrowtime <= 0)
            {
                this.skillcooldown = this.SKILLINIT;
                this.UI_cooldowndisplay.mask.alphaCutoff = 1.0f;
            }

            if (Input.GetKey(KeyCode.X) && !this.arrowkeyhold && this.arrowtime <= 0 && PlayerData.main.weapon == Weapon.Bow)
            {
                //this.arrowmaker.arrowmaking=true;
                this.arrowkeyhold = true;
                this.arrowtime = this.ARROWINIT;
            }

            this.dash();
        }

        if (!this.isground) { this.behavior = 2; }

        if (!Input.GetKey(KeyCode.C))
        {
            this.keyputting = 0;
        }

        /*if(Input.GetKey(KeyCode.A) && !this.isgrapping){
            //引き寄せ
            enemy = enemyManager.TheMostDistantObjectOnScreen();
            if (enemy != null)
            {
                this.isgrapping = true;
                this.grapvelocity = (enemy.transform.position - this.rigid.transform.position) * 2.7f;
                this.pos_tomove = enemy.transform.position - this.grapvelocity.normalized;
            }
        }/**/
        //引き寄せ
        if ((Input.GetKeyDown(KeyCode.A)) && !this.iskbing())
        {
            StartCoroutine(GrappingCoroutine(GetDistantObject()));
        }

        if (!Input.GetKey(KeyCode.S))
        {
            this.arrowkeyhold = false;
        }

        if (this.arrowtime == this.ARROWBEGIN) { this.arrowProducer.arrowmaking = true; }
        switch (PlayerData.main.weapon)
        {
            case Weapon.Bat:
                if (this.attacktime < this.BATEND || this.BATBEGIN < this.attacktime)
                {
                    this.batObj.transform.position = this.hide;
                }
                else
                {
                    this.batObj.transform.position = new Vector3(this.transform.position.x + 0.75f * PlayerData.main.direction, this.transform.position.y, 0f);
                }
                break;
            case Weapon.Spear:
                if (this.attacktime < this.SPEAREND || this.HAMMERBEGIN < this.attacktime)
                {
                    this.spearObj.transform.position = this.hide;
                }
                else
                {
                    this.spearObj.transform.position = new Vector3(this.transform.position.x + 0.75f * PlayerData.main.direction, this.transform.position.y, 0f);
                }
                break;
            case Weapon.Hammer:
                if (this.attacktime < this.HAMMEREND || this.HAMMERBEGIN < this.attacktime)
                {
                    this.hammerObj.transform.position = this.hide;
                }
                else
                {
                    this.hammerObj.transform.position = new Vector3(this.transform.position.x + 0.75f * PlayerData.main.direction, this.transform.position.y, 0f);
                }
                break;
            default:
                break;
        }
        if (this.isskillusing(2))
        {
            this.skill.transform.position = new Vector2(this.transform.position.x + 2.5f * PlayerData.main.direction, this.transform.position.y + 0.25f);
            if (this.isskillusing()) { this.skillcollider.offset = new Vector2(0.125f, -0.125f); }
            else { this.skillcollider.offset = this.hide2D; }
            this.velcopy_x = this.rigid.linearVelocity;
            this.velcopy_x.x = 0f;
            this.rigid.linearVelocity = this.velcopy_x;
        }
        else
        {
            this.skill.transform.position = this.hide;
            this.skillanimator.SetBool("isskillusing", false);
        }

        if (0 < this.arrowtime)
        {
            this.velcopy_x = this.rigid.linearVelocity;
            this.velcopy_x.x = 0f;
            this.rigid.linearVelocity = this.velcopy_x;
        }

        if (this.iskbing())
            this.rigid.linearVelocity = this.rigid.linearVelocity * (this.kb_time - 1) / this.kb_time;

        if (this.attacktime > 0) { this.attacktime--; }
        if (this.arrowtime > 0) { this.arrowtime--; }
        if (this.skillcooldown > 0) { this.skillcooldown--; }
        if (this.kb_time > 0) { this.kb_time--; }
        if (this.invincibletime > 0) { this.invincibletime--; }

        /*if(this.isgrapping){
            this.grap(this.enemy);
        }/**/

        if (this.hitflag)
            this.hitflag = false;

        this.last_velocity = this.rigid.linearVelocity.y;

        this.transform.localScale = new Vector3(-PlayerData.main.direction, this.transform.localScale.y, 1);

        if (0 < this.attacktime)
        {
            switch (PlayerData.main.weapon)
            {
                case Weapon.Bat:
                    behavior = 3;
                    break;
                case Weapon.Spear:
                    behavior = 6;
                    break;
                case Weapon.Bow:
                    behavior = 5;
                    break;
                case Weapon.Hammer:
                    behavior = 7;
                    rigid.AddForce(new Vector2(0, -20f));
                    break;
                default:
                    break;
            }
        }
        if (0 < this.arrowtime) { this.behavior = 5; }
        if (this.isskillusing(2)) { this.skillanimator.SetBool("isskillusing", true); this.behavior = 4; }
        animator.SetInteger("behave", this.behavior);
    }

    /*void grap(GameObject enemy){
        this.rigid.linearVelocity=this.grapvelocity;
        if((enemy.transform.position-this.transform.position).magnitude<=1.5f){
            this.rigid.linearVelocity=Vector3.zero;
            this.isgrapping=false;
        }
    }/**/

    IEnumerator GrappingCoroutine(GameObject enemyObj)
    {

        yield return new WaitForSeconds(0.4f);
        if (isgrapping) { yield break; }
        isgrapping = true;
        Vector3 playerBasePosition = transform.position;
        if (enemyObj == null) { isgrapping = false; yield break; }
        Vector3 enemyBasePosition = enemyObj.transform.position;
        Vector3 relativePos = (enemyBasePosition - playerBasePosition);
        if (relativePos.x != 0)
        {
            relativePos -= 2 * new Vector3(relativePos.x / Mathf.Abs(relativePos.x), 0, 0);
        }
        for (float _timer = 0; _timer < relativePos.magnitude / grappingSpeed; _timer += Time.deltaTime)
        {
            float t = _timer * grappingSpeed / relativePos.magnitude;
            if (enemyObj == null) { rigid.linearVelocity = Vector2.zero; isgrapping = false; yield break; }
            enemyObj.transform.position = enemyBasePosition;
            transform.position = playerBasePosition + t * relativePos;
            yield return null;
        }
        rigid.linearVelocity = Vector2.zero;
        isgrapping = false;
        yield break;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        //if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("damage_factor") || (collision.gameObject.CompareTag("assign_attack") && Random.Range(0, 2) == 0)) && !this.hitflag && this.invincibletime <= 0 && (!collision.otherCollider.gameObject.CompareTag("attack") && !collision.otherCollider.gameObject.CompareTag("skillattack"))) {
        if (GameManager.enemyWeaponTag.Contains(collider.gameObject.tag) && Random.Range(0, 2) == 0 && !this.hitflag && this.invincibletime <= 0)
        {
            Debug.Log("damage");
            PlayerData.main.Damage(1);
            this.hitflag = true;
            this.rigid.linearVelocity = Vector2.zero;
            this.rigid.AddForce(this.transform.position.x <= collider.transform.position.x ? this.KBVEC : Vector2.Reflect(this.KBVEC, Vector2.right), ForceMode2D.Impulse);
            this.kb_time = 20;
            this.invincibletime = 60;
            this.dashtime = 0;
            if (PlayerData.main.hp <= 0)
            {
                Debug.Log("dead");
            }
            //this.kbtimer=50;
            //Time.timeScale=0;

        }
    }


    void dash()
    {
        if (this.dashtime > 0)
        {
            if (this.dashtime > 8)
                this.rigid.linearVelocity = this.DASHSPEED * PlayerData.main.direction;
            this.dashtime--;
        }
    }

    bool iskbing()
    {
        return this.kb_time > 0 ? true : false;
    }

    bool isskillusing(int mode = 0)
    {
        if (mode == 1)
            return this.SKILLBEGIN < this.skillcooldown;
        if (mode == 2)
            return this.SKILLEND < this.skillcooldown;
        return this.SKILLEND < this.skillcooldown && this.skillcooldown < this.SKILLBEGIN;
    }

    private GameObject GetDistantObject()
    {
        List<GameObject> enemyList = new List<GameObject>();
        GameObject obj = null;
        float distance = 0;
        enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        enemyList.RemoveAll(item => !item.isVisible());
        if (enemyList.Count == 0) return null;
        else if (enemyList.Count == 1) return enemyList[0];
        else
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] != null)
                {
                    Vector3 pos = enemyList[i].transform.position - transform.position;
                    obj = (distance < pos.magnitude) ? enemyList[i] : obj;
                    distance = (distance < pos.magnitude) ? pos.magnitude : distance;

                }
            }
            return obj;
        }
    }
}
public static class Extentions
{
    public static bool isVisible(this GameObject go)
    {
        Vector3 pos = go.transform.position;
        return (pos.x >= 0 && pos.x <= Screen.width && pos.y >= 0 && pos.y <= Screen.height);
    }
}
