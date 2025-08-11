using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemyPrefs = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemyList.Clear();
        enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    public GameObject TheMostDistantObjectOnScreen()
    {
        List<GameObject> objOnScreenList = enemyList.Where(item => (item.GetComponent<SpriteRenderer>() != null && item.GetComponent<SpriteRenderer>().isVisible)).ToList();
        if (objOnScreenList.Count == 0) return null;
        if (objOnScreenList.Count == 1) return objOnScreenList[0];
        GameObject obj = null;
        float distance = 0;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null)
            {
                Vector3 pos = enemyList[i].transform.position - GameObject.Find("player").transform.position;
                obj = (distance < pos.magnitude) ? enemyList[i] : obj;
                distance = (distance < pos.magnitude) ? pos.magnitude : distance;

            }
        }
        return obj;
    }
}
