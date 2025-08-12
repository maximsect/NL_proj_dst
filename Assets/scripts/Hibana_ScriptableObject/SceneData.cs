
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scriptable Objects/SceneData")]
public class SceneData : ScriptableObject
{
    public bool deviceMode = false;
    public int currentScene = 0;
    public int ProfessorScene = 7;
    public int nekomataScene = 8;
    public int majorScene = 10;
    public int resultScene = 15;
    public List<int> bossScene = new List<int>();
    public string stageLevel = "鬼";
    public SceneMode sceneMode;

    List<string> classDescription = new List<string>()
    {
        "ステージを攻略するだけで満点がくるおいしい授業\n評価法: 出席満点",
        "鬼は決して気を緩めない。\n敵の攻撃をよけろ。\n評価法: 被ダメ減点",
        "かすり傷でも、戦場では死に直結する。\n決して気を緩めるな。\n評価法: 一回でも攻撃に当たらないこと",
        "倒した敵の数こそが、鬼の強さを物語る。\n敵を殲滅せよ。\n評価法: 敵撃破数",
        "神出鬼没。鬼らしさを追及せよ。\n評価法: タイムアタック"

    };
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
    public string GetDescription()
    {
        return classDescription[(int)sceneMode];
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
            else if (currentScene == nekomataScene) return 3;
            else if (currentScene == ProfessorScene) return 4;
            else return 0;
        }
#else
        ccurrentScene++;
        if (currentScene == majorScene) return 1;
        else if (currentScene == resultScene) return 2;
        else return 0;
#endif
    }
    public void ResetValue()
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
    ///InspectorのGUI更新
    ///</summary>
    public override void OnInspectorGUI()
    {
        SceneData sceneData = target as SceneData;
        base.OnInspectorGUI();
        if (GUILayout.Button("ReplaceEveryValue"))
        {
            sceneData.ResetValue();
        }
    }
}
