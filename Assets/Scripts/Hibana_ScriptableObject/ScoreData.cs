
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
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
    public int numberOfDeath = 0;

    public void Initialize()
    {
        scoreList.Clear();
    }
}
