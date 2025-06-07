using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_maneger : MonoBehaviour
{

    public GameObject newPrefab;
    public float intervalSec = 3f;
    public int newZ = -5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("CreatePrefab", intervalSec, intervalSec);
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CreatePrefab()
    {
        float area_x = 9f;
        float area_y = 4f;

        Vector3 newPos = transform.position;
        newPos.x = UnityEngine.Random.Range(-area_x / 2, area_x / 2);
        newPos.y = UnityEngine.Random.Range(-area_y / 2, area_y / 2);
        newPos.z = newZ;

        GameObject newGameObject = Instantiate(newPrefab) as GameObject;
        newGameObject.transform.position = newPos;
    }
}
