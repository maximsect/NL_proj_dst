using UnityEngine;

public class UI_skillcooldown : MonoBehaviour
{
    public player_main player;
    public SpriteRenderer sprite;
    public Sprite in_cooldown;
    public Sprite ready;

    void FixedUpdate()
    {
        if(this.player.skillcooldown<=0)
            this.sprite.sprite=this.ready;
        else
            this.sprite.sprite=this.in_cooldown;
    }
}
