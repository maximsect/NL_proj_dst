using UnityEngine;

public class arrow : MonoBehaviour
{

    public Rigidbody2D rigid;
    Vector2 nogravity = new Vector2(7.5f, 0f);
    player_main player;
    public PlayerData playerData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(this.gameObject.name=="arrow"){
            this.rigid.constraints=RigidbodyConstraints2D.FreezePosition;
        }
        else{
            this.rigid.constraints=RigidbodyConstraints2D.None;
            this.nogravity*=PlayerData.main.direction;
            this.transform.localScale=new Vector3(-PlayerData.main.direction, 1, 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.rigid.linearVelocity=this.nogravity;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("arrow"))
            Destroy(this.gameObject);
    }
}
