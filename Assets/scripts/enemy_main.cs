using UnityEngine;

public class enemy_main : MonoBehaviour
{
    int life=5;
    bool hitflag=false;
    public player_main player_sc;
    int xp=1;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.hitflag){
            this.hitflag=false;
        }
        if(this.life<=0){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("attack") && !this.hitflag){
            damage(1);
            Debug.Log("attack");
        }
        if (collider.gameObject.CompareTag("skillattack") && !this.hitflag){
            damage(2);
            Debug.Log("skillattack");
        }
        else if(collider.gameObject.CompareTag("arrow") && !this.hitflag){
            damage(1);
            Debug.Log("arrow");
        }
    }
    // && 8<=this.player_sc.attackkeyholdtime
    //this.player_sc.attackkeyholdtime==0 && 

    void damage(int n){
        this.hitflag=true;
        this.life-=n;
        if(this.life<=0){
            this.player_sc.xp+=this.xp;
            Destroy(this.gameObject);
        }
    }
}
