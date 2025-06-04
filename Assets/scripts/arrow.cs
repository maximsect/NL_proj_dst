using UnityEngine;

public class arrow : MonoBehaviour
{

    public Rigidbody2D rigid;
    Vector2 nogravity = new Vector2(5.0f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(this.gameObject.name=="arrow"){
            this.rigid.constraints=RigidbodyConstraints2D.FreezePosition;
        }
        else{
            this.rigid.constraints=RigidbodyConstraints2D.None;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.rigid.linearVelocity=this.nogravity;
    }

    void OnTriggerEnter2D(Collider2D collider){
        Destroy(this.gameObject);
    }
}
