using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class AssignmentTarget
{
    public GameObject killTarget;
    [HideInInspector] public List<GameObject> genList = new List<GameObject>();
    public int assignmentKillNumber = 5;
    [HideInInspector] public int generatedNumber = 0;
}
public class AssignmentManager : StageManager
{
    public LayerMask groundLayer;
    public List<AssignmentTarget> assignments = new List<AssignmentTarget>();
    public float generateInterval = 2;
    public float appearMaxNum = 10;
    private List<int> genIndex = new List<int>();
    public AudioClip summonSound;
    void Start()
    {
        for (int i = 0; i < assignments.Count; i++)
        {
            for (int j = 0; j < assignments[i].assignmentKillNumber; j++)
            {
                genIndex.Add(i);
            }
        }
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
            yield return new WaitForSeconds(generateInterval);
            if (genIndex.Count == 0) yield break;
            int assignIndex = genIndex[Random.Range(0, genIndex.Count)];
            genIndex.Remove(assignIndex);
            GameObject target = assignments[assignIndex].killTarget;
            GameObject generated = Instantiate(target);
            GameManager.main.PlayOneShot(summonSound);
            generated.transform.position = RandomPos().ToVector3(0);
            assignments[assignIndex].genList.Add(generated);
            assignments[assignIndex].generatedNumber++;
            while (true)
            {
                int counter = 0;
                foreach (var assign in assignments)
                {
                    assign.genList.RemoveAll(item => item == null);
                    counter += assign.genList.Count;
                }
                if (counter < appearMaxNum) break;
                yield return null;
            }
        }
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
        float timer = 0;
        while (true) 
        { 
            bool assignmentClear = true;
            for (int i = 0; i < assignments.Count; i++)
            {
                assignments[i].genList.RemoveAll(item => item == null);
                assignmentClear = assignmentClear && assignments[i].generatedNumber - assignments[i].genList.Count >= assignments[i].assignmentKillNumber;

            }
            if (assignmentClear) break;
            yield return null;
        }
        GameManager.main.PlayOneShot(successSound);
        SceneTransition.main.StageClearReciever();
    }
}