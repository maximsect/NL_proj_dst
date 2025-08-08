using UnityEngine;
using System.Collections;

public class kawaraoni : MonoBehaviour
{
    public int hp=10000;
    public float interval = 2f;
    public float firetime = 0.5f;
    public float attackoffset = 1.0f;
    public GameObject LaserPref;
    GameObject laserObj;
    
    public short in_interval = 0;

    void FixedUpdate()
    {
        if(this.in_interval == 0)
            this.transform.localScale=new Vector3(GameManager.main.player.transform.position.x - this.transform.position.x < 0f ? 1 : -1, 1, 1);
        if(Mathf.Abs(GameManager.main.player.transform.position.y - this.transform.position.y) < this.attackoffset && this.in_interval == 0){
            this.in_interval = 1;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(this.firetime);
        GameObject laserObj = Instantiate(LaserPref, transform.position, Quaternion.identity);
        laserObj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-10f * this.transform.localScale.x, 0f);
        yield return new WaitForSeconds(this.interval);
        Destroy(laserObj);
        this.in_interval = 0;
        yield break;
    }
}
