using UnityEngine;

public class present : leveling_factor
{
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")){
            xp_get(3);
            Destroy(this.gameObject);
        }
    }
}
