using UnityEngine;

public class leveling_factor : MonoBehaviour
{
    private player_main player;

    void Start(){
        this.player=GameManager.player.GetComponent<player_main>();
    }
    
    public void xp_get(int n){
        this.player.xp+=n;
    }
}
