using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{

    BoxCollider2D Collider;
    LayerMask objects;
    private GameObject object_a; 

    // Start is called before the first frame update
    void Start()
    {
        this.Collider=GetComponent<BoxCollider2D>();
        this.objects=LayerMask.GetMask("object");
        this.object_a=transform.Find("object").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    void OnTriggerStay2D(Collider2D other){        
        if(this.Collider.IsTouchingLayers(this.objects)){
            Debug.Log("a");
        }
    }
}
