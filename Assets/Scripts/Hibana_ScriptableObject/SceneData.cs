using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scriptable Objects/SceneData")]
public class SceneData : ScriptableObject
{
    public bool deviceMode = false;
    public int currentScene = 0;
    public int majorScene = 10;
    public int resultScene = 15;
    public List<int> bossScene = new List<int>();
    public string stageLevel = "";
    public List<Vector2> ratios = new List<Vector2>()
    {
        new Vector2(0.6f,0.7f),
        new Vector2(0.7f,0.8f),
        new Vector2(0.8f,1.2f),
        new Vector2(1.2f,1.4f),
        new Vector2(1.4f,1.7f)
    };
    public int GetStageLevel()
    {
        List<string> difficultyList = GameManager.syllabusLevel;
        return difficultyList.IndexOf(stageLevel);
    }
    public int StageCheck()
    {
#if UNITY_EDITOR
        if(!deviceMode)
        {
            currentScene = 0;
            return 0;
        }
        else
        {
            currentScene++;
            if (currentScene == majorScene) return 1;
            else if (currentScene == resultScene) return 2;
            else return 0;
        }
#else
        ccurrentScene++;
        if (currentScene == majorScene) return 1;
        else if (currentScene == resultScene) return 2;
        else return 0;
#endif
    }
}