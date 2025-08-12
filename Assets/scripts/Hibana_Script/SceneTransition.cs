using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition main;
    public ScoreData scoreData;
    public SceneData sceneData;
    [HideInInspector] public bool clearFlag = false;
    private float startTime = 0, endTime = 0;
    private SceneScore sceneScore = new SceneScore();
    private GameObject stageClearPanel;

    public void E_SceneTransition(string name)
    {
        this.LoadSceneByName(name);
    }
    private void Start()
    {
        if (main == null) main = this;
        StartCoroutine(AwaitStageClear());
        if (PlayerData.main != null)
            StartCoroutine(AwaitGameOver());
        if (SceneManager.GetActiveScene().name == "StartScene") scoreData.Initialize();
        stageClearPanel = scoreData.scoreDisplayer;
    }
    private IEnumerator AwaitStageClear()
    {
        startTime = Time.time;
        yield return new WaitUntil(() => clearFlag);
        clearFlag = false;
        endTime = Time.time;
        sceneScore.elapsedTime = endTime - startTime;
        ScoreCalculation();
        GameObject generatedPanel = Instantiate(stageClearPanel, GameObject.Find("Canvas").transform);
        TextMeshProUGUI stageDescription = generatedPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
        stageDescription.text =
            "経過時間" + (int)sceneScore.elapsedTime + "秒" +
            "\n倒した数:" + sceneScore.numberOfKill + "体" + 
            "\n与えたダメージ:" + sceneScore.damageAmount + "ダメージ" +
            "\n成績:" + sceneScore.sceneScore + "点";
        yield return new WaitForSecondsRealtime(1.5f);
        yield return StartCoroutine(WaitForClick());
        Time.timeScale = 1;
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
                case 3:
                this.LoadSceneByName("NekomataPreparationScene");
                break;
                case 4:
                this.LoadSceneByName("ProfessorPreparationScene");
                break;
            default:
                break;
        }
    }
    private IEnumerator AwaitGameOver()
    {
        yield return new WaitUntil(() => PlayerData.main.hp <= 0);
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        this.LoadSceneByName("ResultScene");
    }
    public void StageClearReciever()
    {
        Debug.Log("Admitted");
        clearFlag = true;
    }
    public void GetKill()
    {
        sceneScore.numberOfKill++;
    }
    public void DamageAmount(int delta)
    {
        sceneScore.damageAmount += delta;
    }
    public void Death()
    {
        scoreData.numberOfDeath++;
    }
    IEnumerator WaitForClick()
    {
        Time.timeScale = 0;
        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKey)
            {
                Time.timeScale = 1;
                yield break;
            }
            yield return null;
        }
        
    }

    /*
    public enum SceneMode
    {
        Complete, LessDamage, NoDamage, NumberOfKill, TimeAttack
    }
    SceneMode scene;/**/
    public void ScoreCalculation()
    {
        switch (sceneData.sceneMode)
        {
            case SceneMode.Complete:
                sceneScore.sceneScore = 100;
                break;
            case SceneMode.LessDamage:
                sceneScore.sceneScore = 100 * (float)PlayerData.main.hp / PlayerData.main.maxHp;
                break;
            case SceneMode.NoDamage:
                sceneScore.sceneScore = PlayerData.main.hp == PlayerData.main.maxHp ? 100 : 0;
                break;
            case SceneMode.NumberOfKill:
                sceneScore.sceneScore = Mathf.Clamp(50 + sceneScore.numberOfKill * 10, 50, 100);
                break;
            case SceneMode.TimeAttack:
                sceneScore.sceneScore = Mathf.Clamp(120 - (endTime - startTime) * 0.2f, 20, 100);
                break;
            default:
                break;
        }
        scoreData.scoreList.Add(sceneScore);
    }
    /*public void ChangeScoreData(int num)
    {
        scoreData.scores.Add(num);
        scoreData.sumScore = scoreData.scores.GetSum();
    }/**/
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
            //UnityEngine.Debug.LogError($"シ拏ン '{sceneName}' は存昊しません");
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
