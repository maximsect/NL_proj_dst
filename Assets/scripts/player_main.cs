using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class player_main : MonoBehaviour
{
    public EnemyManager enemyManager;

    Rigidbody2D rigid;
    float hor_input=0f;
    float ver_input=0f;
    Vector2 velcopy_x;
    Vector2 velcopy_y;
    readonly float maxspeedX=5.0f;

    GameObject enemy;
    public GameObject attackobj;
    public GameObject skill;
    public arrowmaker arrowmaker;
    [System.NonSerialized]public int attackkeyholdtime=0;
    //bool arrowkeyhold=false;
    readonly Vector3 hide=new Vector3(0f, 100f, 0f);
    bool isgrapping=false;
    Vector3 pos_tomove;
    Vector3 grapvelocity;
    int dashtime=0;
    bool hitflag=false;
    int direction=1;
    int life=5;
    bool isground=false;
    public LayerMask ground;
    bool istwicejumpused=false;
    short keyputting=0;
    [System.NonSerialized] public int skillcooldown=0;
    int invincibletime=0;
    int kb_time=0;
    public int xp=0;
    int kbtimer=0;
    float last_velocity=0f;
    bool dashflag=false;
    public float grappingSpeed = 25;
    public Animator animator;
    int behavior=0;

    readonly Vector2 KBVEC=new Vector2(-7f, 7f);
    readonly Vector2 DASHSPEED=new Vector2(12f, 0f);
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    /*void Update(){
        if(this.kbtimer>0)
            this.kbtimer--;
        else
            Time.timeScale=1f;
    }*/

    void FixedUpdate(){
        this.hor_input=Input.GetAxisRaw("Horizontal");
        this.ver_input=Input.GetAxisRaw("Vertical");
        
        if(!this.iskbing() && !this.isgrapping)
        {

            if(this.rigid.IsTouchingLayers(this.ground) && this.rigid.linearVelocity.y<0.01f && this.rigid.linearVelocity.y>-0.01f && this.last_velocity<0.01f && this.last_velocity>-0.01f){
                //接地判定
                this.isground=true;
                this.istwicejumpused=false;
                this.dashflag=false;
            }
            else{
                this.isground=false;
            }

            if(this.hor_input>0.1f){
                //右移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
                if(this.attackkeyholdtime<8 && this.skillcooldown<=65)
                    this.direction=1;
                this.behavior=1;
            }
            if(this.hor_input<-0.1f){
                //左移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=-this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
                if(this.attackkeyholdtime<8 && this.skillcooldown<=65)
                    this.direction=-1;
                this.behavior=1;
            }

            if(this.hor_input<=0.1f && this.hor_input>=-0.1f){
                //停止
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=0f;
                this.rigid.linearVelocity=this.velcopy_x;
                this.behavior=0;
            }

            if(Input.GetKey(KeyCode.C) && this.isground && this.keyputting%2==0){
                //ジャンプ
                this.velcopy_y=this.rigid.linearVelocity;
                this.velcopy_y.y=12.0f;
                this.rigid.linearVelocity=this.velcopy_y;
                this.keyputting=1;
            }

            if(Input.GetKey(KeyCode.C) && !this.isground && !this.istwicejumpused && this.keyputting%2==0){
                //空中ジャンプ
                this.velcopy_y=this.rigid.linearVelocity;
                this.velcopy_y.y=10.0f;
                this.rigid.linearVelocity=this.velcopy_y;
                this.istwicejumpused=true;
                this.keyputting=1;
            }

            if(!this.isground){this.behavior=2;}

            if(Input.GetKey(KeyCode.Z) && this.dashtime<=0 && !this.dashflag){
                this.dashtime=20;
                this.dashflag=true;
            }

            this.dash();
        }

        if(!Input.GetKey(KeyCode.C)){
            this.keyputting=0;
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
            StartCoroutine(GrappingCoroutine(enemyManager.TheMostDistantObjectOnScreen()));
        }

        if (Input.GetKey(KeyCode.D) && !this.iskbing() && this.skillcooldown<=0){
            this.skillcooldown=100;
        }

        if(Input.GetKey(KeyCode.X)){
            if(this.attackkeyholdtime<=0){
                this.attackkeyholdtime=20;
            }
        }

        /*if(Input.GetKey(KeyCode.S) && !this.arrowkeyhold){
            this.arrowmaker.arrowmaking=true;
            this.arrowkeyhold=true;
        }
        if(!Input.GetKey(KeyCode.S)){
            this.arrowkeyhold=false;
        }*/

        if(this.attackkeyholdtime<8){
            this.attackobj.transform.position=this.hide;
        }
        else{
            this.attackobj.transform.position=new Vector3(this.transform.position.x+0.75f*this.direction, this.transform.position.y, 0f);
        }

        if(this.isskillusing()){
            this.skill.transform.position=new Vector3(this.transform.position.x+1.5f*this.direction, this.transform.position.y, 0f);
        }
        else{
            this.skill.transform.position=this.hide;
        }

        if(this.iskbing())
            this.rigid.linearVelocity=this.rigid.linearVelocity*(this.kb_time-1)/this.kb_time;

        if(this.attackkeyholdtime>0){this.attackkeyholdtime--;}
        if(this.skillcooldown>0){this.skillcooldown--;}
        if(this.kb_time>0){this.kb_time--;}
        if(this.invincibletime>0){this.invincibletime--;}

        /*if(this.isgrapping){
            this.grap(this.enemy);
        }/**/

        if(this.hitflag)
            this.hitflag=false;

        this.last_velocity=this.rigid.linearVelocity.y;

        this.transform.localScale=new Vector3(-this.direction, this.transform.localScale.y, 1);

        if(8<=this.attackkeyholdtime){this.behavior=3;}
        if(this.isskillusing(1)){this.behavior=4;}
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

    void OnCollisionStay2D(Collision2D collision){
        if((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("damage_factor")) && !this.hitflag && this.invincibletime<=0){
            Debug.Log("damage");
            this.life--;
            this.hitflag=true;
            this.rigid.linearVelocity=Vector2.zero;
            this.rigid.AddForce(this.transform.position.x<=collision.transform.position.x ? this.KBVEC : Vector2.Reflect(this.KBVEC, Vector2.right), ForceMode2D.Impulse);
            this.kb_time=20;
            this.invincibletime=60;
            this.dashtime=0;
            SceneScript.GetDamaged();
            if(this.life<=0){
                Debug.Log("dead");
            }
            //this.kbtimer=50;
            //Time.timeScale=0;
        }
    }

    void dash(){
        if(this.dashtime>0){
            if(this.dashtime>8)
                this.rigid.linearVelocity=this.DASHSPEED*this.direction;
            this.dashtime--;
        }
    }

    bool iskbing(){
        return this.kb_time > 0 ? true:false;
    }

    bool isskillusing(int mode=0){
        if(mode==1)
            return 65<this.skillcooldown;
        return 65<this.skillcooldown && this.skillcooldown<85;
    }
}
