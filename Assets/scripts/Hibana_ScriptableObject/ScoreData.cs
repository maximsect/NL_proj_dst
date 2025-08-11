using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
[System.Serializable]
public class SceneScore
{
    public float elapsedTime = 0;
    public int numberOfKill = 0;
    public int damageAmount = 0;
    public float sceneScore = 0;
}
[CreateAssetMenu(fileName = "ScoreData", menuName = "Scriptable Objects/ScoreData")]
public class ScoreData : ScriptableObject
{
    public List<SceneScore> scoreList = new List<SceneScore>();
    public ScoreData copySource;
    public int numberOfDeath = 0;

    public void ResetValues()
    {
        FieldInfo[] fields = this.GetType().GetFields();
        foreach (var field in fields)
        {
            field.SetValue(this, field.GetValue(copySource));
        }
    }
}
[CustomEditor(typeof(ScoreData))]
public class ScoreDataEditor : Editor
{
    ///<summary>
    ///InspectorÇÃGUIçXêV
    ///</summary>
    public override void OnInspectorGUI()
    {
        ScoreData scoreData = target as ScoreData;
        base.OnInspectorGUI();
        if (GUILayout.Button("ReplaceEveryValue"))
        {
            scoreData.ResetValues();
        }
    }
}
