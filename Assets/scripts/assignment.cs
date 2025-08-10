using UnityEngine;

public class assignment : MonoBehaviour
{
    public GameObject assign;
    public Rigidbody2D rigid;
    //public GameObject player;

    int attacktimer=0;
    float direction=1f;
    float relativeX;
    int freezetimer=0;
    int hp=100;
    short invincible=0;

    readonly Vector3 hide=new Vector3(100f, 100f, 0f);
    readonly Vector3 visible=new Vector3(1.75f, 0f, 0f);
    readonly int ATTACKBEGIN=170;
    readonly int ATTACKEND=150;
    readonly int ATTACKDEFAULT=200;
    readonly float ATTACKFREEZEEND=130;
    readonly float ATTACKDISTANCE=2.5f;
    readonly float MOVESCALE=0.5f;
    readonly float MAXSPEED=0.5f;
    readonly float MAXFORCE=1.5f;

    void FixedUpdate()
    {
        if(this.attacktimer<this.ATTACKEND)
            this.direction = GameManager.player.transform.position.x < this.transform.position.x ? -1f : 1f;
        this.assign.transform.position=this.hide;
        this.relativeX=(this.transform.position - GameManager.player.transform.position).x;

        //move
        if(this.attacktimer<=this.ATTACKFREEZEEND){
            if(this.direction*this.relativeX<0f)
                this.rigid.AddForce(new Vector3(this.MAXFORCE*this.direction, 0f, 0f));
            if(Mathf.Abs(this.rigid.linearVelocityX)>this.MAXSPEED && this.direction*this.relativeX>0f)
                this.rigid.linearVelocityX=this.MAXSPEED*this.direction;
        }
        else{
            this.rigid.linearVelocityX=0f;
        }

        //attack
        if(this.attacktimer<=0 && Vector3.Distance(this.transform.position, GameManager.player.transform.position)<=this.ATTACKDISTANCE)
            this.attacktimer=this.ATTACKDEFAULT;
        if(this.attacktimer>0){
            this.attacktimer--;
            if(this.attacktimer<=this.ATTACKBEGIN && this.attacktimer>=this.ATTACKEND){
                this.assign.transform.position=this.visible*this.direction+this.transform.position;
            }
        }
    }
    
    // && Random.Range(0, 2)==0
    
    void OnTriggerEnter2D(Collider2D collider){
        int rand=Random.Range(0, 2);
        if(this.invincible==0){Debug.Log(rand==0 ? "hit" : "miss");}
        if(collider.gameObject.CompareTag("attack") && this.invincible==0 && rand==0){
            this.hp-=10;
            this.invincible=1;
            if(this.hp<=0)
                Destroy(this.gameObject);
        }
        if(collider.gameObject.CompareTag("skillattack") && this.invincible==0 && rand==0){
            this.hp-=30;
            this.invincible=1;
            if(this.hp<=0)
                Destroy(this.gameObject);
        }
        if(rand==1)
            this.invincible=1;
    }

    void OnCollisionEnter2D(Collision2D collision){
        int rand=Random.Range(0, 2);
        if(collision.gameObject.CompareTag("arrow") && this.invincible==0 && rand==0){
            this.hp-=5;
            if(this.hp<=0)
                Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if((collider.gameObject.CompareTag("attack") || collider.gameObject.CompareTag("skillattack")) && this.invincible==1){
            this.invincible=0;
        }
    }
}
