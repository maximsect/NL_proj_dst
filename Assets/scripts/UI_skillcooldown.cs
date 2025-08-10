using UnityEngine;
using UnityEngine.UI;

public class UI_skillcooldown : MonoBehaviour
{
    //public SpriteRenderer sprite;
    //public Sprite in_cooldown;
    //public Sprite ready;
    [HideInInspector] public SpriteMask mask;
    void Start()
    {
        mask = GameObject.Find("Sprite Mask").GetComponent<SpriteMask>();
    }
    void FixedUpdate()
    {/*
        if(this.player.skillcooldown<=0)
            this.sprite.sprite=this.ready;
        else
            this.sprite.sprite=this.in_cooldown;*/

        if(player_main.main.skillcooldown>0)
            this.mask.alphaCutoff-=0.008f;
        else
            this.mask.alphaCutoff=0.0f;
    }
}
