using UnityEngine;

public class enemy : MonoBehaviour
{
    int life;
    int kb_time=0;
    float kb_resist=1.0f;
    bool cankb=true;
    bool hitflag=false;
    bool canmove=true;
    public player player_sc;
    public Rigidbody2D rigid;
    Vector3 kb_vec;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        this.life=30;
        //this.Collider=GetComponent<BoxCollider2D>();
        //this.player=GameObject.Find("player").GetComponent<player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.player_sc.attacktime==0 && this.hitflag){
            this.hitflag=false;
        }
        if(this.kb_time > 0){
            this.kb_time--;
            this.canmove=false;
        }
        else{
            this.canmove=true;
        }
        if(this.transform.position.y<-10f){
            Destroy(this.gameObject);
        }
        if(this.life<=0){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("attack") && 4<=this.player_sc.attacktime && !this.hitflag){
            this.hitflag=true;
            this.life--;
            this.player_sc.ishitted=true;
            switch(this.player_sc.attack_direction){
                case 1:
                Debug.Log("up");
                break;
                case -1:
                Debug.Log("down");
                break;
                default:
                Debug.Log("neutral");
                break;
            }
            if(this.cankb){
                this.kb_vec.Set((this.player_sc.attack_direction == 0 ? 3.0f : 0f) * player_sc.transform.localScale.x * 10f, 3.0f * this.player_sc.attack_direction, 0f);
                this.knockback(this.kb_vec / this.kb_resist);
            }
            if(this.life<=0){
                Destroy(this.gameObject);
            }
        }
    }

    void knockback(Vector3 vec){
        this.rigid.linearVelocity=Vector3.zero;
        this.rigid.AddForce(vec, ForceMode2D.Impulse);
        this.kb_time=20;
    }
}
