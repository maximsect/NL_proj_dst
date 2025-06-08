using UnityEngine;

public class leveling_factor : MonoBehaviour
{
    public player_main player;

    void Start(){
        this.player=GameObject.Find("player").GetComponent<player_main>();
    }
    
    public void xp_get(int n){
        this.player.xp+=n;
    }
}
