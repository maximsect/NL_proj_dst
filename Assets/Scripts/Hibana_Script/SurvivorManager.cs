using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnemyProducts
{
    public GameObject[] enemyPrefs;
    public float genInterval;
    public int maxNum = 3;
}
public class SurvivorManager : StageManager
{
    public EnemyProducts[] enemyProducts;
    public LayerMask groundLayer;
    public float surviveTime = 30;
    public TMP_Text clock;
    void Start()
    {
        foreach (EnemyProducts enemyProduct in enemyProducts)
        {
            StartCoroutine(EnemyFactory(enemyProduct));
        }
        StartCoroutine(Waiting());
        base.MainStart();
    }
    Vector2 RandomPos()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Random.Range(-10f, 10f), Random.Range(-3f, 10f));
            if (Vector3.Distance(pos.ToVector3(), GameManager.player.transform.position) < 3) continue;
            if (Physics2D.OverlapCircle(pos, 0.5f, groundLayer)) continue;
            if (!Physics2D.OverlapCircle(pos - new Vector2(0, 0.3f), 0.5f, groundLayer)) continue;

            return pos;
        }
    }
    IEnumerator EnemyFactory(EnemyProducts ene)
    {
        List<GameObject> genList = new List<GameObject>();
        int index = 0;
        while (true)
        {
            while (true)
            {
                genList.RemoveAll(item => item == null);
                if (genList.Count < ene.maxNum) break;
                yield return null;
            }
            yield return new WaitForSeconds(ene.genInterval);
            foreach (GameObject enemy in ene.enemyPrefs)
            {
                GameObject generated = Instantiate(enemy);
                generated.transform.position = RandomPos().ToVector3(0);
                genList.Add(generated);
            }
            
        }
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
        float timer = 0;
        for (; timer < surviveTime; timer += Time.deltaTime)
        {
            clock.SetText(((int)(surviveTime - timer)).ToString());
            yield return null;
        }
        SceneTransition.main.StageClearReciever();
    }
}
public static class MyExtention
{
    public static Vector3 ToVector3(this Vector2 go, float z)
    {
        return new Vector3(go.x, go.y, z);
    }
}
