using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject player;
    float minimum_x=-30f;
    float maximum_x=34f;
    float minimum_y=-3f;
    float maximum_y=30f;
    Vector3 test_battle_pos=new Vector3(34f,3f,-10f);
    float test_battle_begin_pos=28f;
    bool[] pos_fixed=new bool[4]{false, false, false, false};
    bool f=false;
    float c=0f;
    Vector3 lastcamerapos;

    void Start()
    {
        //this.player=GameObject.Find("player");
    }

    void Update()
    {
        Vector3 playerpos=this.player.transform.position;

        if(playerpos.x>this.test_battle_begin_pos && !this.f){
            //展示用に無効化
            //this.lastcamerapos=this.transform.position;
            //his.c=20f;
            //this.f=true;
        }

        if(!this.f){
            if(playerpos.x<this.minimum_x){
                this.pos_fixed[0]=true;
            }
            if(playerpos.y<this.minimum_y){
                this.pos_fixed[1]=true;
            }
            if(playerpos.x>this.maximum_x){
                this.pos_fixed[2]=true;
            }
            if(playerpos.y>this.maximum_y){
                this.pos_fixed[3]=true;
            }
            transform.position=new Vector3(playerpos.x, playerpos.y, transform.position.z);
            if(this.pos_fixed[0])
                transform.position=new Vector3(this.minimum_x, playerpos.y, transform.position.z);
            if(this.pos_fixed[1])
                transform.position=new Vector3(playerpos.x, this.minimum_y, transform.position.z);
            if(this.pos_fixed[2])
                transform.position=new Vector3(this.maximum_x, playerpos.y, transform.position.z);
            if(this.pos_fixed[3])
                transform.position=new Vector3(playerpos.x, this.maximum_y, transform.position.z);
            this.pos_fixed=new bool[4]{false, false, false, false};
        }
        if(Input.GetKeyDown("f")){
            this.f=false;
        }
    }

    void FixedUpdate(){
        if(this.c>0f){
            this.c--;
            this.transform.position=this.lastcamerapos/20f*this.c+this.test_battle_pos/20f*(20f-this.c);
        }
    }
}
