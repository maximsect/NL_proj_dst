using UnityEngine;
using System.Collections;

public class kawaraoni : EnemyBaseScript
{
    public float interval = 2f;
    public float firetime = 0.5f;
    public float attackoffset = 1.0f;
    public GameObject LaserPref;
    public Animator anim;
    GameObject laserObj;
    
    short in_interval = 0;
    short invincible=0;
    public AudioClip fireSound;

    void FixedUpdate()
    {
        if(this.in_interval <= 1)
            this.transform.localScale=new Vector3(GameManager.player.transform.position.x - this.transform.position.x < 0f ? 1 : -1, 1, 1);
        if(Mathf.Abs(GameManager.player.transform.position.y - this.transform.position.y) < this.attackoffset && this.in_interval == 0){
            this.in_interval = 2;
            this.anim.SetBool("fire", true);
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(this.firetime);
        GameManager.main.PlayOneShot(fireSound);
        GameObject laserObj = Instantiate(LaserPref, transform.position, Quaternion.identity);
        laserObj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-10f * this.transform.localScale.x, 0f);
        laserObj.transform.localScale = this.transform.localScale;
        yield return new WaitForSeconds(0.2f);
        this.in_interval = 1;
        this.anim.SetBool("fire", false);
        yield return new WaitForSeconds(this.interval - 0.2f);
        Destroy(laserObj);
        this.in_interval = 0;
        yield break;
    }
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("ground"))
            Debug.Log(collision.collider.name);
        if (collision.collider.name == "attack" && this.invincible == 0)
        {
            this.hp -= 10;
            this.invincible = 1;
            if (this.hp <= 0)
                Destroy(this.gameObject);
        }
        if (collision.collider.name == "skillattack" && this.invincible == 0)
        {
            this.hp -= 30;
            this.invincible = 1;
            if (this.hp <= 0)
                Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("arrow") && this.invincible == 0)
        {
            this.hp -= 5;
            if (this.hp <= 0)
                Destroy(this.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.collider.name == "attack" || collision.collider.name == "skillattack") && this.invincible == 1)
        {
            this.invincible = 0;
        }
    }/**/
}
