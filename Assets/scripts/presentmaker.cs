using UnityEngine;

public class presentmaker : MonoBehaviour
{

    readonly float interval=10f;
    public GameObject newPrefab;
    
    void Start()
    {
        InvokeRepeating("CreatePrefab", interval, interval);
    }

    void FixedUpdate()
    {
        
    }

    void CreatePrefab()
    {
        float area_x = 9f;
        float area_y = 15f;

        Vector3 newPos = transform.position;
        newPos.x = UnityEngine.Random.Range(-area_x / 2, area_x / 2);
        newPos.y = UnityEngine.Random.Range(-area_y / 2, area_y / 2);
        newPos.z = 0f;

        GameObject newGameObject = Instantiate(newPrefab) as GameObject;
        newGameObject.transform.position = newPos;
    }
}
