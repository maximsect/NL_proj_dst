using UnityEngine;

public class presentmaker : MonoBehaviour
{

    readonly float interval=10f;
    public GameObject newPrefab;
    float area_x = 9f;
    float area_y = 12f;
    
    void Start()
    {
        InvokeRepeating("CreatePrefab", interval, interval);
    }

    void FixedUpdate()
    {
        
    }

    void CreatePrefab()
    {

        Vector3 newPos = transform.position;
        newPos.x = UnityEngine.Random.Range(-this.area_x / 2, this.area_x / 2);
        newPos.y = UnityEngine.Random.Range(-this.area_y / 2 + 2, this.area_y / 2 + 2);
        newPos.z = 0f;

        GameObject newGameObject = Instantiate(newPrefab) as GameObject;
        newGameObject.transform.position = newPos;
    }
}
