using UnityEngine;

public class arrowmaker : MonoBehaviour
{

    public GameObject player;
    public GameObject arrow_prefab;
    GameObject arrow;
    [System.NonSerialized]public bool arrowmaking=false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.arrowmaking){
            this.arrow=Instantiate(this.arrow_prefab, this.player.transform.position, Quaternion.identity);
            this.arrow.name="arrows";
            this.arrowmaking=false;
        }
    }
}
