using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnpoint : MonoBehaviour
{
    BoxCollider2D[] Collider = new BoxCollider2D[2];
    public Vector3 pos=new Vector3(0f, 0f, 0f);
    public player player;
    public LayerMask ground;
    bool resetflag=false;
    // Start is called before the first frame update
    void Start()
    {
        this.Collider=GetComponents<BoxCollider2D>();
        //this.player = GameObject.Find("player").GetComponent<player>();
        //this.ground=LayerMask.GetMask("ground");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //復帰

        if(!this.player.jumpingflag && this.player.dashtime <= 18){
            if(!this.resetflag){
                this.resetflag=true;
                this.pos=this.player.transform.position;
                this.transform.position=this.pos;
            }
            if(this.Collider[0].IsTouchingLayers(this.ground) && this.Collider[1].IsTouchingLayers(this.ground)){
                this.pos=this.player.transform.position;
                this.transform.position=this.pos;
            }
        }
        else{this.resetflag=false;}
    }
}
