using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class player_old : MonoBehaviour
{
    
    public Rigidbody2D m_rigidbody;
    public BoxCollider2D Collider;
    public EdgeCollider2D footline;
    public float hor_input=0f;
    public float ver_input=0f;
    //int jump=0;
    float maxspeedX=4.0f;
    float maxfallspeed=-9.0f;
    Vector2 velcopy_x;
    Vector2 velcopy_y;
    public LayerMask ground;
    public LayerMask objects;
    public bool isground=false;
    public bool iswallhit=false;
    //int dirflag=0;
    public GameObject attack;
    public BoxCollider2D attack_collider;
    public int attacktime=0;
    bool forwardisright=true;
    public int attack_direction=0;
    bool jumpkeyhold=false;
    bool attackkeyhold=false;
    public bool jumpingflag=true;
    bool twicejumpflag=false;
    public GameObject returnpoint;
    public returnpoint ret_src;
    Vector3 ret_pos=new Vector3(0f, 0f, 0f);
    float last_velocity_y=0f;
    int jumpkeyholdtime=0;
    float will_jump=0f;
    int jumptimeout=0;
    int freezetime=0;
    public bool ishitted=false;
    bool isattackhopped=false;
    //bool isjumpkeyactive=false;
    //int jump_times=0;

    //keyconfig可能にする予定のもの
    public int minijumpframe=6;

    // Start is called before the first frame update
    void Start()
    {
        //this.Collider=GetComponent<BoxCollider2D>();
        //this.footline=GetComponent<EdgeCollider2D>();
        //this.m_rigidbody=GetComponent<Rigidbody2D>();
        //this.ground=LayerMask.GetMask("ground");
        //this.objects=LayerMask.GetMask("object");
        //this.returnpoint=GameObject.Find("returnpoint");
        //this.ret_src=this.returnpoint.GetComponent<returnpoint>();
        //this.attack=transform.Find("attack").gameObject;
        //this.attack_collider=this.attack.GetComponent<BoxCollider2D>();
        //Debug.Log("Screen Width : " + Screen.width);
        //Debug.Log("Screen  height: " + Screen.height);

        //Time.timeScale=0.1f; :スローモーションで動かしたいとき用
    }

    // Update is called once per frame
    void Update(){
        this.hor_input=Input.GetAxisRaw("Horizontal");
        this.ver_input=Input.GetAxisRaw("Vertical");
        this.maxspeedX=4.0f*Time.fixedDeltaTime*50f;
        this.maxfallspeed=-12.0f*Time.fixedDeltaTime*50f;

        if(!Input.GetKey(KeyCode.X)){this.attackkeyhold=false;}

        //if(Input.GetKeyDown("z")){this.isjumpkeyactive=true;}
        //else{isjumpkeyactive=false;}
        if(this.attacktime<4){
            this.attack.SetActive (false);
            this.ishitted=false;
        }
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
        
        if(Input.GetKeyDown(KeyCode.R) || this.transform.position.y<-10f){
            //リセット
            this.reset();
        }

    }

    void FixedUpdate(){

        this.Collider.enabled=true;
        
        if(!Input.GetKey(KeyCode.Z)){this.jumpkeyhold=false;}
        else{this.jumpkeyholdtime++;}

        //player_move

        if(this.footline.IsTouchingLayers(this.ground)){
            this.isground=true;
            //Debug.Log("istouching");
        }
        else{
            this.isground=false;
        }
        if(this.Collider.IsTouchingLayers(this.ground) && (this.m_rigidbody.linearVelocity.y!=0f || this.m_rigidbody.linearVelocity.x==0f) && this.hor_input * (this.forwardisright? 1f:-1f)>0.1f){
            this.iswallhit=true;
            //Debug.Log("istouching");
        }
        else{
            this.iswallhit=false;
        }
        
        //Debug.Log(this.last_detect!=0f);

        if(this.m_rigidbody.linearVelocity.y==0f && this.last_velocity_y==0f){
            //this.isground && !this.iswallhit && 
            //接地判定
            this.twicejumpflag=false;
            this.jumpingflag=false;
        }
        else{this.jumpingflag=true;}

        //if(!this.isground && !this.iswallhit){Debug.Log("b");}

        if(this.jumptimeout==1){
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
        }

        if(this.freezetime>0){
            this.freezetime--;
        }
        else{
            if(this.hor_input>0.1f && (!this.m_rigidbody.IsTouchingLayers(this.ground) || this.footline.IsTouchingLayers(this.ground)) && (!this.iswallhit || (this.m_rigidbody.linearVelocity.y==0f && this.last_velocity_y==0f))){
                //右移動
                this.velcopy_x=this.m_rigidbody.linearVelocity;
                this.velcopy_x.x=this.maxspeedX;
                this.m_rigidbody.linearVelocity=this.velcopy_x;
                this.forwardisright=true;
            }
            if(this.hor_input<-0.1f && (!this.m_rigidbody.IsTouchingLayers(this.ground) || this.footline.IsTouchingLayers(this.ground)) && (!this.iswallhit || (this.m_rigidbody.linearVelocity.y==0f && this.last_velocity_y==0f))){
                //左移動
                this.velcopy_x=this.m_rigidbody.linearVelocity;
                this.velcopy_x.x=-this.maxspeedX;
                this.m_rigidbody.linearVelocity=this.velcopy_x;
                this.forwardisright=false;
            }
            if(Input.GetKey(KeyCode.Z) && this.m_rigidbody.linearVelocity.y==0f && this.isground && !this.iswallhit && !this.jumpkeyhold){
                //ジャンプ
                /*this.velcopy_y=this.m_rigidbody.velocity;
                this.velcopy_y.y=12.0f;
                this.m_rigidbody.velocity=this.velcopy_y;
                this.jumpkeyhold=true;*/
                this.will_jump=12.0f;
                this.jumpkeyhold=true;
                //this.jump(12.0f);
                this.jumpingflag=true;
                this.jumptimeout=6;
                //this.jump_times=1;
            }
            if(Input.GetKey(KeyCode.Z) && !this.jumpkeyhold && !this.twicejumpflag){
                //2段ジャンプ
                /*this.velcopy_y=this.m_rigidbody.velocity;
                this.velcopy_y.y=10.0f;
                this.m_rigidbody.velocity=this.velcopy_y;
                this.jumpkeyhold=true;*/
                this.jumpkeyhold=true;
                this.jumptimeout=6;
                if(!this.jumpingflag){
                    this.will_jump=12.0f;
                    //this.jump(12.0f);
                    this.jumpingflag=true;
                }
                else{
                    this.will_jump=10.0f;
                    //this.jump(10.0f);
                    this.twicejumpflag=true;
                }
            }
        }

        if(Mathf.Abs(this.hor_input)<0.1f && this.isground){
            //自動横移動停止(地上)
            /*this.velcopy_x=this.m_rigidbody.velocity;
            this.velcopy_x.x=this.velcopy_x.x*0.8f;
            if(Mathf.Abs(this.velcopy_x.x)<0.01f){this.velcopy_x.x=0f;}
            this.m_rigidbody.velocity=this.velcopy_x;*/
            this.velcopy_x=this.m_rigidbody.linearVelocity;
            this.velcopy_x.x=0f;
            this.m_rigidbody.linearVelocity=this.velcopy_x;
        }
        if(Mathf.Abs(this.hor_input)<0.1f && !this.isground){
            //自動横移動完全停止(空中)
            this.velcopy_x=this.m_rigidbody.linearVelocity;
            this.velcopy_x.x=0f;
            this.m_rigidbody.linearVelocity=this.velcopy_x;
        }
        if(this.m_rigidbody.linearVelocity.y<this.maxfallspeed){
            //落下速度制御
            this.velcopy_y=this.m_rigidbody.linearVelocity;
            this.velcopy_y.y=this.maxfallspeed;
            this.m_rigidbody.linearVelocity=this.velcopy_y;
        }

        //player attack
        //if(this.hor_input<-0.1 && this.ver_input<-0.1){Debug.Log(Input.GetKey("x"));}
        if(Input.GetKey(KeyCode.X) && this.attacktime==0 && !this.attackkeyhold){
            this.attack.SetActive (true);
            this.attacktime=16;
            this.isattackhopped=false;
            if(0.1f<this.ver_input){this.attack_direction=1;}
            else if(-0.1f>this.ver_input && this.jumpingflag){this.attack_direction=-1;}
            else{this.attack_direction=0;}
            this.attackkeyhold=true;
        }

        if(this.attacktime>0){
            //攻撃クールタイム
            this.attacktime--;
        }
        if(this.ishitted && this.attack_direction==-1 && !this.isattackhopped){
            //下攻撃ヒット時、跳ねる
            this.ishitted=false;
            this.jump(10.0f);
            this.jumpingflag=true;
            this.twicejumpflag=false;
            this.isattackhopped=true;
        }
        
        /*if(this.attack_collider.IsTouchingLayers(this.objects)){
            Debug.Log("a");
        }*/

        if(this.m_rigidbody.IsTouchingLayers(this.ground) && !this.footline.IsTouchingLayers(this.ground) && !this.isground && !this.iswallhit && this.m_rigidbody.linearVelocity.y==0){
            //バグ対策補正
            //Debug.Log("a");
            //this.transform.position=this.transform.position+new Vector3(0f, 0.000001f, 0f);
            this.Collider.enabled=false;
        }
        
        //Debug.Log(this.Collider.IsTouchingLayers(this.ground));

        this.last_velocity_y=this.m_rigidbody.linearVelocity.y;

        if(this.jumptimeout>=1){this.jumptimeout--;}

    }

    void jump(float height){
        this.velcopy_y=this.m_rigidbody.linearVelocity;
        this.velcopy_y.y=height;
        this.m_rigidbody.linearVelocity=this.velcopy_y;
    }

    void reset(){
        this.ret_pos = this.ret_src.transform.position;
        this.transform.position = this.ret_pos;
        this.m_rigidbody.linearVelocity = new Vector3(0f, 0f, 0f);
        this.freezetime=3;
    }
}

//メモエリア
//attackのrigidbodyを削除しました。オブジェクトの動作に異常が出た場合はそちらの追加も試してみてください。