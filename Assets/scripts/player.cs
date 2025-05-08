using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class player : MonoBehaviour
{
    
    public Rigidbody2D rigid;
    public BoxCollider2D Collider;
    public BoxCollider2D footline;
    float hor_input=0f;
    float ver_input=0f;
    //int jump=0;
    float maxspeedX=4.0f;
    float maxfallspeed=-9.0f;
    Vector2 velcopy_x;
    Vector2 velcopy_y;
    public LayerMask ground;
    public LayerMask objects;
    public GameObject attack;
    public BoxCollider2D attack_collider;
    public GameObject returnpoint;
    public returnpoint ret_src;
    //int dirflag=0;
    [System.NonSerialized] public int attacktime=0;
    public bool isground=false;
    public bool iswallhit=false;
    public bool iswallpush=false;
    bool forwardisright=true;
    [System.NonSerialized] public int attack_direction=0;
    bool attackkeyhold=false;
    bool dashkeyhold=false;
    public bool jumpingflag=true;
    bool jumpkeyhold=false;
    bool twicejumpflag=false;
    Vector3 ret_pos=new Vector3(0f, 0f, 0f);
    float last_velocity_y=0f;
    public int jumpkeyholdtime=0;
    //float will_jump=0f;
    //int jumptimeout=0;
    int freezetime=0;
    public bool ishitted=false;
    bool isattackhopped=false;
    bool jumpend=false;
    //[System.NonSerialized] 
    public bool headhit=false;
    bool jumpfreeze=false;
    public float timescale=1.0f;
    //public int life=5;
    //bool isjumpkeyactive=false;
    //int jump_times=0;
    int kb_time=0;
    [System.NonSerialized]public int dashtime=0;
    bool isdashing=false;
    ContactPoint2D[] contactpoint = new ContactPoint2D[8];
    int contacts;

    //定数エリア
    readonly int JUMPKEYAFFORDANCE=11;
    readonly float JUMPSCALE1=1.0f;
    readonly float JUMPSCALE2=0.75f;
    readonly float JUMPSPEEDBONUS=1.0f;
    readonly Vector3 ATTACKPOS_OFF=new Vector3(1000f, 1000f, 0f);
    readonly int MAXDASHTIME=30;
    readonly int DASHENDTIME=18;
    readonly float MAXDASHSPEED=0.5f;

    //keyconfig可能にする予定のもの
    //public int minijumpframe=6;

    // Start is called before the first frame update
    void Start()
    {
        //this.Collider=GetComponent<BoxCollider2D>();
        //this.footline=GetComponent<EdgeCollider2D>();
        //this.rigid=GetComponent<rigid2D>();
        //this.ground=LayerMask.GetMask("ground");
        //this.objects=LayerMask.GetMask("object");
        //this.returnpoint=GameObject.Find("returnpoint");
        //this.ret_src=this.returnpoint.GetComponent<returnpoint>();
        //this.attack=transform.Find("attack").gameObject;
        //this.attack_collider=this.attack.GetComponent<BoxCollider2D>();
        //Debug.Log("Screen Width : " + Screen.width);
        //Debug.Log("Screen  height: " + Screen.height);
    }

    // Update is called once per frame
    void Update(){

        Time.timeScale=this.timescale;

    }

    void FixedUpdate(){

        this.hor_input=Input.GetAxisRaw("Horizontal");
        this.ver_input=Input.GetAxisRaw("Vertical");
        this.maxspeedX=4.0f*Time.fixedDeltaTime*50f;
        this.maxfallspeed=-12.0f*Time.fixedDeltaTime*50f;

        if(!Input.GetKey(KeyCode.X)){this.attackkeyhold=false;}
        if(!Input.GetKey(KeyCode.Z)){this.dashkeyhold=false;}

        //if(Input.GetKeyDown("z")){this.isjumpkeyactive=true;}
        //else{isjumpkeyactive=false;}
        /*if(hor_input<0)
            dirflag=1;
        if(hor_input>0)
            dirflag=0;*/

        if(this.forwardisright==true){
            //向き調整(右)
            this.transform.localScale=new Vector3(0.1f, 0.1f, 1);
        }
        else{
            //向き調整(左)
            this.transform.localScale=new Vector3(-0.1f, 0.1f, 1);
        }

        if(this.attack_direction==1){
            //攻撃向き調整(上)
            this.attack.transform.rotation=Quaternion.Euler(0, 0, 90 * (this.forwardisright ? 1:-1));
            this.attack.transform.position=new Vector3(0f, 0.8f, 0f) + this.transform.position;
        }
        else if(this.attack_direction==-1){
            //攻撃向き調整(下)
            this.attack.transform.rotation=Quaternion.Euler(0, 0, -90 * (this.forwardisright ? 1:-1));
            this.attack.transform.position=new Vector3(0f, -0.8f, 0f) + this.transform.position;
        }
        else{
            //攻撃向き調整(横)
            this.attack.transform.rotation=Quaternion.Euler(0, 0, 0);
            this.attack.transform.position=new Vector3(8f * this.transform.localScale.x, 0f, 0f) + this.transform.position;
        }
        
        //攻撃中でないなら下斬りで跳んだ判定をリセット
        if(this.attacktime<4){
            //this.attack.SetActive (false);
            this.attack.transform.position=this.ATTACKPOS_OFF;
            this.ishitted=false;
        }

        this.Collider.enabled=true;

        if(!Input.GetKey(KeyCode.C) && this.jumpkeyhold){this.jumpend=true;}
        
        if(!Input.GetKey(KeyCode.C)){this.jumpkeyhold=false;this.headhit=false;}
        else{this.jumpkeyholdtime++;}

        //player_move

        this.contacts=this.rigid.GetContacts(this.contactpoint);

        if(this.contacts/4>0){
            this.iswallhit=true;
            //Debug.Log("istouching");
            //this.Collider.IsTouchingLayers(this.ground) && (this.rigid.linearVelocity.y!=0f || this.rigid.linearVelocity.x==0f)
        }
        else{
            this.iswallhit=false;
        }
        if(this.contacts/4>0 && this.hor_input * (this.forwardisright? 1f:-1f)>0.1f){
            this.iswallpush=true;
            //Debug.Log("istouching");
        }
        else{
            this.iswallpush=false;
        }
        this.isground=false;
        if(this.contacts%4==2){
            this.contacts=this.footline.GetContacts(this.contactpoint);
            if(this.contacts>=2)
                this.isground=true;
            //Debug.Log("istouching");
            //this.footline.IsTouchingLayers(this.ground)
        }
        
        //Debug.Log(this.last_detect!=0f);

        if(Mathf.Abs(this.rigid.linearVelocity.y)<0.01f && Mathf.Abs(this.last_velocity_y)<0.01f && !this.isdashing){
            //this.isground && !this.iswallhit && 
            //接地判定
            this.twicejumpflag=false;
            this.jumpingflag=false;
            if(!Input.GetKey(KeyCode.C))
                this.jumpkeyholdtime=0;
        }
        else{this.jumpingflag=true;}
        //ダッシュ中は接地判定を行わない

        /*if(this.isground && this.iswallhit && this.last_velocity_y!=0 && this.rigid.linearVelocity.y==0){
            Debug.Log("1");
        }*/

        /*if(this.jumptimeout==1){
            //ジャンプ実行
            //Debug.Log(this.jumpkeyhold);
            if(this.will_jump==12.0f){this.jumpingflag=true;}
            else{this.twicejumpflag=true;}
            //小ジャンプ
            //if(this.jumpkeyholdtime==1 || this.jumpkeyholdtime==2){this.will_jump/=2f;}
            if(this.jumpkeyholdtime<this.minijumpframe){this.will_jump/=1.5f;}
            this.jump(this.will_jump);
            this.will_jump=0f;
            this.jumpkeyholdtime=0;
        }*/

        if(this.freezetime>0){
            this.freezetime--;
        }
        else{
            if(this.hor_input>0.1f && (!this.rigid.IsTouchingLayers(this.ground) || this.footline.IsTouchingLayers(this.ground)) && !this.iswallpush && !iskbing() && !this.isdashing){
                //右移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
                this.forwardisright=true;
            }
            if(this.hor_input<-0.1f && (!this.rigid.IsTouchingLayers(this.ground) || this.footline.IsTouchingLayers(this.ground)) && !this.iswallpush && !iskbing() && !this.isdashing){
                //左移動
                this.velcopy_x=this.rigid.linearVelocity;
                this.velcopy_x.x=-this.maxspeedX;
                this.rigid.linearVelocity=this.velcopy_x;
                this.forwardisright=false;
            }
            if(Input.GetKey(KeyCode.C) && this.isground && !this.jumpkeyhold && !this.jumpfreeze && !iskbing() && !this.isdashing){
                //ジャンプ  !this.iswallhit &&
                /*this.velcopy_y=this.rigid.velocity;
                this.velcopy_y.y=12.0f;
                this.rigid.velocity=this.velcopy_y;
                this.jumpkeyhold=true;*/
                //this.will_jump=12.0f;
                this.jumpkeyhold=true;
                //this.jump(12.0f);
                this.jumpingflag=true;
                //this.jumptimeout=6;
                //this.jump_times=1;
                //&& this.rigid.velocity.y==0f
            }
            if(Input.GetKey(KeyCode.C) && !this.jumpkeyhold && !this.twicejumpflag && !iskbing() && !this.isdashing){
                //2段ジャンプ
                /*this.velcopy_y=this.rigid.velocity;
                this.velcopy_y.y=10.0f;
                this.rigid.velocity=this.velcopy_y;
                this.jumpkeyhold=true;*/
                this.jumpkeyhold=true;
                //this.jumptimeout=6;
                if(!this.jumpingflag && !this.jumpfreeze){
                    //this.will_jump=12.0f;
                    //this.jump(12.0f);
                    this.jumpingflag=true;
                }
                else{
                    //this.will_jump=10.0f;
                    //this.jump(10.0f);
                    this.twicejumpflag=true;
                    //this.jumpfreeze=true;
                }
            }

            //ジャンプ実行
            if(Input.GetKey(KeyCode.C) && this.jumpkeyhold && this.jumpkeyholdtime <= this.JUMPKEYAFFORDANCE){
                if(this.last_velocity_y>0f && Mathf.Abs(this.rigid.linearVelocity.y)<0.01f && !this.isground){
                    this.headhit=true;
                }
                else{
                    if(!this.twicejumpflag)
                        this.rigid.linearVelocity=new Vector2(this.rigid.linearVelocity.x, this.JUMPSCALE1 * (1.5f*this.JUMPKEYAFFORDANCE-this.jumpkeyholdtime));
                    else
                        this.rigid.linearVelocity=new Vector2(this.rigid.linearVelocity.x, this.JUMPSCALE2 * (1.5f*this.JUMPKEYAFFORDANCE-this.jumpkeyholdtime));
                    if(this.rigid.linearVelocity.y > 3.0f)
                        this.rigid.linearVelocity=new Vector2(this.rigid.linearVelocity.x, this.rigid.linearVelocity.y * this.JUMPSPEEDBONUS);
                }
                //Debug.Log(this.rigid.linearVelocity.y);
            }
            /*if(Input.GetKey(KeyCode.C) && this.jumpkeyhold && this.jumpkeyholdtime <= this.JUMPKEYAFFORDANCE){
                //速度を単純化
                if(this.last_velocity_y>0f && this.rigid.velocity.y==0f){
                    this.headhit=true;
                    //Debug.Log("a");
                }
                else{
                    if(!this.twicejumpflag)
                        this.rigid.velocity=new Vector2(this.rigid.velocity.x, this.JUMPSCALE1 * 20f);
                    else
                        this.rigid.velocity=new Vector2(this.rigid.velocity.x, this.JUMPSCALE2 * 20f);
                    if(this.jumpkeyholdtime<6)
                        this.rigid.velocity=new Vector2(this.rigid.velocity.x, this.rigid.velocity.y * this.JUMPSPEEDBONUS);
                }
            }*/
            if(this.headhit && this.rigid.linearVelocity.y>=0.01f){
                this.rigid.linearVelocity=new Vector2(this.rigid.linearVelocity.x, 0f);
                this.jumpkeyholdtime=38;
            }
            if(this.jumpend && this.jumpkeyholdtime <= this.JUMPKEYAFFORDANCE){
                this.rigid.linearVelocity=new Vector2(this.rigid.linearVelocity.x, this.rigid.linearVelocity.y * this.jumpkeyholdtime / this.JUMPKEYAFFORDANCE * 0.5f);
                this.jumpend=false;
                this.jumpkeyholdtime=0;
            }
            else if(this.jumpend){this.jumpend=false;this.jumpkeyholdtime=0;}

            //if(Input.GetKey(KeyCode.C) && this.jumpkeyholdtime==this.JUMPKEYAFFORDANCE)
            //    Debug.Log("a");
            
        }

        if(Mathf.Abs(this.hor_input)<0.1f && this.isground && !iskbing()){
            //自動横移動停止(地上)
            /*this.velcopy_x=this.rigid.velocity;
            this.velcopy_x.x=this.velcopy_x.x*0.8f;
            if(Mathf.Abs(this.velcopy_x.x)<0.01f){this.velcopy_x.x=0f;}
            this.rigid.velocity=this.velcopy_x;*/
            this.velcopy_x=this.rigid.linearVelocity;
            this.velcopy_x.x=0f;
            this.rigid.linearVelocity=this.velcopy_x;
        }
        if(Mathf.Abs(this.hor_input)<0.1f && !this.isground && !iskbing()){
            //自動横移動完全停止(空中)
            this.velcopy_x=this.rigid.linearVelocity;
            this.velcopy_x.x=0f;
            this.rigid.linearVelocity=this.velcopy_x;
        }
        if(this.rigid.linearVelocity.y<this.maxfallspeed){
            //落下速度制御
            this.velcopy_y=this.rigid.linearVelocity;
            this.velcopy_y.y=this.maxfallspeed;
            this.rigid.linearVelocity=this.velcopy_y;
        }

        //攻撃
        //if(this.hor_input<-0.1 && this.ver_input<-0.1){Debug.Log(Input.GetKey("x"));}
        if(Input.GetKey(KeyCode.X) && this.attacktime<=0 && !this.attackkeyhold && !iskbing()){
            //this.attack.SetActive (true);
            this.attacktime=16;
            //this.isattackhopped=false;
            if(0.1f<this.ver_input){this.attack_direction=1;}
            else if(-0.1f>this.ver_input && this.jumpingflag){this.attack_direction=-1;}
            else{this.attack_direction=0;}
            this.attackkeyhold=true;
            this.isattackhopped=false;
        }

        if(this.attacktime>0){
            //攻撃クールタイム
            this.attacktime--;
        }
        if(this.ishitted && this.attack_direction==-1 && !this.isattackhopped){
            //下攻撃ヒット時、跳ねる

            //this.ishitted=false;
            //↑バグの原因になりそう(複数ヒットしそう...)

            this.jump(8.0f);
            this.jumpingflag=true;
            this.twicejumpflag=false;
            this.isattackhopped=true;
        }

        //ダッシュ
        //if(this.hor_input<-0.1 && this.ver_input<-0.1){Debug.Log(Input.GetKey("x"));}
        if(Input.GetKey(KeyCode.Z)&& !this.dashkeyhold){
            this.dashkeyhold = true;
            if(this.dashtime<=0)
                this.dashtime=this.MAXDASHTIME;
        }

        if(this.dashtime > 0){
            this.isdashing=false;
            if(this.dashtime > this.DASHENDTIME){
                this.rigid.linearVelocity=new Vector3((float)(this.dashtime * (this.forwardisright ? 1:-1))*this.MAXDASHSPEED, 0f, 0f);
                this.isdashing=true;
            }
            this.dashtime--;
        }

        if(this.kb_time > 0){
            this.kb_time--;
        }
        // && !this.isattackhopped
        
        /*if(this.attack_collider.IsTouchingLayers(this.objects)){
            Debug.Log("a");
        }*/

        /*if(this.rigid.IsTouchingLayers(this.ground) && !this.footline.IsTouchingLayers(this.ground) && !this.isground && !this.iswallhit && this.rigid.velocity.y==0){
            //バグ対策補正
            //Debug.Log("a");
            //this.transform.position=this.transform.position+new Vector3(0f, 0.000001f, 0f);
            this.Collider.enabled=false;
        }*/
        //ジャンプのバグ対策
        //if(this.rigid.velocity.y==0f && this.last_velocity_y==0f)
        this.jumpfreeze=false;
        
        //Debug.Log(this.Collider.IsTouchingLayers(this.ground));

        //getcontactsを使う。配列の大きさが2なら地上で壁に触れてない。4なら空中で壁に触れている。6なら地上かつ壁に触れている。
        /*ContactPoint2D[] contacts = new ContactPoint2D[8];
        if(this.rigid.GetContacts(contacts)==4){
            this.rigid.linearVelocity=new Vector3(this.rigid.linearVelocity.x, this.last_velocity_y, 0f);
        }*/

        this.last_velocity_y=this.rigid.linearVelocity.y;

        //if(this.jumptimeout>=1){this.jumptimeout--;}
        
        if(Input.GetKeyDown(KeyCode.R) || this.transform.position.y<-10f){
            //リセット
            this.reset();
        }

    }

    void jump(float height){
        this.velcopy_y=this.rigid.linearVelocity;
        this.velcopy_y.y=height;
        this.rigid.linearVelocity=this.velcopy_y;
    }

    void reset(){
        this.ret_pos = this.ret_src.transform.position;
        this.transform.position = this.ret_pos;
        this.rigid.linearVelocity = new Vector3(0f, 0f, 0f);
        this.freezetime=3;
        this.dashtime=0;
    }

    public void knockback(Vector3 vec){
        this.rigid.linearVelocity=Vector3.zero;
        this.rigid.AddForce(vec, ForceMode2D.Impulse);
        this.kb_time=20;
    }

    bool iskbing(){
        return kb_time > 0 ? true:false;
    }
}

//メモエリア
//attackのrigidを削除しました。オブジェクトの動作に異常が出た場合はそちらの追加も試してみてください。