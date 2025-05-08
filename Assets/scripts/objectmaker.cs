using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectmaker : MonoBehaviour
{
    GameObject[] objects=new GameObject[5];
    public GameObject obj_prefab;
    public objects ob;

    Vector3[,] obj_pos=new Vector3[1,5]{{
        new Vector3(0f, 0f, 0f), new Vector3(10f, 0f, 0f), new Vector3(10f, -3f, 0f), new Vector3(12f, -2f, 0f), new Vector3(0f, 5f, 0f)
    }};
    int mapnumber=0;
    public int count=0;

    // Start is called before the first frame update
    void Start()
    {
        this.count=0;
        //this.obj_prefab=GameObject.Find("object");
        //this.ob.block=this.obj_prefab;

        this.obj_pos[0,0]=new Vector3(0f, 0f, 0f);
        this.obj_pos[0,1]=new Vector3(10f, 0f, 0f);
        this.obj_pos[0,2]=new Vector3(10f, -3f, 0f);
        this.obj_pos[0,3]=new Vector3(12f, -2f, 0f);
        this.obj_pos[0,4]=new Vector3(0f, 5f, 0f);
        
        for(int i=0;i<5;i++){
            this.objects[i]=Instantiate(this.obj_prefab, this.obj_pos[this.mapnumber,i], Quaternion.identity);
            this.objects[i].name="object" + i;
            //this.objects[i].GetComponent<objects>().block=this.objects[i];
        }
        this.obj_prefab.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(this.count>=5){
            this.count=0;
            this.objects=new GameObject[5];
            this.generate();
        }
    }

    void generate(){
        this.obj_prefab.SetActive(true);
        //this.ob.block=this.obj_prefab;
        this.obj_pos[0,0]=new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(0.0f, 30.0f), 0f);
        this.obj_pos[0,1]=new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(0.0f, 30.0f), 0f);
        this.obj_pos[0,2]=new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(0.0f, 30.0f), 0f);
        this.obj_pos[0,3]=new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(0.0f, 30.0f), 0f);
        this.obj_pos[0,4]=new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(0.0f, 30.0f), 0f);
        
        for(int i=0;i<5;i++){
            this.objects[i]=Instantiate(this.obj_prefab, this.obj_pos[this.mapnumber,i], Quaternion.identity);
            this.objects[i].name="object" + i;
            //this.objects[i].GetComponent<objects>().block=this.objects[i];
        }
        this.obj_prefab.SetActive(false);
    }
}
