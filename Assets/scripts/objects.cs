using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objects : MonoBehaviour
{
    int object_life;
    bool hitflag=false;
    public GameObject block;
    public player player;
    public objectmaker obj;
    // Start is called before the first frame update
    //int attacktime;
    void Start()
    {
        this.object_life=3;
        //this.Collider=GetComponent<BoxCollider2D>();
        //this.player=GameObject.Find("player").GetComponent<player>();
        //this.obj=GameObject.Find("objectmaker").GetComponent<objectmaker>();
        //Debug.Log(gameObject.ToString());
        //this.block=GameObject.Find(gameObject.ToString());
        //this.attacktime=GameObject.Find("player").GetComponent<player>().attacktime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if(this.Collider.IsTouchingLayers(this.attack)==true){Debug.Log("detect");}
        if(this.player.attacktime==0 && this.hitflag){
            this.hitflag=false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("attack") && 4<=this.player.attacktime && !this.hitflag){
            this.hitflag=true;
            this.object_life--;
            this.player.ishitted=true;
            if(this.object_life<=0){
                this.obj.count++;
                Destroy(this.gameObject);
            }
        }
    }
}

//attackのrigidbodyを削除しました。オブジェクトの動作に異常が出た場合はそちらの追加も試してみてください。
//加えて、objectのcolliderをtriggerにしました。attackはもともとtriggerになっています。