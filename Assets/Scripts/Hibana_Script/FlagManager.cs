using UnityEngine;
using UnityEditor;
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
    private List<Vector2> usedPositions = new List<Vector2>() { Vector2.zero };
    public Sprite attendFlagImage;
    public LayerMask groundLayer;
    public List<GameObject> generatePoints = new List<GameObject>();
    void Start()
    {
        StartCoroutine(FlagGenerator());
    }
    Vector2 RandomPos()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Random.Range(-10f, 10f), Random.Range(-3f, 10f));
            if (Vector3.Distance(pos.ToVector3(), GameManager.player.transform.position) < 3) continue;
            if (Physics2D.OverlapCircle(pos, 0.5f, groundLayer)) continue;
            if (!Physics2D.OverlapCircle(pos - new Vector2(0, 0.3f), 0.5f, groundLayer)) continue;
            
            usedPositions.Add(pos);
            return pos;
        }
    }
    Vector2 SelectPos()
    {
        if (generatePoints.Count == 0) return Vector2.zero;
        else
        {
            GameObject target = generatePoints[Random.Range(0, generatePoints.Count)];

            generatePoints.Remove(target);
            return new Vector2(target.transform.position.x, target.transform.position.y);
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
            Vector2 generatePosition = SelectPos();
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
    public void CreateGeneratePoints()
    {
        GameObject obj = new GameObject("GeneratePoint:" + generatePoints.Count);
        generatePoints.Add(obj);
        GameObject parent = GameObject.Find("FlagManager");
        if (parent == null) parent = new GameObject("FlagManager");
        obj.transform.SetParent(parent.transform);
    }
    public void SortTheList()
    {
        generatePoints.RemoveAll(item => item == null);
    }
    private void OnDrawGizmos()
    {
        transform.position = Vector3.zero;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        foreach (var item in generatePoints)
        {
            GizmosUtility.DrawWireCircle(item.transform.position, 0.5f);
        }
    }
}
[CustomEditor(typeof(FlagManager))]
public class LocateFlagPoints : Editor
{
    public override void OnInspectorGUI()
    {
        FlagManager flagManager = target as FlagManager;
        base.OnInspectorGUI();
        if (GUILayout.Button("AddNewPoint"))
        {
            flagManager.CreateGeneratePoints();
        }
        if (GUILayout.Button("SortTheList"))
        {
            flagManager.SortTheList();
        }
    }
}
