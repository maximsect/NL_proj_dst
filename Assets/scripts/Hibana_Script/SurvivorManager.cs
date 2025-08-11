using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SurvivorManager : StageManager
{
    public LayerMask groundLayer;
    public List<GameObject> enemyPrefs = new List<GameObject>();
    private List<GameObject> enemyGen = new List<GameObject>();
    public float surviveTime = 30;
    public TMP_Text clock;
    void Start()
    {
        StartCoroutine(EnemyFactory());
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
    IEnumerator EnemyFactory()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            GameObject target = enemyPrefs[Random.Range(0, enemyPrefs.Count)];
            GameObject generated = Instantiate(target);
            generated.transform.position = RandomPos().ToVector3(0);
            enemyGen.Add(generated);
            while (true)
            {
                enemyGen.RemoveAll(item => item == null);
                if (enemyGen.Count < 10) break;
                yield return null;
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
    public static Vector3 ToVector3(this Vector2 go,float z)
    {
        return new Vector3(go.x, go.y, z);
    }
}
