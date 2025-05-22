using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{

    public Rigidbody2D rigid;
    float hor_input=0f;
    float ver_input=0f;
    Vector2 velcopy_x;
    Vector2 velcopy_y;
    readonly float maxspeedX=5.0f;
    public GameObject enemy;
    public GameObject attackobj;
    public arrowmaker arrowmaker;
    [System.NonSerialized]public int attackkeyholdtime=0;
    bool arrowkeyhold=false;
    readonly Vector3 hide=new Vector3(0f, 10000f, 0f);
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

    readonly Vector2 DASHSPEED=new Vector2(12f, 0f);

    void FixedUpdate(){
        this.hor_input=Input.GetAxisRaw("Horizontal");
        this.ver_input=Input.GetAxisRaw("Vertical");
        
        if(!this.isgrapping){

            if(this.rigid.IsTouchingLayers(this.ground) && this.rigid.linearVelocity.y<0.01f && this.rigid.linearVelocity.y>-0.01f){
                this.isground=true;
                this.istwicejumpused=false;
            }
            else{
                this.isground=false;
            }

            if(this.hor_input>0.1f){
                //右移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
                this.direction=1;
            }
            if(this.hor_input<-0.1f){
                //左移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=-this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
                this.direction=-1;
            }

            if(this.hor_input<=0.1f && this.hor_input>=-0.1f){
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=0f;
                this.rigid.linearVelocity=this.velcopy_x;
            }

            if(Input.GetKey(KeyCode.C) && this.isground && this.keyputting%2==0){
                //ジャンプ
                this.velcopy_y=this.rigid.linearVelocity;
                this.velcopy_y.y=10.0f;
                this.rigid.linearVelocity=this.velcopy_y;
                this.keyputting=1;
            }

            if(Input.GetKey(KeyCode.C) && !this.isground && !this.istwicejumpused && this.keyputting%2==0){
                //空中ジャンプ
                this.velcopy_y=this.rigid.linearVelocity;
                this.velcopy_y.y=8.0f;
                this.rigid.linearVelocity=this.velcopy_y;
                this.istwicejumpused=true;
                this.keyputting=1;
            }

            if(Input.GetKey(KeyCode.Z) && this.dashtime<=0)
                this.dashtime=20;

            this.dash();
        }

        if(!Input.GetKey(KeyCode.C)){
            this.keyputting=0;
        }

        if(Input.GetKey(KeyCode.D) && !this.isgrapping){
            //引き寄せ
            this.isgrapping=true;
            this.grapvelocity = (enemy.transform.position-this.rigid.transform.position) * 2.7f;
            this.pos_tomove = enemy.transform.position-this.grapvelocity.normalized;
        }

        if(Input.GetKey(KeyCode.X)){
            if(this.attackkeyholdtime<=0){
                this.attackkeyholdtime=20;
            }
        }

        if(Input.GetKey(KeyCode.S) && !this.arrowkeyhold){
            this.arrowmaker.arrowmaking=true;
            this.arrowkeyhold=true;
        }
        if(!Input.GetKey(KeyCode.S)){
            this.arrowkeyhold=false;
        }

        if(this.attackkeyholdtime<8){
            this.attackobj.transform.position=this.hide;
        }
        else{
            this.attackobj.transform.position=new Vector3(this.transform.position.x+0.75f*this.direction, this.transform.position.y, 0f);
        }

        if(this.attackkeyholdtime>0){this.attackkeyholdtime--;}

        if(this.isgrapping){
            this.grap(this.enemy);
        }

        if(this.hitflag)
            this.hitflag=false;
    }

    void grap(GameObject enemy){
        this.rigid.linearVelocity=this.grapvelocity;
        if((enemy.transform.position-this.transform.position).magnitude<=1.5f){
            this.rigid.linearVelocity=Vector3.zero;
            this.isgrapping=false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Enemy") && !this.hitflag){
            Debug.Log("damage");
            this.life--;
            this.hitflag=true;
            if(this.life<=0){
                Debug.Log("dead");
            }
        }
    }

    void dash(){
        if(this.dashtime>0){
            if(this.dashtime>8)
                this.rigid.linearVelocity=this.DASHSPEED*this.direction;
            this.dashtime--;
        }
    }
}
