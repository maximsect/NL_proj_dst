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
public class AssignmentManager : MonoBehaviour
{
    public LayerMask groundLayer;
    public List<AssignmentTarget> assignments = new List<AssignmentTarget>();
    public float generateInterval = 2;
    void Start()
    {
        StartCoroutine(EnemyFactory());
        StartCoroutine(Waiting());
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
            int assignIndex = Random.Range(0, assignments.Count);
            GameObject target = assignments[assignIndex].killTarget;
            GameObject generated = Instantiate(target);
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
                if (counter < 10) break;
                yield return null;
            }
        }
    }
    IEnumerator Waiting()
    {
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
        SceneTransition.main.StageClearReciever();
    }
    
}