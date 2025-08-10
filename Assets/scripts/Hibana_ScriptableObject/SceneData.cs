using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scriptable Objects/SceneData")]
public class SceneData : ScriptableObject
{
    public bool deviceMode = false;
    public int currentScene = 0;
    public int majorScene = 10;
    public int resultScene = 15;
    public List<int> bossScene = new List<int>();
    public string stageLevel = "ãS";
    public SceneMode sceneMode;
    public SceneData copySource;
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
    public void ResetValues()
    {
        FieldInfo[] fields = this.GetType().GetFields();
        foreach (var field in fields)
        {
            field.SetValue(this, field.GetValue(copySource));
        }
    }
}
[CustomEditor(typeof(SceneData))]
public class SceneDataEditor : Editor
{
    ///<summary>
    ///InspectorÇÃGUIçXêV
    ///</summary>
    public override void OnInspectorGUI()
    {
        SceneData sceneData = target as SceneData;
        base.OnInspectorGUI();
        if (GUILayout.Button("ReplaceEveryValue"))
        {
            sceneData.ResetValues();
        }
    }
}
