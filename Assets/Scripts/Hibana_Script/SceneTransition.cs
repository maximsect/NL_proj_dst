using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition main;
    public ScoreData scoreData;
    public SceneData sceneData;
    [HideInInspector] public bool clearFlag = false;

    public void E_SceneTransition(string name)
    {
        this.LoadSceneByName(name);
    }
    private void OnEnable()
    {
        if (main == null) main = this;
        StartCoroutine(AwaitStageClear());
    }
    private IEnumerator AwaitStageClear()
    {
        yield return new WaitUntil(() => clearFlag);
        clearFlag = false;
        yield return new WaitForSeconds(1f);
        switch (sceneData.StageCheck())
        {
            case 0:
                this.LoadSceneByName("SyllabusScene");
                break;
            case 1:
                this.LoadSceneByName("SelectMajorScene");
                break;
            case 2:
                this.LoadSceneByName("ResultScene");
                break;
            default:
                break;
        }
    }
    public void StageClearReciever()
    {
        Debug.Log("Admitted");
        clearFlag = true;
    }
    public void ChangeScoreData(int num)
    {
        scoreData.scores.Add(num);
        scoreData.sumScore = scoreData.scores.GetSum();
    }
}
public static class GameObjectExtention
{
    public static Vector3 ToVector3(this Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }
    public static void LoadSceneByName(this MonoBehaviour script, string name)
    {
        LoadScene(name);
    }
    public static void LoadSceneByName(this ScriptableObject script, string name)
    {
        LoadScene(name);
    }
    public static void LoadScene(string sceneName)
    {
        if (SceneExists(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.LogError($"ÉVÅ[Éì '{sceneName}' ÇÕë∂ç›ÇµÇ‹ÇπÇÒ");
        }
    }
    private static bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    public static int GetSum(this List<int> go)
    {
        int sum = 0;
        foreach (var item in go)
        {
            sum += item;
        }
        return sum;
    }
}
