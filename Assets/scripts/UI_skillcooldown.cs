using UnityEngine;
using UnityEngine.UI;

public class UI_skillcooldown : MonoBehaviour
{
    //public SpriteRenderer sprite;
    //public Sprite in_cooldown;
    //public Sprite ready;-
    //[System.NonSerialized] public float alphaCutoffDelta;
    [HideInInspector] public SpriteMask mask;
    void Start()
    {
        mask = GameObject.Find("Sprite Mask").GetComponent<SpriteMask>();
        //this.alphaCutoffDelta = 1.0f / (float)(PlayerData.main.skillinit);
    }
    void FixedUpdate()
    {
        /*if(this.mask.alphaCutoff>0.0f)
            this.mask.alphaCutoff-=this.alphaCutoffDelta;
        else
            this.mask.alphaCutoff=0.0f;*/
    }
}
