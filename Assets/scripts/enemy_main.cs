using UnityEngine;

public class enemy_main : MonoBehaviour
{
    int life=5;
    bool hitflag=false;
    public player_main player_sc;

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
            damage();
            Debug.Log("attack");
        }
        else if(collider.gameObject.CompareTag("arrow") && !this.hitflag){
            damage();
            Debug.Log("arrow");
        }
    }
    // && 8<=this.player_sc.attackkeyholdtime
    //this.player_sc.attackkeyholdtime==0 && 

    void damage(){
        this.hitflag=true;
        this.life--;
        if(this.life<=0){
            Destroy(this.gameObject);
        }
    }
}
