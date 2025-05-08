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
    [System.NonSerialized]public int attackkeyholdtime=0;
    readonly Vector3 hide=new Vector3(0f, 10000f, 0f);
    bool isgrapping=false;
    Vector3 pos_tomove;
    Vector3 grapvelocity;

    void FixedUpdate(){
        this.hor_input=Input.GetAxisRaw("Horizontal");
        this.ver_input=Input.GetAxisRaw("Vertical");
        
        if(!this.isgrapping){
            if(this.hor_input>0.1f){
                //右移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
            }
            if(this.hor_input<-0.1f){
                //左移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=-this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
            }

            if(this.hor_input<=0.1f && this.hor_input>=-0.1f){
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=0f;
                this.rigid.linearVelocity=this.velcopy_x;
            }

            if(Input.GetKey(KeyCode.C) && this.rigid.linearVelocity.y<0.01f && this.rigid.linearVelocity.y>-0.01f){
                //ジャンプ
                this.velcopy_y=this.rigid.linearVelocity;
                this.velcopy_y.y=8.0f;
                this.rigid.linearVelocity=this.velcopy_y;
            }
        }

        if(Input.GetKey(KeyCode.D) && !this.isgrapping){
            //引き寄せ
            this.isgrapping=true;
            this.grapvelocity = (enemy.transform.position-this.rigid.transform.position) * 2.7f;
            this.pos_tomove = enemy.transform.position-this.grapvelocity.normalized;
        }

        if(Input.GetKey(KeyCode.X)){
            if(this.attackkeyholdtime==0){
                this.attackkeyholdtime=20;
            }
        }

        if(this.attackkeyholdtime<8){
            this.attackobj.transform.position=this.hide;
        }
        else{
            this.attackobj.transform.position=new Vector3(this.transform.position.x+0.75f, this.transform.position.y, 0f);
        }

        if(this.attackkeyholdtime>0){this.attackkeyholdtime--;}

        if(this.isgrapping){
            this.grap(this.enemy);
        }
    }

    void grap(GameObject enemy){
        this.rigid.linearVelocity=this.grapvelocity;
        if((enemy.transform.position-this.transform.position).magnitude<=1.5f){
            this.rigid.linearVelocity=Vector3.zero;
            this.isgrapping=false;
        }
    }
}
