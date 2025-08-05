using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public enum GameMode
{
    Survival, Damage, Attendance, Attack, KillNumber, KillSpeed
}
public class SceneTransition : MonoBehaviour
{
    public static SceneTransition main;
    public PlayerData playerData;
    public ScoreData scoreData;
    public SceneData sceneData;
    public GameMode gameMode;
    public GameObject resultPanel;
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
        yield return new WaitForSeconds(0.2f);
        ChangeScoreData(GetScore());
        yield return new WaitForSecondsRealtime(3f);
        if (sceneData.StageCheck()) this.LoadSceneByName("SyllabusScene");
        else this.LoadSceneByName("ResultScene");
    }
    public void StageClearReciever()
    {
        clearFlag = true;
    }
    public void ChangeScoreData(int num)
    {
        scoreData.scores.Add(num);
        scoreData.sumScore = scoreData.scores.GetSum();
    }
    public int GetScore()
    {
        resultPanel.SetActive(true);
        switch (gameMode)
        {
            case GameMode.Survival:
                return 50 + (int)(50 * ((float)playerData.hp / playerData.maxHp));
            case GameMode.Damage:
                return 100 - (playerData.maxHp - playerData.hp);
            case GameMode.Attendance:
                return 100;
            case GameMode.Attack:
                return 100;
            case GameMode.KillNumber:
                return 100;
            case GameMode.KillSpeed:
                return 100;
            default:
                return 100;
        }
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