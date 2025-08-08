using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public Sprite attendFlagImage;
    private SpriteRenderer renderer;
    [HideInInspector] public bool isAttended = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = attendFlagImage;
            isAttended = true;
        }
    }
}
