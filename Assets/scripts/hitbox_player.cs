using UnityEngine;

public class hitbox_player : MonoBehaviour
{
    public int life=5;
    int inv_time=0;
    public player pl;
    Vector3 kb_vec;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.life=5;
        this.inv_time=0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(0 < this.inv_time){
            this.inv_time--;
        }
    }
    
    void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Enemy") && this.inv_time <= 0){
            this.life--;
            //Debug.Log("damage, last life is " + this.life);
            this.inv_time=40;
            kb_vec=this.transform.position-collider.transform.position;
            //Debug.Log(kb_vec);
            this.kb_vec.Set(0.57735f * (kb_vec.x < 0 ? -1.0f:1.0f), 0.5f, 0f);
            this.pl.knockback(this.kb_vec * 5.0f);
        }
    }
}
