using UnityEngine;

public class assignment : MonoBehaviour
{
    public GameObject assign;
    public Rigidbody2D rigid;
    public GameObject player;

    int attacktimer=0;
    float direction=1f;
    public float relativeX;
    int freezetimer=0;

    readonly Vector3 hide=new Vector3(100f, 100f, 0f);
    readonly Vector3 visible=new Vector3(1.75f, 0f, 0f);
    readonly int ATTACKBEGIN=70;
    readonly int ATTACKEND=50;
    readonly int ATTACKDEFAULT=90;
    readonly float ATTACKFREEZEEND=30;
    readonly float ATTACKDISTANCE=2.5f;
    readonly float MOVESCALE=0.5f;
    readonly float MAXSPEED=0.5f;
    readonly float MAXFORCE=1.5f;

    void FixedUpdate()
    {
        this.direction = this.player.transform.position.x < this.transform.position.x ? -1f : 1f;
        this.assign.transform.position=this.hide;
        this.relativeX=(this.transform.position - this.player.transform.position).x;

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
        if(this.attacktimer<=0 && Vector3.Distance(this.transform.position, this.player.transform.position)<=this.ATTACKDISTANCE)
            this.attacktimer=this.ATTACKDEFAULT;
        if(this.attacktimer>0){
            this.attacktimer--;
            if(this.attacktimer<=this.ATTACKBEGIN && this.attacktimer>=this.ATTACKEND){
                this.assign.transform.position=this.visible*this.direction+this.transform.position;
            }
        }
    }
}
