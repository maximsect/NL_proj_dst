using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

public class FlagManager : MonoBehaviour
{
    public GameObject flagObjs;
    public GameObject enemyPref;
    public int numberOfFlags = 5;
    private Vector3 currentPosition = Vector3.zero;
    public Sprite attendFlagImage;
    void Start()
    {
        StartCoroutine(FlagGenerator());
    }
    Vector2 RandomPos()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Random.Range(-3f, 10f), Random.Range(-10f, 10f)); 
            if (Physics2D.OverlapCircle(pos, 0.5f)) continue;
            if (!Physics2D.OverlapCircle(pos - new Vector2(0, 0.3f), 0.5f)) continue;
            if (Vector3.Distance(currentPosition, pos) < 5) continue;
            currentPosition = pos;
            return pos;
        }
    }
    IEnumerator FlagGenerator()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < numberOfFlags; i++)
        {
            GameObject flag = Instantiate(flagObjs);
            FlagScript flagScript = flag.AddComponent<FlagScript>();
            bool isGeneratorable = false;
            Vector2 generatePosition = RandomPos();
            flag.transform.position = generatePosition;
            flagScript.attendFlagImage = attendFlagImage;

            for (int j = 0; j < 3; j++)
            {
                Instantiate(enemyPref, RandomPos().ToVector3(),transform.rotation);
            }
            yield return new WaitUntil(() => flagScript.isAttended);
        }
        SceneTransition.main.StageClearReciever();
    }
}